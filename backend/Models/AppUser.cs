using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PalBet.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<BetParticipant> BetsParticipation { get; set; } = new List<BetParticipant>();

        public int PersonalCoins { get; set; } = 50;

        public ICollection<Friendship> friendshipsInstantiated { get; set; } = new List<Friendship>();
        public ICollection<Friendship> friendshipsRecieved { get; set; } = new List<Friendship>();

        public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
    }
}
