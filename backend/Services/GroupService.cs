using Microsoft.AspNetCore.Identity;
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

        public async Task AddUser(int groupId, string requesterId, string requesteeId)
        {

            //TODO:
            //Check if admin
            //Check if user is already in group

            var group = await _groupRepository.GetGroupAsync(groupId);

            if (!group.UserGroups.FirstOrDefault(u => u.UserId == requesterId).IsAdmin) throw new CustomException("Only admins can add users to the group", "GROUP_ADDUSER_DENIED", 403);
            if (group.UserGroups.Select(u => u.UserId).Contains(requesteeId)) throw new CustomException("User is already a member of the group", "GROUP_ADDUSER_ALREADYMEMBER", 400);

            var newUser = new UserGroup
            {
                UserId = requesteeId,
                GroupId = groupId,
                IsAdmin = false,
                CoinBalance = group.DefaultCoinBalance
            };
            group.UserGroups.Add(newUser);
            await _groupRepository.SaveAsync();
        }
        public async Task RemoveUser(int groupId, string requesterId, string requesteeId)
        {

            var group = await _groupRepository.GetGroupAsync(groupId);

            if (!group.UserGroups.FirstOrDefault(u => u.UserId == requesterId).IsAdmin) throw new CustomException("Only admins can remove users from the group", "GROUP_REMOVEUSER_DENIED", 403);
            var userGroup = group.UserGroups.FirstOrDefault(u => u.UserId == requesteeId) ?? throw new CustomException("User is not a member of the group", "GROUP_REMOVEUSER_NOTMEMBER", 400);

            group.UserGroups.Remove(userGroup);

            await _groupRepository.SaveAsync();

        }

        public async Task<Group> CreateGroup(CreateGroupDto dto, string groupAdmin)
        {

            var groupUsers = new List<UserGroup>();

            foreach (var username in dto.GroupUsernames)
            {
                var user = await _userManager.FindByNameAsync(username) ?? throw new CustomException($"User {username} not found", "GROUP_USERNAME_NOTFOUND", 404);
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

        public async Task EditGroup(EditGroupDto dto, string requesterId)
        {
            var group = await _groupRepository.GetGroupAsync(dto.GroupId);
            if (!group.UserGroups.FirstOrDefault(u => u.UserId == requesterId).IsAdmin) throw new CustomException("Only admins can edit the group", "GROUP_EDIT_DENIED", 403);
            group.DefaultCoinBalance = dto.DefaultCoinBalance;
            await _groupRepository.SaveAsync();
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


            var usersDto = users.Select(u => u.fromGroupUserToUserDto(false)).ToList();

            return usersDto;
        }

        public async Task<List<GroupDto>> GetUserGroups(string userId)
        {
            var groups = await _groupRepository.GetGroupsAsync(userId);
            var groupsDto = groups.Select(g => g.toGroupDtoFromGroup()).ToList();
            return groupsDto;
        }

        public async Task EditMemberPermissons(EditGroupMemberPermissionsDto dto, string requesterId)
        {
            var group = await _groupRepository.GetGroupAsync(dto.GroupId);
            if (!group.UserGroups.FirstOrDefault(u => u.UserId == requesterId).IsAdmin) throw new CustomException("Only admins can edit member permissions", "GROUP_EDITMEMBERPERMISSIONS_DENIED", 403);
            var userGroup = group.UserGroups.FirstOrDefault(u => u.User.UserName == dto.Username) ?? throw new CustomException("User is not a member of the group", "GROUP_EDITMEMBERPERMISSIONS_NOTMEMBER", 400);

            userGroup.CanCreateBet = dto.CanCreateBets;

            await _groupRepository.SaveAsync();

        }
    }
}
