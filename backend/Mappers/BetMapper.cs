using Azure.Core;
using PalBet.Dtos.Bet;
using PalBet.Enums;
using PalBet.Models;
using System.Globalization;

namespace PalBet.Mappers
{
    public static class BetMapper
    {

        public static Bet ToBetFromCreateBetDto(this CreateBetDto dto)
        {
            return new Bet
            {
                BetDescription = dto.BetDescription,
                Deadline = dto.Deadline,
            };
        }

            
        public static BetDto ToBetDtoFromBets(this Bet bet, string userId)
        {

            return new BetDto
            {
                BetId = bet.Id,
                ParticipantNames = bet.Participants?.Select(b => b.appUser?.UserName).ToList(),
                BetDescription = bet.BetDescription,
                UserWinner = bet.UserWinner,
                BetState = bet.State.ToString(),
                BetStake = bet.BetType == BetStakeType.UserInput ? bet.UserInput : bet.Coins.ToString() + " Coins",
                isHost = bet.Participants.Last().appUserId == userId,
                Deadline = bet.Deadline?.ToString("dd/MM/yy HH:mm"),
                GroupName = bet.Group?.Name,

            };
        }
    }
}
