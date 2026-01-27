using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PalBet.Extensions;
using PalBet.Interfaces;
using PalBet.Models;

namespace PalBet.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPut("collect")]
        [Authorize]
        public async Task<ActionResult> CollectRewards()
        {

            var userId = await User.GetCurrentUserIdAsync(_userManager);
            await _rewardService.GetRewards(userId);

            return Ok();
        }

        [HttpGet("check")]
        [Authorize]
        public async Task<ActionResult<string>> CheckLast()
        {

            var userId = await User.GetCurrentUserIdAsync(_userManager);
            var timeRemaining = await _rewardService.GetTimeRemaining(userId);
            return Ok(timeRemaining);
        }

    }
}
