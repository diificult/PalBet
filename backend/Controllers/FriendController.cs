using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PalBet.Exceptions;
using PalBet.Extensions;
using PalBet.Interfaces;
using PalBet.Models;

namespace PalBet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<Friendship?>> SendRequest([FromBody]string Username)
        {
            var RequesteeId = await _userManager.FindByNameAsync(Username);
            if (RequesteeId == null) throw new CustomException("This user does not exist", "USER_NOTFOUND", 400);

            var RequesterUsername = User.GetUsername();
            var RequesterId = await _userManager.FindByNameAsync(RequesterUsername);

            var newFriendRequest = await _friendService.FriendshipRequest(RequesterId.Id, RequesteeId.Id);

            
            if (newFriendRequest == null) throw new CustomException("Could not send friend request", "FRIEND_REQUEST_FAILED", 400);

            return Ok(newFriendRequest);

        }




        //Accept Request

        [HttpPut("accept")]
        [Authorize]
        public async Task<IActionResult> AcceptRequest([FromBody]string Username)
        {
            var RequesterId = await _userManager.FindByNameAsync(Username);
            if (RequesterId == null) throw new CustomException("This user does not exist", "USER_NOTFOUND", 400);

            var AccepteeUsername = User.GetUsername();
            var AccepteeId = await _userManager.FindByNameAsync(AccepteeUsername);

            var acceptRequest = await _friendService.AcceptRequest(RequesterId.Id, AccepteeId.Id);
            if (acceptRequest == false) throw new CustomException("Could not accept friend request", "FRIEND_REQUEST_FAILED", 400);
            return Ok();
        }


        //Cancel Request

        [HttpDelete("cancel")]
        [Authorize]
        public async Task<IActionResult> CancelFriendRequest([FromBody] string Username)
        {
            var RequesterUsername = User.GetUsername();
            var Requester = await _userManager.FindByNameAsync(RequesterUsername);
            var Requestee = await _userManager.FindByNameAsync(Username);
            if (Requestee == null) throw new CustomException("This user does not exist", "USER_NOTFOUND", 400);

            var deletedRequest = await _friendService.CancelRequest(Requester.Id, Requestee.Id);
            if (deletedRequest == false) throw new CustomException("Could not cancel friend request", "FRIEND_REQUEST_FAILED", 400);
            return Ok();
        }

        //Decline Request

        [HttpDelete("decline")]
        [Authorize]
        public async Task<IActionResult> DeclineFriendRequest([FromBody] string Username)
        {
            var RequesterUsername = User.GetUsername();
            var Requestee = await _userManager.FindByNameAsync(RequesterUsername);
            var Requester = await _userManager.FindByNameAsync(Username);
            if (Requester == null) throw new CustomException("This user does not exist", "USER_NOTFOUND", 400);

            var deletedRequest = await _friendService.CancelRequest(Requester.Id, Requestee.Id);
            if (deletedRequest == false) throw new CustomException("Could not decline friend request", "FRIEND_REQUEST_FAILED", 400);
            return Ok();
        }

        //Delete Friend

        //Block User

        //Get all friends
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetFriendsList()
        {
            var userId = await User.GetCurrentUserIdAsync(_userManager);
            var List = await _friendService.GetFriendsList(userId);
            return Ok(List);
        }


        //Get all friend requests
        [HttpGet("requests")]
        [Authorize]
        public async Task<IActionResult> GetFriendRequests()
        {
            var userId = await User.GetCurrentUserIdAsync(_userManager);
            var List = await _friendService.GetFriendRequests(userId);
            return Ok(List);
        }
        [HttpGet("requested")]
        [Authorize]
        public async Task<IActionResult> GetFriendRequested()
        {
            var userId = await User.GetCurrentUserIdAsync(_userManager);
            var List = await _friendService.GetFriendRequested(userId);
            return Ok(List);
        }



    }
}
