using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PalBet.Dtos.Groups;
using PalBet.Extensions;
using PalBet.Interfaces;
using PalBet.Models;
using System.Threading.Tasks;

namespace PalBet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        public readonly IGroupService _groupService;
        public readonly UserManager<AppUser> _userManager;

        public GroupController(IGroupService groupService, UserManager<AppUser> userManager)
        {
            _groupService = groupService;
            _userManager = userManager;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDto dto)
        {
            var Username = User.GetUsername();
            dto.GroupUsernames.Add(Username);
            await _groupService.CreateGroup(dto, Username);
            return Created();
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetUserGroups()
        {
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            var groups = await _groupService.GetUserGroups(appUser.Id);
            return Ok(groups);


        }

        [HttpGet("{id}/members")]
        [Authorize]
        public async Task<IActionResult> GetGroupMembers([FromRoute] int id)
        {
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);

            var groupMembers = await _groupService.GetGroupMembers(id, appUser.Id);
            

            return Ok(groupMembers);
        }

        [HttpPost("{id}/members/{username}")]
        [Authorize]
        public async Task<IActionResult> AddUser([FromRoute] int id, [FromRoute] string username)
        {

            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            var requesteeAppUser = await _userManager.FindByNameAsync(username);

            await _groupService.AddUser(id, appUser.Id, requesteeAppUser.Id);

            return Ok();
        }
        [HttpDelete("{id}/members/{username}")]
        [Authorize]
        public async Task<IActionResult> RemoveUser([FromRoute] int id, [FromRoute] string username)
        {
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            var requesteeAppUser = await _userManager.FindByNameAsync(username);
            await _groupService.RemoveUser(id, appUser.Id, requesteeAppUser.Id);
            return Ok();
        }

        [HttpGet("{id}/details")]
        [Authorize]
        public async Task<IActionResult> GetGroupDetails([FromRoute] int id)
        {
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            var groupDetails = await _groupService.GetGroupDetails(id, appUser.Id);

            return Ok(groupDetails);
        }


        [HttpPut("{id}/edit")]
        [Authorize]
        public async Task<IActionResult> EditGroup([FromRoute]int id, [FromBody] EditGroupDto dto)
        {
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            await _groupService.EditGroup(id, dto, appUser.Id);
            return Ok();
        }

        [HttpPut("{id}/edit-permissions")]
        [Authorize]
        public async Task<IActionResult> EditUserPermissions([FromRoute] int id, [FromBody] EditGroupMemberPermissionsDto dto)
        {
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            await _groupService.EditMemberPermissons(id, dto, appUser.Id);
            return Ok();
        }

    }
}
