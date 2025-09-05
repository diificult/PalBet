using PalBet.Enums;

namespace PalBet.Models
{
    public class Friendship
    {
        public string RequesterId { get; set; } 
        public AppUser Requester { get; set; }

        public string RequesteeId { get; set; }
        public AppUser Requestee { get; set; }

        public FriendshipState state { get; set; } = FriendshipState.Requested;

        public DateTime FriendshipTime { get; set; } = DateTime.Now;


    }
}
