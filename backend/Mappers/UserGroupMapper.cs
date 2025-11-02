using PalBet.Dtos.Friends;
using PalBet.Dtos.Groups;
using PalBet.Models;

namespace PalBet.Mappers
{
    public static class UserGroupMapper
    {

        public static GroupMemberDto fromGroupUserToUserDto(this UserGroup userGroup)
        {
            var groupMember = new GroupMemberDto
            {
                Username = userGroup.User.UserName,
                Role = userGroup.IsAdmin ? "ADMIN" : null,
                Balance = userGroup.CoinBalance,
                UserId = userGroup.UserId,
                
            };
            if (groupMember.Role == "ADMIN")
            {
                groupMember.CanCreateBets = userGroup.CanCreateBet;
            }
            return groupMember;
        }
    }
}
