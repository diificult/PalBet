namespace PalBet.Models
{
    public class BetParticipant
    {
        public string appUserId { get; set; }
        public AppUser appUser { get; set; }
        public int betId { get; set; }
        public Bet bet { get; set;  }

        public bool Accepted { get; set; }
        public bool isBetHost { get; set; }
    }
}
