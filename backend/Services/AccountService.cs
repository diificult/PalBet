using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PalBet.Dtos.Account;
using PalBet.Exceptions;
using PalBet.Interfaces;
using PalBet.Models;
using Xunit.Sdk;

namespace PalBet.Services
{
    public class AccountService : IAccountService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;


        public AccountService(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager) {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }


        public async Task<UserDto> CreateAccountAsync(RegisterDto dto)
        {
            try
            {
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
                        return
                        new UserDto
                        {
                            UserName = AppUser.UserName,
                            Email = AppUser.Email,
                            Token = _tokenService.CreateToken(AppUser)
                        };
                    else throw new CustomException("CANNOT_CREATE_ACCOUNT", "Unable to create account, may be an error on server side", 500);

                }
                else throw new CustomException("CANNOT_CREATE_ACCOUNT", "Unable to create account, may be an error on server side", 500);
            }
            catch (Exception e)
            {
                throw new CustomException("CANNOT_CREATE_ACCOUNT", "Unable to create an account, caught error on server side", 500);
            }
        }

        public async  Task<UserDto?> LoginAsync(LoginDto dto)
        {

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == dto.Username.ToLower());
            if (user == null) throw new CustomException("USER_NOT_EXIST", "The user does not exist", 401);

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded) throw new CustomException("LOGIN_FAILED", "Username / Password incorrect", 401);

            return new UserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
