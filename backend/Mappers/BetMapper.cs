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
                GroupId = dto.GroupId,
                AllowMultipleWinners = dto.AllowMultipleWinners,
                BurnStakeOnNoWnner = dto.BurnStakeOnNoWinner,
                BetStakeType = dto.BetStakeUserInput != null ? BetStakeType.UserInput : BetStakeType.Coins,
            };
        }

            
        public static BetDto ToBetDtoFromBets(this Bet bet, string userId)
        {
            return new BetDto
            {
                BetId = bet.Id,
                ParticipantNames = bet.Participants?.Select(b => b.AppUser?.UserName).ToList(),
                BetDescription = bet.BetDescription,
                UserWinner = bet.Participants.Where(p => p.IsWinner == true).Select(p => p.AppUser.UserName).ToList(),
                BetState = bet.State.ToString(),
                BetStake = bet.BetStakeType == BetStakeType.UserInput ? bet.UserInput : bet.Coins.ToString() + " Coins",
                IsHost = bet.Participants.Last().AppUserId == userId,
                Deadline = bet.Deadline?.ToString("dd/MM/yy HH:mm"),
                GroupName = bet.Group?.Name,
                AllowMultipleWinners = bet.AllowMultipleWinners,
                BurnStakeOnNoWnner = bet.BurnStakeOnNoWnner,
                OutcomeChoice = bet.OutcomeChoice,
                Choices = bet.Choices?.Select(c => c.Text).ToList(),



            };
        }
    }
}
