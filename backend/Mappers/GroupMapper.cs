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
    }
}
