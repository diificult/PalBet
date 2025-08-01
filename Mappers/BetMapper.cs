using Azure.Core;
using PalBet.Dtos.Bet;
using PalBet.Enums;
using PalBet.Models;

namespace PalBet.Mappers
{
    public static class BetMapper
    {

        public static Bet ToBetFromCreateBetDto(this CreateBetDto dto)
        {
            return new Bet
            {
             //   BetParticipants = request.ParticipantIds.Select(userId => new BetParticipant
              //  {
                 //   AppUserId = userId
              //  }).ToList()
                BetDescription = dto.BetDescription,
                UserWinner = dto.UserWinner,

            };
        }

    }
}
