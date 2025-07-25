using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PalBet.Dtos;
using PalBet.Interfaces;
using PalBet.Mappers;
using PalBet.Services;
using PalBet.Extensions;
using Microsoft.AspNetCore.Identity;
using PalBet.Models;

namespace PalBet.Controllers
{
    [Route("api/Bet")]
    public class BetController : ControllerBase
    {

        public readonly UserManager<AppUser> _userManager;
        private readonly IBetService _betService;

        public BetController(IBetService bs)
        {
            _betService = bs;
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewBet([FromBody] CreateBetDto dto)
        {

            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            dto.ParticipantIds.Add(appUser.Id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var bet = await _betService.CreateBet(dto);
            if (bet != null)
                return Created();
            else return StatusCode(500, "Could not create");
        }


    }
}
