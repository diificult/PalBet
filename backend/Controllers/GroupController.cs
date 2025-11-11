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
    [Route("[controller]")]
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

        [HttpPost("CreateGroup")]
        [Authorize]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDto dto)
        {
            var Username = User.GetUsername();
            dto.GroupUsernames.Add(Username);
            await _groupService.CreateGroup(dto, Username);
            return Created();
        }

        [HttpGet("GetUserGroups")]
        [Authorize]
        public async Task<IActionResult> GetUserGroups()
        {
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            var groups = await _groupService.GetUserGroups(appUser.Id);
            return Ok(groups);


        }

        [HttpGet("{id}/GetMembers")]
        [Authorize]
        public async Task<IActionResult> GetGroupMembers([FromRoute] int id)
        {
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);

            var groupMembers = await _groupService.GetGroupMembers(id, appUser.Id);
            

            return Ok(groupMembers);
        }

        [HttpGet("{id}/GetDetails")]
        [Authorize]
        public async Task<IActionResult> GetGroupDetails([FromRoute] int id)
        {
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            var groupDetails = await _groupService.GetGroupDetails(id, appUser.Id);

            return Ok(groupDetails);
        }

        [HttpPost("AddUser")]
        [Authorize]
        public async Task<IActionResult> AddUser([FromBody] AddRemoveUserDto dto)
        {

            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            var requesteeAppUser = await _userManager.FindByNameAsync(dto.Username);

            await _groupService.AddUser(dto.GroupId, appUser.Id, requesteeAppUser.Id);

            return Ok();
        }
        [HttpDelete("RemoveUser")]
        [Authorize]
        public async Task<IActionResult> RemoveUser([FromBody] AddRemoveUserDto dto)
        {
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            var requesteeAppUser = await _userManager.FindByNameAsync(dto.Username);
            await _groupService.RemoveUser(dto.GroupId, appUser.Id, requesteeAppUser.Id);
            return Ok();
        }

            [HttpPut("EditGroup")]
        [Authorize]
        public async Task<IActionResult> EditGroup([FromBody] EditGroupDto dto)
        {
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            await _groupService.EditGroup(dto, appUser.Id);
            return Ok();
        }

    }
}
