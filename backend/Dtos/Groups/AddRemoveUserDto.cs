using System.ComponentModel.DataAnnotations;

namespace PalBet.Dtos.Groups
{
    public class AddRemoveUserDto
    {
        [Required]
        public int GroupId { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
