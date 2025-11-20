using PalBet.Enums;

namespace PalBet.Models
{
    public class Bet
    {
        public int Id { get; set; }
        public ICollection<BetParticipant> Participants { get; set; } = new List<BetParticipant>();
        public string BetDescription { get; set; }
        public BetState State { get; set; }
        public BetStakeType BetStakeType { get; set; }
        public int? Coins { get; set; }
        public string? UserInput { get; set; }
        public DateTime? Deadline { get; set; }

        public int? GroupId { get; set; }
        public Group? Group { get; set; }
        public bool IsGroup => GroupId != null;

        public ICollection<BetChoice> Choices { get; set; } = new List<BetChoice>();


        //Bet Settings
        public bool AllowMultipleWinners { get; set; } = false;
        public OutcomeChoice OutcomeChoice { get; set; }    
        public bool BurnStakeOnNoWnner { get; set; } = true;

    }
}
