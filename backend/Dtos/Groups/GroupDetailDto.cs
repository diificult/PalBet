using PalBet.Dtos.Bet;
using PalBet.Dtos.Friends;

namespace PalBet.Dtos.Groups
{
    public class GroupDetailDto
    {


        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsRequesterAdmin { get; set; }
        public List<GroupMemberDto> Users { get; set; }

        public List<BetDto> Bets { get; set; }

        public List<(string user, int coins)>? Leaderboard { get; set; }

        //Group Settings 
        public int DefaultCoinAmount { get; set; }

        

     



    }
}
