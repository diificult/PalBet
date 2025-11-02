using PalBet.Dtos.Bet;
using PalBet.Dtos.Friends;

namespace PalBet.Dtos.Groups
{
    public class GroupDetailDto
    {


        public int Id { get; set; }
        public string Name { get; set; }
        public List<GroupMemberDto> Users { get; set; }

        public List<BetDto> Bets { get; set; }

        //Group Settings 
        public int DefaultCoinAmount { get; set; }

     



    }
}
