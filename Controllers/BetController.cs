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

        public BetController(IBetService bs, UserManager<AppUser> um)
        {
            _betService = bs;
            _userManager = um;
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewBet([FromBody] CreateBetDto dto)
        {
            //Todo put these two lines into a seperate bit
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            dto.ParticipantIds.Add(appUser.Id);
            var bet = await _betService.CreateBet(dto, appUser.Id);
            if (bet != null)
                return Created();
            else return StatusCode(500, "Could not create");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBetRequests()
        {
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            var bets = await _betService.GetBetRequests(appUser.Id);
            if (bets == null) return NotFound();
            return Ok(bets);
        }

        [HttpPut("AcceptBet")]
        [Authorize]
        public async Task<IActionResult> AcceptBet(int betId)
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);

            var Accepted = await _betService.AcceptBet(AppUser.Id, betId);

            if (Accepted) return Ok();
            return NotFound();

        }

        [HttpPut("ChooseWinner")]
        [Authorize]

        public async Task<IActionResult> ChooseWinner(int betId, string userId)
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);

            var winner = await _betService.SetWinner(userId, AppUser.Id, betId);

            if (winner) return Ok();
            return BadRequest();
        }

        [HttpGet("GetBetFromState")]
        [Authorize]
        public async Task<IActionResult> GetBetFromState([FromQuery] BetState? state)
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);

            var bets = await _betService.GetBetsByState(AppUser.Id, state);
        }

    }
}
