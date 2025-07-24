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

        //1 or 2 or 0 for no current winners
        public int UserWinner { get; set; } = 0;
        [Required]
        public BetState state { get; set; }
        public int BetStake { get; set; }
    }
}
