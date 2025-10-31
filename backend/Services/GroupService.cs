using Microsoft.AspNetCore.Identity;
using PalBet.Dtos.Groups;
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

        public async Task<List<GroupDto>> GetUserGroups(string userId)
        {
           var groups = await _groupRepository.GetGroupsAsync(userId);
            var groupsDto = groups.Select(g => g.toGroupDtoFromGroup()).ToList();
            return groupsDto;
        }
    }
}
