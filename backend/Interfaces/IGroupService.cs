using PalBet.Dtos.Friends;
using PalBet.Dtos.Groups;
using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface IGroupService
    {

        public Task<Group> CreateGroup(CreateGroupDto dto, string groupAdmin);

        public Task<List<GroupDto>> GetUserGroups(string userId);
        public Task<List<GroupMemberDto>> GetGroupMembers(int groupId, string requesterId);
        public Task<GroupDetailDto> GetGroupDetails(int groupId, string requesterId);

        public Task AddUser(int groupId, string requesterId, string requesteeId);
        public Task RemoveUser(int groupId, string requesterId, string requesteeId);
        public Task EditGroup(int groupId, EditGroupDto dto, string requesterId);
        public Task EditMemberPermissons(int groupId, EditGroupMemberPermissionsDto dto, string requesterId);

    }
}
