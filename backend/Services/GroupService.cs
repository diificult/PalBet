using Microsoft.AspNetCore.Identity;
using PalBet.Dtos.Friends;
using PalBet.Dtos.Groups;
using PalBet.Exceptions;
using PalBet.Interfaces;
using PalBet.Mappers;
using PalBet.Models;

namespace PalBet.Services
{
    public class GroupService : IGroupService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IGroupRepository _groupRepository;

        public GroupService(UserManager<AppUser> userManager, IGroupRepository groupRepository)
        {
            _userManager = userManager;
            _groupRepository = groupRepository;
        }
        public async Task<Group> CreateGroup(CreateGroupDto dto, string groupAdmin)
        {

            var groupUsers = new List<UserGroup>();

            foreach (var username in dto.GroupUsernames)
            {
                var user = await _userManager.FindByNameAsync(username) ?? throw new Exception($"User {username} not found");
                var userGroup = new UserGroup
                {
                    User = user,
                    IsAdmin = username == groupAdmin,
                    CoinBalance = dto.DefaultCoinBalance ?? 100
                };

                groupUsers.Add(userGroup);

            }

            var group = new Group
            {
                Name = dto.GroupName,
                DefaultCoinBalance = dto.DefaultCoinBalance ?? 100,
                UserGroups = groupUsers
            };

            var createdGroup = await _groupRepository.CreateAsync(group);

            return createdGroup;
        }

        public async Task<GroupDetailDto> GetGroupDetails(int groupId, string requesterId)
        {
            var group = await _groupRepository.GetGroupAsync(groupId);
            if (!group.UserGroups.Select(u => u.UserId).Contains(requesterId)) throw new CustomException("Requester is not a member of the group", "GROUP_ACCESS_DENIED", 403);
            var groupDto = group.toGroupDetailDtoFromGroup(requesterId);

            return groupDto;
        }

        public async Task<List<GroupMemberDto>> GetGroupMembers(int groupId, string requesterId)
        {
            
            var users = await _groupRepository.GetGroupMembersAsync(groupId);
            if (!users.Select(u => u.User.Id).Contains(requesterId)) throw new CustomException("Requester is not a member of the group", "GROUP_ACCESS_DENIED", 403);

            
            var usersDto = users.Select(u => u.fromGroupUserToUserDto()).ToList();

            return usersDto;
        }

        public async Task<List<GroupDto>> GetUserGroups(string userId)
        {
           var groups = await _groupRepository.GetGroupsAsync(userId);
            var groupsDto = groups.Select(g => g.toGroupDtoFromGroup()).ToList();
            return groupsDto;
        }
    }
}
