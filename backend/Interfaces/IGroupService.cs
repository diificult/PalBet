using PalBet.Dtos.Groups;
using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface IGroupService
    {

        public Task<Group> CreateGroup(CreateGroupDto dto, string groupAdmin);

        public Task<List<GroupDto>> GetUserGroups(string userId);
    }
}
