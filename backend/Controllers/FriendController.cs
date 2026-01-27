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
        public async Task<IActionResult> SendRequest([FromBody]string Username)
        {

            var RequesterUsername = User.GetUsername();
            var RequesterId = await _userManager.FindByNameAsync(RequesterUsername);
            var RequesteeId = await _userManager.FindByNameAsync(Username);
            if (RequesteeId == null) throw new CustomException("This user does not exist", "USER_NOTFOUND", 400);

            var newFriendRequest = await _friendService.FriendshipRequest(RequesterId.Id, RequesteeId.Id);

            
            if (newFriendRequest == null) return NotFound();

            return Ok(newFriendRequest);

        }




        //Accept Request

        [HttpPut("accept")]
        [Authorize]
        public async Task<IActionResult> AcceptRequest([FromBody]string Username)
        {
            var AccepteeUsername = User.GetUsername();
            var AccepteeId = await _userManager.FindByNameAsync(AccepteeUsername);
            var RequesterId = await _userManager.FindByNameAsync(Username);

            if (RequesterId == null) return BadRequest("Could not find user");

            var acceptRequest = await _friendService.AcceptRequest(RequesterId.Id, AccepteeId.Id);
            if (acceptRequest == false) return BadRequest("Could not accept");
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
            if (Requestee == null) return BadRequest("Could not find user");

            var deletedRequest = await _friendService.CancelRequest(Requester.Id, Requestee.Id);
            if (deletedRequest == false) return BadRequest("Could not cancel request");
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
            if (Requester == null) return BadRequest("Could not find user");

            var deletedRequest = await _friendService.CancelRequest(Requester.Id, Requestee.Id);
            if (deletedRequest == false) return BadRequest("Could not decline request");
            return Ok();
        }

        //Delete Friend

        //Block User

        //Get all friends
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetFriendsList()
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);
            var List = await _friendService.GetFriendsList(AppUser.Id);
            return Ok(List);
        }


        //Get all friend requests
        [HttpGet("requests")]
        [Authorize]
        public async Task<IActionResult> GetFriendRequests()
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);
            var List = await _friendService.GetFriendRequests(AppUser.Id);
            return Ok(List);
        }
        [HttpGet("requested")]
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
