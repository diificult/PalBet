using PalBet.Dtos.Groups;
using PalBet.Models;

namespace PalBet.Mappers
{
    public static class GroupMapper
    {

        public static GroupDto toGroupDtoFromGroup(this Group group)
        {
            return new GroupDto
            {
                Id = group.Id,
                Name = group.Name,
                Users = group.UserGroups.Select(ug => ug.User.fromFreindshipToFriendDto()).ToList(),
            };
        }

        public static GroupDetailDto toGroupDetailDtoFromGroup(this Group group, string userId)
        {
            return new GroupDetailDto
            {
                Id = group.Id,
                Name = group.Name,
                Users = group.UserGroups.Select(ug => ug.fromGroupUserToUserDto()).ToList(),
                Bets = group.GroupBets.Select(b => b.toBetDtoFromBets(userId)).ToList(),
                DefaultCoinAmount = group.DefaultCoinBalance,
            };
        }
    }
}
