using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PalBet.Dtos.Account;
using PalBet.Extensions;
using PalBet.Interfaces;
using PalBet.Models;


namespace PalBet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUserService _appUserService;
        private readonly IAccountService _accountService;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, IAppUserService appUserService, IAccountService accountService)
        {
            _userManager = userManager;
            _appUserService = appUserService;
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto dto)
        {
            var account = await _accountService.CreateAccountAsync(dto);
            return CreatedAtAction(nameof(account), new { }, account);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody]LoginDto dto)
        {
            var account = await _accountService.LoginAsync(dto);
            return Ok(account);
        }

        [HttpGet("coins")]
        [Authorize]
        public async Task<ActionResult<int>> GetCoins()
        {
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);

            var coins = await _appUserService.GetCoins(appUser.Id);

            return Ok(coins);
        }


        [HttpGet("username")]
        [Authorize]
        public ActionResult<string> GetUsername()
        {
            var Username = User.GetUsername();
            return Ok(Username);
        }
    }
}
