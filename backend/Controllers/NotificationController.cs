using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using PalBet.Extensions;
using PalBet.Interfaces;
using PalBet.Models;

namespace PalBet.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        private readonly INotificationService _notificationService;
            private readonly UserManager<AppUser> _userManager;
        public NotificationController(INotificationService notificationService, UserManager<AppUser> userManager) {
        
            _notificationService = notificationService;
            _userManager = userManager; 
        }

        //Get Noitifications
        [HttpGet("GetNotifications")]
        [Authorize]
        public async Task<IActionResult> GetNotifications() {

            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);
            var Notifications = await _notificationService.GetNotifications(AppUser.Id);
            if (Notifications.IsNullOrEmpty()) { return NoContent(); }
            return Ok(Notifications);
        }

        //Read Notification (Should do it instead when got???)

        //Get Notification Count
        //Get Noitifications
        [HttpGet("GetCount")]
        [Authorize]
        public async Task<IActionResult> GetNotificationCount()
        {

            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);
            var Notifications = await _notificationService.GetNotificationCount(AppUser.Id);
            return Ok(Notifications);
        }

    }
}
