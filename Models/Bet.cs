using PalBet.Enums;

namespace PalBet.Models
{
    public class Bet
    {
        public int Id { get; set; }
        public ICollection<BetParticipant> Participants { get; set; } = new List<BetParticipant>();
        public string BetDescription { get; set; }
        //1 or 2
        public int UserWinner { get; set; }
        public BetState state { get; set; }
        public int BetStake { get; set; }

    }
}
