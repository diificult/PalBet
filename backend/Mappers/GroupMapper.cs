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
                Users = group.UserGroups.Select(ug => ug.fromGroupUserToUserDto(false)).ToList(),
            };
        }

        public static GroupDetailDto toGroupDetailDtoFromGroup(this Group group, string userId, List<(string userId, double coins)> values)
        {
            var isAdmin = group.UserGroups.FirstOrDefault(u => u.UserId == userId).IsAdmin;
            return new GroupDetailDto
            {
                Id = group.Id,
                Name = group.Name,
                IsRequesterAdmin = isAdmin,
                Users = group.UserGroups.Select(ug => ug.fromGroupUserToUserDto(isAdmin)).ToList(),
                Bets = group.GroupBets.Select(b => b.ToBetDtoFromBets(userId)).ToList(),
                DefaultCoinAmount = group.DefaultCoinBalance,
                Leaderboard = values.Select(s => (group.UserGroups.FirstOrDefault(ug => ug.UserId == s.userId).User.UserName, (int)s.coins)).ToList(),
            };
        }
    }
}
