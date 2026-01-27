using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PalBet.Interfaces;
using PalBet.Mappers;
using PalBet.Services;
using PalBet.Extensions;
using Microsoft.AspNetCore.Identity;
using PalBet.Models;
using PalBet.Dtos.Bet;
using PalBet.Enums;

namespace PalBet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {

        public readonly UserManager<AppUser> _userManager;
        private readonly IBetService _betService;

        public BetController(IBetService betService, UserManager<AppUser> userManager)
        {
            _betService = betService;
            _userManager = userManager;
        }
        
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateNewBet([FromBody] CreateBetDto dto)
        {

            var username = User.GetUsername();
            dto.ParticipantUsernames.Add(username);
            var bet = await _betService.CreateBet(dto, username);
            if (bet != null)
                return Created();
            else return StatusCode(500, "Could not create, most likely not enough coins from a user");
        }

        [HttpGet("requests")]
        [Authorize]
        public async Task<IActionResult> GetBetRequests()
        {         
            var userId = await User.GetCurrentUserIdAsync(_userManager);
            var bets = await _betService.GetBetRequests(userId);
            if (bets == null) return NotFound();
            return Ok(bets);
        }

        [HttpPut("{BetId}/accept")]
        [Authorize]
        public async Task<IActionResult> AcceptBet([FromRoute] int betId, [FromBody] string? choice)
        {
            var userId = await User.GetCurrentUserIdAsync(_userManager);
            await _betService.AcceptBet(userId, betId, choice);

            return Ok();

        }
        [HttpPut("reject")]
        [Authorize]
        public async Task<IActionResult> RejectBet([FromBody] int betId)
        {

            var userId = await User.GetCurrentUserIdAsync(_userManager);
            await _betService.DeclineBet(userId, betId);

            return Ok();
        }



        [HttpPut("{BetId}/winner")]
        [Authorize]
        public async Task<IActionResult> DeclareWinner([FromRoute] int BetId, [FromBody] int ChoiceId)
        {
            var userId = await User.GetCurrentUserIdAsync(_userManager);
            await _betService.DeclareWinner(userId, BetId, ChoiceId);
            return Ok();
        }

        [HttpGet("state")]
        [Authorize]
        public async Task<IActionResult> GetBetFromState([FromQuery] BetState? state)
        {
            var userId = await User.GetCurrentUserIdAsync(_userManager);
            var bets = await _betService.GetBetsByState(userId, state);
            return Ok(bets);
        }

        [HttpGet("{BetId}")]
        [Authorize]
        public async Task<IActionResult> GetBetFromId([FromRoute] int betId)
        {
            var userId = await User.GetCurrentUserIdAsync(_userManager);
            var bet = await _betService.GetBetById(userId, betId);
            return Ok(bet);
        }

        [HttpPut("cancel")]
        [Authorize]
        public async Task<IActionResult> CancelBet([FromBody] int betId)
        {
            var userId = await User.GetCurrentUserIdAsync(_userManager);

            await _betService.CancelBet(userId, betId);
            return Ok();
        }
    
    }
}
