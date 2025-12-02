using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PalBet.Dtos.Account;
using PalBet.Extensions;
using PalBet.Interfaces;
using PalBet.Models;
using StackExchange.Redis;
using System.Diagnostics;
using System.Text.Json;

namespace PalBet.Controllers
{
    public class AccountController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAppUserService _appUserService;
        private readonly IDatabase _redis;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, IAppUserService appUserService, IConnectionMultiplexer muxer)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _appUserService = appUserService;
            _redis = muxer.GetDatabase();


        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var AppUser = new AppUser
                {
                    UserName = dto.Username,
                    Email = dto.EmailAddress,
                };

                var createUser = await _userManager.CreateAsync(AppUser, dto.Password);

                if (createUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(AppUser, "User");
                    if (roleResult.Succeeded)
                        return Ok(
                        new UserDto
                        {
                            UserName = AppUser.UserName,
                            Email = AppUser.Email,
                            Token = _tokenService.CreateToken(AppUser)
                        });
                    else return StatusCode(500, roleResult.Errors);
                }
                else return StatusCode(500, createUser.Errors);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == dto.Username.ToLower());
            if (user == null) return Unauthorized("Invalid Username");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded) return Unauthorized("Invalid Username / Password Incorrect");

            return Ok(new UserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });
        }
        [HttpGet("GetCoins")]
        [Authorize]
        public async Task<IActionResult> GetCoins()
        {
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            
            var watch = Stopwatch.StartNew();

            var keyName = $"coins:{appUser.Id}";
            string json = await _redis.StringGetAsync($"coins:{appUser.Id}");
            if (string.IsNullOrEmpty(json))
            {
                var retrievedCoins =  await _appUserService.GetCoins(appUser.Id);
                var setTask = _redis.StringSetAsync(keyName, JsonSerializer.Serialize(retrievedCoins.ToString()), TimeSpan.FromMinutes(1));
                await Task.WhenAll(setTask);
                watch.Stop();
                Console.WriteLine($"Time taken to get coins: {watch.ElapsedMilliseconds} ms");
                return Ok(retrievedCoins);
                
            }

            Console.WriteLine(json);
            var coins = int.Parse(JsonSerializer.Deserialize<string>(json));
            watch.Stop();
            Console.WriteLine($"Time taken to get coins: {watch.ElapsedMilliseconds} ms");

            return Ok(coins);
        }
        [HttpGet("GetUsername")]
        [Authorize]
        public IActionResult GetUsername()
        {
            var Username = User.GetUsername();
            return Ok(Username);
        }
    }
}
