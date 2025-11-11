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
    [Route("[controller]")]
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
        
        [HttpPost("CreateBet")]
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

        [HttpGet("GetBetRequests")]
        [Authorize]
        public async Task<IActionResult> GetBetRequests()
        {            //Todo put these two lines into a seperate bit
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            var bets = await _betService.GetBetRequests(appUser.Id);
            if (bets == null) return NotFound();
            return Ok(bets);
        }

        [HttpPut("AcceptBet")]
        [Authorize]
        public async Task<IActionResult> AcceptBet([FromBody]int betId)
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);

            var Accepted = await _betService.AcceptBet(AppUser.Id, betId);

            if (Accepted) return Ok();
            return NotFound();

        }
        [HttpPut("RejectBet")]
        [Authorize]
        public async Task<IActionResult> RejectBet([FromBody] int betId)
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);

            var Rejected = await _betService.DeclineBet(AppUser.Id, betId);

            if (Rejected) return Ok();
            return NotFound();
        }

        [HttpPut("ChooseWinner")]
        [Authorize]

        public async Task<IActionResult> ChooseWinner([FromBody] ChooseWinnerDto dto)
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);
            var winnerUserId = await _userManager.FindByNameAsync(dto.winnerUsername);

            await _betService.SetWinner(winnerUserId.Id, AppUser.Id, dto.betId);

           return Ok();
           
        }

        [HttpGet("GetBetFromState")]
        [Authorize]
        public async Task<IActionResult> GetBetFromState([FromQuery] BetState? state)
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);

            var bets = await _betService.GetBetsByState(AppUser.Id, state);
            return Ok(bets);
        }

        [HttpGet("GetBetFromId/{betId}")]
        [Authorize]
        public async Task<IActionResult> GetBetFromId([FromRoute] int betId)
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);
            var bet = await _betService.GetBetById(AppUser.Id, betId);
            return Ok(bet);
        }

        [HttpPut("CancelBet")]
        [Authorize]
        public async Task<IActionResult> CancelBet([FromBody] int betId)
        {
            var Username = User.GetUsername();
            var AppUser = await _userManager.FindByNameAsync(Username);

            var bet = await _betService.CancelBet(AppUser.Id, betId);
            return Ok(bet);
        }
    
    }
}
