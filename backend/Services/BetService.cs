using Microsoft.AspNetCore.Identity;
using PalBet.Dtos.Bet;
using PalBet.Enums;
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

        public BetService(IBetRepository betRepository, IAppUserRepository appUserRepository, INotificationService notificationService, UserManager<AppUser> userManager)
        {
            _betRepository = betRepository;
            _userRepository = appUserRepository;
            _notificationService = notificationService;
            _userManager = userManager;
        }

        public async Task<bool> AcceptBet(string userId, int betId)
        {
            var bet = await _betRepository.GetByIdAsync(betId);

            //keeping the checks seperate incase in future want to return the reason. Will have to explore this further.
            if (bet == null)
            {
                //No Bet exists
                return false;
            }
            //Check to see if it is they are included in the bet
            var betParticipant = bet.Participants.FirstOrDefault(b => b.appUserId == userId);
            if (betParticipant == null)
            {
                //There is no particpant with this id
                return false;
                //To do, return reason?
            }
            if (betParticipant.Accepted == true)
            {
                return false;
                //Already accepted
            }

            betParticipant.Accepted = true;
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
                            return false;
                        }

                    }
                }
                foreach (BetParticipant p in bet.Participants)
                {
                    await _notificationService.MarkAsComplete(p.appUserId, betId.ToString());
                    await _notificationService.CreateNotification(NotificationType.BetInPlay, betId.ToString(), p.appUserId);
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
                return false;

            }
            var betParticipant = bet.Participants.FirstOrDefault(b => b.appUserId == userId);
            if (betParticipant == null)
            {
                //There is no particpant with this id
                return false;
                //To do, return reason?
            }
            if (betParticipant.Accepted == true)
            {
                return false;
                //Already accepted
            }
            if (bet.State != BetState.Requested)
            {
                return false;
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

            BetStakeType BetType = betDto.BetStakeUserInput != null ? BetStakeType.UserInput : BetStakeType.Coins;

            foreach (var username in betDto.ParticipantUsernames)
            {
                var user = await _userManager.FindByNameAsync(username);

                participants.Add(new BetParticipant
                {
                    appUserId = user.Id,
                    isBetHost = username == betHost,
                    Accepted = username == betHost,
                });
            }

            Bet betModel = new Bet
            {
                Participants = participants,
                State = BetState.Requested,
                BetType = BetType,
                Coins = betDto.BetStakeCoins,
                UserInput = betDto.BetStakeUserInput,
                UserWinner = null,
                BetDescription = betDto.BetDescription,

            };


            //Check to see if all participants have enough coins.
            if (BetType == BetStakeType.Coins)
            {
                foreach (var p in betModel.Participants)
                {
                    if (await _userRepository.GetCoins(p.appUserId) < betModel.Coins) { return null; }
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
            return bets.Select(bet => bet.toBetDtoFromBets(userId)).ToList();
        }

        public async Task<List<BetDto>?> GetBetsByState(string userId, BetState? betState)
        {
            var bets = await _betRepository.GetUsersBets(userId);
            if (betState == null) return null;

            return bets.Where(b => b.State == betState).ToList().Select(bet => bet.toBetDtoFromBets(userId)).ToList();
        }

        public Task<List<Bet>?> GetRequestedBets(string userId)
        {
            return _betRepository.GetRequestedBets(userId);
        }

        public async Task<bool> SetWinner(string winnerUserId, string updaterUserId, int betId)
        {
            var bet = await _betRepository.GetByIdAsync(betId);

            //Check to see if the person updating it is the person who should be.

            if (!bet.Participants.FirstOrDefault(p => p.appUserId == updaterUserId).isBetHost) return false;

            //Check to see if the bet is still in play.
            if (bet.State != BetState.InPlay) return false;

            //Check to see if the winner id exists in the bet
            if (!bet.Participants.Any(p => p.appUserId == winnerUserId)) return false;

            //Otherwise update the bet

            await _userRepository.UpdateCoins(winnerUserId, betId);
            bet.UserWinner = winnerUserId;
            bet.State = BetState.Completed;


            await _betRepository.SaveAsync();
            foreach (BetParticipant bp in bet.Participants)
            {
                await _notificationService.MarkAsComplete(bp.appUserId, betId.ToString());
                await _notificationService.CreateNotification(NotificationType.WinnerChosen, betId.ToString(), bp.appUserId);
            }

            return true;




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
                return bet.toBetDtoFromBets(userId);
            }
            else return null;
        }
    }


}
