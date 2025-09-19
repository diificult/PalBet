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

            };
        }

            
        public static BetDto toBetDtoFromBets(this Bet bet, string userId)
        {
            
            return new BetDto
            {
                BetId = bet.Id,
                ParticipantNames = bet.Participants?.Select(b => b.appUser?.UserName).ToList(),
                BetDescription = bet.BetDescription,
                UserWinner = bet.UserWinner,
                BetState = bet.state,
                BetStake = bet.BetStake,
                isHost = bet.Participants.Last().appUserId == userId,

            };
        }
    }
}
