﻿using Microsoft.AspNetCore.Authorization;
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
    }
}
