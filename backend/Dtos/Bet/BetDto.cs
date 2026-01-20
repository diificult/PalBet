using PalBet.Enums;

namespace PalBet.Dtos.Bet
{
    public class BetDto
    {

        public int BetId { set; get; }

        public ICollection<(string UserName, string? choice)> ParticipantNames { set; get; }
        
        public string BetDescription { set; get; }
        public List<string>? UserWinner { set; get; }

        public string BetState { set; get; }
        public string BetStake { set; get; }

        public bool IsHost { set; get; } = false;

        public string? Deadline { set; get; }

        public string? GroupName { set; get; }

        public bool AllowMultipleWinners { get; set; } 
        public string OutcomeChoice { get; set; }
        public bool BurnStakeOnNoWnner { get; set; }
        public List<string>? Choices { get; set; }
    }
}
