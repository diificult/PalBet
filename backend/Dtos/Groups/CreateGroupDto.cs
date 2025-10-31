using System.ComponentModel.DataAnnotations;

namespace PalBet.Dtos.Groups
{
    public class CreateGroupDto
    {

        [Required]
        public string GroupName { get; set; }

        public int? DefaultCoinBalance { get; set; }

        [Required]
        public List<string> GroupUsernames { get; set; }
    }
}
