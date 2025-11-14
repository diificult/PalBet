using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PalBet.Extensions;
using PalBet.Interfaces;
using PalBet.Models;

namespace PalBet.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RewardsController : ControllerBase
    {

        public readonly IRewardService _rewardService;
        public readonly UserManager<AppUser> _userManager;    
        public RewardsController(IRewardService rewardService, UserManager<AppUser> userManager)
        {
            _rewardService = rewardService;
            _userManager = userManager;
        }

        [HttpPut("GetRewards")]
        [Authorize]
        public async Task<IActionResult> GetRewards()
        {

            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            await _rewardService.GetRewards(appUser.Id);

            return Ok();
        }

        [HttpGet("CheckLast")]
        [Authorize]
        public async Task<IActionResult> CheckLast()
        {

            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);

            var timeRemaining = await _rewardService.GetTimeRemaining(appUser.Id);
            return Ok(timeRemaining);
        }

    }
}
