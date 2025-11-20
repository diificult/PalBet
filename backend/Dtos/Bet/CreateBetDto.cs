using Microsoft.AspNetCore.Mvc;
using PalBet.Enums;
using PalBet.Models;
using System.ComponentModel.DataAnnotations;

namespace PalBet.Dtos.Bet
{
    public class CreateBetDto
    {
        [Required]
        public List<string> ParticipantUsernames { get; set; }
        [Required]
        public string BetDescription { get; set; }
        public int? BetStakeCoins { get; set; }

        public string? BetStakeUserInput { get; set; }
        
        public DateTime? Deadline { get; set; }

        public int? GroupId { get; set; }

        public bool AllowMultipleWinners { get; set; }
        public bool BurnStakeOnNoWinner { get; set; }

        public List<string>? ChoicesText { get; set; }

        public OutcomeChoice OutcomeChoice { get; set; }


    }
}
