using Microsoft.AspNetCore.Mvc;
using PalBet.Dtos.Bet;
using PalBet.Enums;
using PalBet.Interfaces;
using PalBet.Models;
using System.Data.SqlTypes;

namespace PalBet.Services
{
    public class BetService : IBetService
    {

        public readonly IBetRepository _betRepository;

        public BetService(IBetRepository betRepository)
        {
            _betRepository = betRepository;
        }

        public async Task<bool> AcceptBet(string userId, int betId)
        {
            var bet = await _betRepository.GetByIdAsync(betId);

            //keeping the checks seperate incase in future want to return the reason. Will have to explore this further.
            if (bet == null )
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
            await _betRepository.SaveAsync();


            //TODO: Check to see if the bet has been accepted by everyone.

            return true;
            

        }

        public async Task<Bet> CreateBet(CreateBetDto betDto, string betHost)
        {

            Bet betModel = new Bet
            {
                Participants = betDto.ParticipantIds.Select(userId => new BetParticipant
                {
                    appUserId = userId,
                    isBetHost = userId == betHost,
                    Accepted = userId == betHost,
                }).ToList(),

                state = betDto.state,
                BetStake = betDto.BetStake,
                UserWinner = betDto.UserWinner,
                BetDescription = betDto.BetDescription,

            };
            




            //To do: Validation
            return await _betRepository.CreateBet(betModel);
        }

        public async Task<List<Bet>?> GetBetRequests(string userId)
        {
            return await _betRepository.GetBetRequests(userId);
        }

        public async Task<List<Bet>?> GetBetsByState(string userId, BetState? betState)
        {
            var bets = await _betRepository.GetUsersBets(userId);
            if (betState == null) return null;
            return bets.Where(b => b.state == betState).ToList();
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
            if (bet.state != BetState.InPlay) return false;

            //Check to see if the winner id exists in the bet
            if (!bet.Participants.Any(p => p.appUserId == winnerUserId)) return false;

            //Otherwise update the bet

            bet.UserWinner = winnerUserId;
            bet.state = BetState.Completed;

            await _betRepository.SaveAsync();

            return true;
            



        }
    }

    
}
