using Microsoft.AspNetCore.Mvc;
using PalBet.Enums;
using PalBet.Models;
using System.ComponentModel.DataAnnotations;

namespace PalBet.Dtos
{
    public class CreateBetDto 
    {
        [Required]
        public List<string> ParticipantIds { get; set ; }
        [Required]
        public string BetDescription { get; set; }
        public string UserWinner { get; set; } = string.Empty;
        [Required]
        public BetState state { get; set; }
        public int BetStake { get; set; }
    }
}
