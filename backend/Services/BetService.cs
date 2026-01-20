using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PalBet.Dtos.Bet;
using PalBet.Enums;
using PalBet.Exceptions;
using PalBet.Interfaces;
using PalBet.Mappers;
using PalBet.Models;

namespace PalBet.Services
{
    public class BetService : IBetService
    {

        private readonly IBetRepository _betRepository;
        private readonly IAppUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGroupRepository _groupRepository;
        private readonly IRedisBetsCacheService _redisBetsCacheService;

        public BetService(IBetRepository betRepository, IAppUserRepository appUserRepository, INotificationService notificationService, UserManager<AppUser> userManager, IGroupRepository groupRepository, IRedisBetsCacheService redisBetsCacheService)
        {
            _betRepository = betRepository;
            _userRepository = appUserRepository;
            _notificationService = notificationService;
            _userManager = userManager;
            _groupRepository = groupRepository;
            _redisBetsCacheService = redisBetsCacheService;
        }

        public async Task AcceptBet(string userId, int betId, string? choice)
        {
            var bet = await _betRepository.GetByIdAsync(betId);

            //keeping the checks seperate incase in future want to return the reason. Will have to explore this further.
            if (bet == null)
            {
                //No Bet exists
                throw new CustomException("Bet not found", "BET_NOTFOUND", 404);
            }
            //Check to see if it is they are included in the Bet
            var betParticipant = bet.Participants.FirstOrDefault(b => b.AppUserId == userId);
            if (betParticipant == null)
            {
                //There is no particpant with this id
                throw new CustomException("User is not a participant in this Bet", "BET_USER_NOTPARTICIPANT", 404);
            }
            if (betParticipant.Accepted == true)
            {
                //Already accepted
                throw new CustomException("User has already accepted this Bet", "BET_USER_ALREADYACCEPTED", 400);
            }

            if (bet.OutcomeChoice == OutcomeChoice.UserSubmitted)
            {
                if (string.IsNullOrEmpty(choice))
                {
                    throw new CustomException("A choice must be provided to accept this Bet", "BET_USER_CHOICE_REQUIRED", 400);
                }
                var createdChoice = new BetChoice { Text = choice };
                bet.Choices.Add(createdChoice);
                betParticipant.SelectedChoice = createdChoice;
            }
            else if (bet.OutcomeChoice == OutcomeChoice.HostDefined)
            {
                if (string.IsNullOrEmpty(choice))
                {
                    throw new CustomException("A choice must be provided to accept this Bet", "BET_USER_CHOICE_REQUIRED", 400);
                }
                var selectedChoice = bet.Choices.FirstOrDefault(c => c.Text == choice);
                if (selectedChoice == null)
                {
                    throw new CustomException("The provided choice is not valid for this Bet", "BET_USER_INVALID_CHOICE", 400);
                }
                betParticipant.SelectedChoice = selectedChoice;
            }

            betParticipant.Accepted = true;
            //Check to see if all participants have accepted
            if (!bet.Participants.Any(p => p.Accepted == false))
            {
                //Remove coins for each player
                if (bet.BetStakeType == BetStakeType.Coins)
                {

                    foreach (BetParticipant p in bet.Participants)
                    {
                        var current = await _userRepository.GetCoins(p.AppUserId);
                        if (current < bet.Coins)
                            throw new CustomException($"User {p.AppUser.UserName} does not have enough coins to accept the Bet", "BET_INSUFFICIENT_COINS", 400);

                    }

                    foreach (BetParticipant p in bet.Participants)
                    {
                        await _userRepository.UpdateCoins(p.AppUserId, -(int)bet.Coins);
                    }

                    await _userRepository.SaveAsync();

                }
                foreach (BetParticipant p in bet.Participants)
                {
                    await _notificationService.MarkAsComplete(p.AppUserId, betId.ToString());
                    await _notificationService.CreateNotification(NotificationType.BetInPlay, betId.ToString(), p.AppUserId);
                    if (bet.Deadline.HasValue) BackgroundJob.Schedule(() => _notificationService.CreateNotification(NotificationType.BetDeadlineReached, bet.Id.ToString(), p.AppUserId), (DateTime)bet.Deadline);
                }

                await _betRepository.SaveAsync();
                bet.State = BetState.InPlay;

            }

            await _betRepository.SaveAsync();

            await InvalidateCache(bet.Participants.ToList(), "Requested");
            await InvalidateCache(bet.Participants.ToList(), "Requests");


        }

        public async Task DeclineBet(string userId, int betId)
        {
            var bet = await _betRepository.GetByIdAsync(betId);

            //keeping the checks seperate incase in future want to return the reason. Will have to explore this further.
            if (bet == null)
            {
                //No Bet exists
                throw new CustomException("Bet not found", "BET_NOTFOUND", 404);

            }
            var betParticipant = bet.Participants.FirstOrDefault(b => b.AppUserId == userId);
            if (betParticipant == null)
            {
                //There is no particpant with this id
                throw new CustomException("User is not a participant in this Bet", "BET_USER_NOTPARTICIPANT", 404);
            }
            if (betParticipant.Accepted == true)
            {

                //Already accepted
                throw new CustomException("User has already accepted this Bet", "BET_USER_ALREADYACCEPTED", 400);
            }
            if (bet.State != BetState.Requested)
            {
                //Bet is not in requested state
                throw new CustomException("Bet is not in requested state", "BET_INVALID_STATE", 400);
            }



            bet.State = BetState.Rejected;
            await _betRepository.SaveAsync();

            await InvalidateCache(bet.Participants.ToList(), BetState.Requested.ToString());


        }

        public async Task<Bet> CreateBet(CreateBetDto betDto, string betHost)
        {
            var participants = new List<BetParticipant>();

            //Error as only one of these should be filled out.
            if (betDto.BetStakeUserInput != null && betDto.BetStakeCoins != null) return null;
            if (betDto.BetStakeUserInput == null && betDto.BetStakeCoins == null) return null;

            BetStakeType BetType = betDto.BetStakeUserInput != null ? BetStakeType.UserInput : BetStakeType.Coins;

            foreach (var username in betDto.ParticipantUsernames)
            {
                var user = await _userManager.FindByNameAsync(username) ?? throw new CustomException($"User {username} not found", "BET_USER_NOTFOUND", 404);
                participants.Add(new BetParticipant
                {
                    AppUserId = user.Id,
                    AppUser = user,
                    IsBetHost = username == betHost,
                    Accepted = false,
                });
            }
            participants = participants.DistinctBy(p => p.AppUserId).ToList();

            if (betDto.OutcomeChoice == OutcomeChoice.HostDefined && betDto.ChoicesText.Count < 2) throw new CustomException("At least two choices are required for host defined bets", "BET_INVALID_CHOICES", 400);

            List<BetChoice> choices = new List<BetChoice>();

            if (betDto.OutcomeChoice == OutcomeChoice.ParticipantAssigned)
            {

                foreach (var p in participants)
                {
                    var choice = new BetChoice
                    {
                        Text = p.AppUser.UserName + " wins"
                    };

                    choices.Add(choice);
                    p.SelectedChoice = choice;

                }

            }
            else if (betDto.OutcomeChoice == OutcomeChoice.HostDefined)
            {
                choices = betDto.ChoicesText.Select(c => new BetChoice { Text = c }).ToList();
            }
            if (betDto.OutcomeChoice == OutcomeChoice.UserSubmitted)
            {
                choices = new List<BetChoice>();
            }

            Bet betModel = new Bet
            {
                Participants = participants,
                State = BetState.Requested,
                BetStakeType = BetType,
                Coins = betDto.BetStakeCoins,
                UserInput = betDto.BetStakeUserInput,
                BetDescription = betDto.BetDescription,
                GroupId = betDto.GroupId,
                Deadline = betDto.Deadline,
                AllowMultipleWinners = betDto.AllowMultipleWinners,
                OutcomeChoice = betDto.OutcomeChoice,
                BurnStakeOnNoWnner = betDto.BurnStakeOnNoWinner,

                Choices = choices
            };

            //Check to see if all participants have enough coins.
            if (BetType == BetStakeType.Coins)
            {

                if (betDto.BetStakeCoins < 0) throw new CustomException("Bet coins must be greater than 0", "BET_INVALID_COINS", 400);
                foreach (var p in betModel.Participants)
                {

                    if (await _userRepository.GetCoins(p.AppUserId) < betModel.Coins) throw new CustomException($"User {p.AppUserId} does not have enough coins", "BET_INSUFFICIENT_COINS", 400);
                }
            }

            var createdBet = await _betRepository.CreateBet(betModel);
            foreach (BetParticipant bp in createdBet.Participants)
            {
                if (!bp.IsBetHost) await _notificationService.CreateNotification(NotificationType.BetRequest, createdBet.Id.ToString(), bp.AppUserId);
            }
            await InvalidateCache(betModel.Participants.ToList(), "Requested");
            await InvalidateCache(betModel.Participants.ToList(), "Requests");

            return createdBet;
        }

        public async Task<List<BetDto>?> GetBetRequests(string userId)
        {

            var bets = await _redisBetsCacheService.GetBetsByStateAsync(userId, "Requests");
            if (!bets.IsNullOrEmpty()) return bets;

            bets = (await _betRepository.GetBetRequests(userId)).Select(bet => bet.ToBetDtoFromBets(userId)).ToList();

            await _redisBetsCacheService.SetBetsByStateAsync(userId, "Requests", bets);

            return bets;

        }

        public async Task<List<BetDto>?> GetBetsByState(string userId, BetState? betState)
        {
            if (betState == null) return null;

            var bets = await _redisBetsCacheService.GetBetsByStateAsync(userId, betState.ToString());
            if (!bets.IsNullOrEmpty()) return bets;

            bets = (await _betRepository.GetUsersBets(userId)).Where(b => b.State == betState).Select(bet => bet.ToBetDtoFromBets(userId)).ToList();

            await _redisBetsCacheService.SetBetsByStateAsync(userId, betState.ToString(), bets);

            return bets;
        }

        public async Task DeclareWinner(string updaterUserId, int betId, int winningChoiceId)
        {
            var bet = await _betRepository.GetByIdAsync(betId) ?? throw new CustomException("Bet not found", "BET_NOTFOUND", 404);
            //Check to see if the person updating it is the person who should be.
            if (!bet.Participants.FirstOrDefault(p => p.AppUserId == updaterUserId).IsBetHost) throw new CustomException("Only the Bet host can set the winner", "BET_SETWINNER_INVALIDUSER", 400);
            //Check to see if the Bet is still in play.
            if (bet.State != BetState.InPlay) throw new CustomException("Bet is not in play", "BET_SETWINNER_INVALIDSTATE", 400);

            var betWinners = bet.Participants.Where(p => p.SelectedChoiceId == winningChoiceId).ToList();

            foreach (BetParticipant bp in bet.Participants)
            {
                await _notificationService.MarkAsComplete(bp.AppUserId, betId.ToString());
                await _notificationService.CreateNotification(NotificationType.WinnerChosen, betId.ToString(), bp.AppUserId);
            }

            bet.State = BetState.Completed;

            if (bet.BetStakeType == BetStakeType.UserInput) return;

            if (betWinners.Count == 0)
            {
                if (!bet.BurnStakeOnNoWnner)
                {
                    //Refund all players if no winner and not burning stakes
                    bet.Participants.ToList().ForEach(async participants => await _userRepository.UpdateCoins(participants.AppUserId, (int)bet.Coins));
                    return;
                }
                else return;
            }

            int winningAmount = (int)(bet.Coins * bet.Participants.Count * 0.95f);

            if (bet.IsGroup)
            {
                var group = await _groupRepository.GetGroupAsync((int)bet.GroupId);
                betWinners.ForEach(winner => group.UserGroups.FirstOrDefault(ug => ug.UserId == winner.AppUserId).CoinBalance += winningAmount);
                await _groupRepository.SaveAsync();
            }
            else
            {
                betWinners.ForEach(async winner =>
                {
                    await _userRepository.UpdateCoins(winner.AppUserId, winningAmount);
                    bet.Participants.FirstOrDefault(p => p.AppUserId == winner.AppUserId).IsWinner = true;
                });
            }

            await _betRepository.SaveAsync();

            await InvalidateCache(bet.Participants.ToList(), BetState.InPlay.ToString());
            await InvalidateCache(bet.Participants.ToList(), BetState.Completed.ToString());

        }

        public async Task CancelBet(string userId, int betId)
        {
            var bet = await _betRepository.GetByIdAsync(betId);
            if (bet == null) throw new CustomException("Bet not found", "BET_NOTFOUND", 404);

            //Check to see if the person updating it is the person who should be.

            if (!bet.Participants.FirstOrDefault(p => p.AppUserId == userId).IsBetHost) throw new CustomException("Only the Bet host can cancel the Bet", "BET_CANCEL_INVALIDUSER", 400);
            // Check to see if Bet is in requeted state.
            if (bet.State != BetState.Requested) throw new CustomException("Bet is not in requested state", "BET_CANCEL_INVALIDSTATE", 400);

            bet.State = BetState.Cancelled;
            await _betRepository.SaveAsync();

        }

        public async Task<BetDto> GetBetById(string userId, int betId)
        {

            var bet = await _betRepository.GetByIdAsync(betId);
            if (bet == null ) throw new CustomException("Bet not found", "BET_NOTFOUND", 404);
            if (bet.Participants.Where(p => p.AppUserId == userId).Any())
            {
                return bet.ToBetDtoFromBets(userId);
            }
            else return null;
        }

        private async Task InvalidateCache(List<BetParticipant> participants, string betState)
        {
            foreach (BetParticipant p in participants)
            {
                await _redisBetsCacheService.InvalidateBetsAsync(p.AppUserId, betState);
            }
        }
    }


}
