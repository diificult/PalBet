using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface IGroupRepository
    {
        public Task<Group> CreateAsync(Group group);
        public Task<List<Group>> GetGroupsAsync(string userId);
        public Task<List<UserGroup>> GetGroupMembersAsync(int groupId);

        public Task<Group> GetGroupAsync(int groupId);
        public Task SaveAsync();
    }
}
