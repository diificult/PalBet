using PalBet.Enums;

namespace PalBet.Models
{
    public class Bet
    {
        public int Id { get; set; }
        public ICollection<BetParticipant> Participants { get; set; } = new List<BetParticipant>();
        public string BetDescription { get; set; }
        public string? UserWinner { get; set; } = string.Empty;
        public BetState State { get; set; }
        public BetStakeType BetType { get; set; }
        public int? Coins { get; set; }
        public string? UserInput { get; set; }

        //TODO??? Bet start datetime, bet deadline.

    }
}
