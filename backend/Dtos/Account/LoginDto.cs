using PalBet.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace PalBet.Dtos.Account
{
    public class LoginDto
    {
        [Required]
        [StringLength(maximumLength: 15, MinimumLength = 5)]
        public string Username { get; set; }
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
