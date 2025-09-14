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
        public int BetStake { get; set; }
    }
}
