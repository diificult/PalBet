using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PalBet.Extensions;
using PalBet.Interfaces;
using PalBet.Models;

namespace PalBet.Controllers
{
    public class FriendController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IFriendService _friendService;

        public FriendController(UserManager<AppUser> userManager, IFriendService friendService) {
            _userManager = userManager;
            _friendService = friendService;
        }

        //Send Request
        [HttpPost("SendRequest")]
        [Authorize]
        public async Task<IActionResult> SendRequest([FromBody]string Username)
        {

            var RequesterUsername = User.GetUsername();
            var RequesterId = await _userManager.FindByNameAsync(RequesterUsername);
            var RequesteeId = await _userManager.FindByNameAsync(Username);
            if (RequesteeId == null) return NotFound();

            var newFriendRequest = await _friendService.FriendshipRequest(RequesterId.Id, RequesteeId.Id);

            
            if (newFriendRequest == null) return NotFound();

            return Ok(newFriendRequest);

        }




        //Accept Request

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> AcceptRequest([FromBody]string Username)
        {
            var AccepteeUsername = User.GetUsername();
            var AccepteeId = await _userManager.FindByNameAsync(AccepteeUsername);
            var RequesterId = await _userManager.FindByNameAsync(Username);

            if (RequesterId == null) return NotFound();

            var acceptRequest = await _friendService.AcceptRequest(RequesterId.Id, AccepteeId.Id);
            if (acceptRequest == null) return NotFound();
            return Ok(acceptRequest);
        }


        //Cancel Request



        //Delete Friend

        //Block User

        //Get all friends
        [HttpGet("GetFriendsList")]
        [Authorize]
        public async Task<IActionResult> GetFriendsList()
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);
            var List = await _friendService.GetFriendsList(AppUser.Id);
            return Ok(List);
        }


        //Get all friend requests
        [HttpGet("GetFriendRequests")]
        [Authorize]
        public async Task<IActionResult> GetFriendRequests()
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);
            var List = await _friendService.GetFriendRequests(AppUser.Id);
            return Ok(List);
        }
        [HttpGet("GetFriendRequested")]
        [Authorize]
        public async Task<IActionResult> GetFriendRequested()
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);
            var List = await _friendService.GetFriendRequested(AppUser.Id);
            return Ok(List);
        }



    }
}
