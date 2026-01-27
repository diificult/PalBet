using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PalBet.Extensions;
using PalBet.Interfaces;
using PalBet.Models;

namespace PalBet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        private readonly INotificationService _notificationService;
        private readonly UserManager<AppUser> _userManager;
        public NotificationController(INotificationService notificationService, UserManager<AppUser> userManager)
        {

            _notificationService = notificationService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetNotifications()
        {

            var userId = await User.GetCurrentUserIdAsync(_userManager);
            var Notifications = await _notificationService.GetNotifications(userId);
            if (Notifications.IsNullOrEmpty()) { return NoContent(); }
            return Ok(Notifications);
        }

        [HttpGet("count")]
        [Authorize]
        public async Task<IActionResult> GetNotificationCount()
        {

            var userId = await User.GetCurrentUserIdAsync(_userManager);
            var Notifications = await _notificationService.GetNotificationCount(userId);
            return Ok(Notifications);
        }

    }
}
