using System.ComponentModel.DataAnnotations;

namespace PalBet.Models
{
    public class UserGroup
    {

        [Required]
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public bool IsAdmin { get; set; } = false;

        public int CoinBalance { get; set; } = 0;

}
}
