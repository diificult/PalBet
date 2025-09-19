using PalBet.Enums;

namespace PalBet.Dtos.Bet
{
    public class BetDto
    {

        public int BetId { set; get; }

        public ICollection<string> ParticipantNames { set; get; }
        
        public string BetDescription { set; get; }
        public string? UserWinner { set; get; }

        public BetState BetState { set; get; }
        public int BetStake { set; get; }

        public bool isHost { set; get; } = false;
    }
}
