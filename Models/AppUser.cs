using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PalBet.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<BetParticipant> BetsParticipation { get; set; } = new List<BetParticipant>();
    }
}
