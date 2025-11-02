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
    }
}
