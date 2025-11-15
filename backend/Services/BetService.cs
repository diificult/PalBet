using Hangfire;
using Hangfire.Client;
using Microsoft.AspNetCore.Identity;
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

        public BetService(IBetRepository betRepository, IAppUserRepository appUserRepository, INotificationService notificationService, UserManager<AppUser> userManager, IGroupRepository groupRepository )
        {
            _betRepository = betRepository;
            _userRepository = appUserRepository;
            _notificationService = notificationService;
            _userManager = userManager;
            _groupRepository = groupRepository;
        }

        public async Task<bool> AcceptBet(string userId, int betId)
        {
            var bet = await _betRepository.GetByIdAsync(betId);

            //keeping the checks seperate incase in future want to return the reason. Will have to explore this further.
            if (bet == null)
            {
                //No Bet exists
                throw new CustomException("Bet not found", "BET_NOTFOUND", 404);
            }
            //Check to see if it is they are included in the bet
            var betParticipant = bet.Participants.FirstOrDefault(b => b.appUserId == userId);
            if (betParticipant == null)
            {
                //There is no particpant with this id
                throw new CustomException("User is not a participant in this bet", "BET_USER_NOTPARTICIPANT", 404);
            }
            if (betParticipant.Accepted == true)
            {
                //Already accepted
                throw new CustomException("User has already accepted this bet", "BET_USER_ALREADYACCEPTED", 400);
            }

            betParticipant.Accepted = true;
            //Check to see if all participants have accepted
            if (!bet.Participants.Any(p => p.Accepted == false))
            {
                //Remove coins for each player
                if (bet.BetType == BetStakeType.Coins)
                {
                    foreach (BetParticipant p in bet.Participants)
                    {
                        var user = await _userRepository.GetUserAsync(p.appUserId);
                        var updatedCoins = user.PersonalCoins - bet.Coins;
                        if (updatedCoins < 0)
                        {
                            throw new CustomException($"User {user.UserName} does not have enough coins to accept the bet", "BET_INSUFFICIENT_COINS", 400);
                        }

                    }
                    await _userRepository.SaveAsync();

                }
                foreach (BetParticipant p in bet.Participants)
                {
                    await _notificationService.MarkAsComplete(p.appUserId, betId.ToString());
                    await _notificationService.CreateNotification(NotificationType.BetInPlay, betId.ToString(), p.appUserId);
                    if (bet.Deadline.HasValue) BackgroundJob.Schedule(() => _notificationService.CreateNotification(NotificationType.BetDeadlineReached, bet.Id.ToString(), p.appUserId), (DateTime) bet.Deadline);
                }

                await _betRepository.SaveAsync();
                bet.State = BetState.InPlay;

            }



            await _betRepository.SaveAsync();





            return true;


        }

        public async Task<bool> DeclineBet(string userId, int betId)
        {
            var bet = await _betRepository.GetByIdAsync(betId);

            //keeping the checks seperate incase in future want to return the reason. Will have to explore this further.
            if (bet == null)
            {
                //No Bet exists
                throw new CustomException("Bet not found", "BET_NOTFOUND", 404);

            }
            var betParticipant = bet.Participants.FirstOrDefault(b => b.appUserId == userId);
            if (betParticipant == null)
            {
                //There is no particpant with this id
                throw new CustomException("User is not a participant in this bet", "BET_USER_NOTPARTICIPANT", 404);
            }
            if (betParticipant.Accepted == true)
            {

                //Already accepted
                throw new CustomException("User has already accepted this bet", "BET_USER_ALREADYACCEPTED", 400);
            }
            if (bet.State != BetState.Requested)
            {
                //Bet is not in requested state
                throw new CustomException("Bet is not in requested state", "BET_INVALID_STATE", 400);
            }

            bet.State = BetState.Rejected;
            await _betRepository.SaveAsync();


            return true;


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
                    appUserId = user.Id,
                    isBetHost = username == betHost,
                    Accepted = username == betHost,
                });
            }
            participants = participants.DistinctBy(p => p.appUserId).ToList();

            Bet betModel = new Bet
            {
                Participants = participants,
                State = BetState.Requested,
                BetType = BetType,
                Coins = betDto.BetStakeCoins,
                UserInput = betDto.BetStakeUserInput,
                UserWinner = null,
                BetDescription = betDto.BetDescription,
                GroupId = betDto.GroupId,
                Deadline = betDto.Deadline,

            };

            


            //Check to see if all participants have enough coins.
            if (BetType == BetStakeType.Coins)
            {

                if (betDto.BetStakeCoins < 0) throw new CustomException("Bet coins must be greater than 0", "BET_INVALID_COINS", 400);
                foreach (var p in betModel.Participants)
                {

                    if (await _userRepository.GetCoins(p.appUserId) < betModel.Coins) throw new CustomException($"User {p.appUserId} does not have enough coins", "BET_INSUFFICIENT_COINS", 400);
                }
            }




            var createdBet = await _betRepository.CreateBet(betModel);
            foreach (BetParticipant bp in createdBet.Participants)
            {
                if (!bp.isBetHost) await _notificationService.CreateNotification(NotificationType.BetRequest, createdBet.Id.ToString(), bp.appUserId);
            }

            return createdBet;
        }

        public async Task<List<BetDto>?> GetBetRequests(string userId)
        {
            var bets = await _betRepository.GetBetRequests(userId);
            return bets.Select(bet => bet.ToBetDtoFromBets(userId)).ToList();
        }

        public async Task<List<BetDto>?> GetBetsByState(string userId, BetState? betState)
        {
            var bets = await _betRepository.GetUsersBets(userId);
            if (betState == null) return null;

            return bets.Where(b => b.State == betState).ToList().Select(bet => bet.ToBetDtoFromBets(userId)).ToList();
        }

        public Task<List<Bet>?> GetRequestedBets(string userId)
        {
            return _betRepository.GetRequestedBets(userId);
        }

        public async Task SetWinner(string winnerUserId, string updaterUserId, int betId)
        {
            var bet = await _betRepository.GetByIdAsync(betId);

            //Check to see if the person updating it is the person who should be.
            if (!bet.Participants.FirstOrDefault(p => p.appUserId == updaterUserId).isBetHost) throw new CustomException("Only the bet host can set the winner", "BET_SETWINNER_INVALIDUSER", 400);

            //Check to see if the bet is still in play.
            if (bet.State != BetState.InPlay) throw new CustomException("Bet is not in play", "BET_SETWINNER_INVALIDSTATE", 400);

            //Check to see if the winner id exists in the bet
            if (!bet.Participants.Any(p => p.appUserId == winnerUserId)) throw new CustomException("Winner is not a participant in the bet", "BET_SETWINNER_INVALIDWINNER", 400);

            //Otherwise update the bet
            foreach (BetParticipant bp in bet.Participants)
            {
                await _notificationService.MarkAsComplete(bp.appUserId, betId.ToString());
                await _notificationService.CreateNotification(NotificationType.WinnerChosen, betId.ToString(), bp.appUserId);
            }

            bet.UserWinner = winnerUserId;
            bet.State = BetState.Completed;


            await _betRepository.SaveAsync();

            if (bet.BetType == BetStakeType.Coins)
            {
                if (bet.IsGroup)
                {
                    var group = await _groupRepository.GetGroupAsync((int)bet.GroupId);
                    group.UserGroups.FirstOrDefault(ug => ug.UserId == winnerUserId).CoinBalance += (int)(bet.Coins * 0.95f);
                    await _groupRepository.SaveAsync();
                }
                else
                {
                    await _userRepository.UpdateCoins(winnerUserId, betId);
                }

            }

        }

        public async Task<bool> DeleteBet(string userId, int betId)
        {

            //UNUSED
            var bet = await _betRepository.GetByIdAsync(betId);
            return false;
        }

        public async Task<bool> CancelBet(string userId, int betId)
        {
            var bet = await _betRepository.GetByIdAsync(betId);
            if (bet == null) return false;

            //Check to see if the person updating it is the person who should be.

            if (!bet.Participants.FirstOrDefault(p => p.appUserId == userId).isBetHost) return false;
            // Check to see if bet is in requeted state.
            if (bet.State != BetState.Requested) return false;

            bet.State = BetState.Cancelled;
            await _betRepository.SaveAsync();

            return true;

        }

        public async Task<BetDto> GetBetById(string userId, int betId)
        {

            var bet = await _betRepository.GetByIdAsync(betId);
            if (bet.Participants.Where(p => p.appUserId == userId).Any())
            {
                return bet.ToBetDtoFromBets(userId);
            }
            else return null;
        }
    }


}
