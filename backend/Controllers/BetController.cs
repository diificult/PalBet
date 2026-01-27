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

            var Username = User.GetUsername();
            dto.ParticipantUsernames.Add(Username);
            var bet = await _betService.CreateBet(dto, Username);
            if (bet != null)
                return Created();
            else return StatusCode(500, "Could not create, most likely not enough coins from a user");
        }

        [HttpGet("requests")]
        [Authorize]
        public async Task<IActionResult> GetBetRequests()
        {            //Todo put these two lines into a seperate bit
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            var bets = await _betService.GetBetRequests(appUser.Id);
            if (bets == null) return NotFound();
            return Ok(bets);
        }

        [HttpPut("{BetId}/accept")]
        [Authorize]
        public async Task<IActionResult> AcceptBet([FromRoute] int betId, [FromBody] string? choice)
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);

            await _betService.AcceptBet(AppUser.Id, betId, choice);

            return Ok();

        }
        [HttpPut("reject")]
        [Authorize]
        public async Task<IActionResult> RejectBet([FromBody] int betId)
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);

           await _betService.DeclineBet(AppUser.Id, betId);

            return Ok();
        }



        [HttpPut("{BetId}/winner")]
        [Authorize]
        public async Task<IActionResult> DeclareWinner([FromRoute] int BetId, [FromBody] int ChoiceId)
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);
            await _betService.DeclareWinner(AppUser.Id, BetId, ChoiceId);
            return Ok();
        }

        [HttpGet("state")]
        [Authorize]
        public async Task<IActionResult> GetBetFromState([FromQuery] BetState? state)
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);

            var bets = await _betService.GetBetsByState(AppUser.Id, state);
            return Ok(bets);
        }

        [HttpGet("{BetId}")]
        [Authorize]
        public async Task<IActionResult> GetBetFromId([FromRoute] int betId)
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);
            var bet = await _betService.GetBetById(AppUser.Id, betId);
            return Ok(bet);
        }

        [HttpPut("cancel")]
        [Authorize]
        public async Task<IActionResult> CancelBet([FromBody] int betId)
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);

            await _betService.CancelBet(AppUser.Id, betId);
            return Ok();
        }
    
    }
}
