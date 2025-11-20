using Microsoft.Identity.Client;

namespace PalBet.Models
{
    public class BetParticipant
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int BetId { get; set; }
        public Bet Bet { get; set;  }

        public bool Accepted { get; set; }
        public bool IsBetHost { get; set; }

        public bool IsWinner { get; set; } = false;

        public int? SelectedChoiceId { get; set; }
        public BetChoice? SelectedChoice { get; set; }
    }
}
