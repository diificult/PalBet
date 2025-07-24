using Microsoft.AspNetCore.Mvc;
using PalBet.Dtos;
using PalBet.Enums;
using PalBet.Interfaces;
using PalBet.Models;

namespace PalBet.Services
{
    public class BetService : IBetService
    {

        public readonly IBetRepository _betRepository;

        public BetService(IBetRepository betRepository)
        {
            _betRepository = betRepository;
        }


        public async Task<Bet> CreateBet(CreateBetDto betDto)
        {

            Bet betModel = new Bet
            {
                Participants = betDto.ParticipantIds.Select(userId => new BetParticipant
                {
                    appUserId = userId,
                }).ToList(),
                state = betDto.state,
                BetStake = betDto.BetStake,
                UserWinner = betDto.UserWinner,
                BetDescription = betDto.BetDescription,

            };




            //To do: Validation
            return await _betRepository.CreateBet(betModel);
        }  

    }
}
