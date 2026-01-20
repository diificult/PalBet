using System.ComponentModel.DataAnnotations;

namespace PalBet.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        [StringLength(maximumLength: 15, MinimumLength = 5)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
