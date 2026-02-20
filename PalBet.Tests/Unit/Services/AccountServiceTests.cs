using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PalBet.Dtos.Account;
using PalBet.Exceptions;
using PalBet.Interfaces;
using PalBet.Models;
using PalBet.Services;

namespace PalBet.Tests.Unit.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null
            );

            _tokenServiceMock = new Mock<ITokenService>();

            // Properly mock SignInManager with required dependencies
            var signInManagerMock = new Mock<SignInManager<AppUser>>(
                _userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<ILogger<SignInManager<AppUser>>>(),
                Mock.Of<IAuthenticationSchemeProvider>(),
                Mock.Of<IUserConfirmation<AppUser>>()
            );
            _signInManager = signInManagerMock.Object;

            _accountService = new AccountService(_userManagerMock.Object, _tokenServiceMock.Object, _signInManager);
        }

        [Fact]
        public async Task CreateAccountAsync_UsernameAlreadyExists_ThrowsCustomException()
        {
            var registerDto = new RegisterDto
            {
                Username = "testuser",
                EmailAddress = "test@example.com",
                Password = "test123123",
            };

            var failedResult = IdentityResult.Failed(new IdentityError { Code = "DuplicateUserName", Description = "Username is already taken" });
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(failedResult);

            var exception = await Assert.ThrowsAsync<CustomException>(() => _accountService.CreateAccountAsync(registerDto));


            Assert.Equal("CANNOT_CREATE_USERNAME_EXISTS", exception.Error);
            Assert.Equal(409, exception.ErrorCode);

        }
        [Fact]
        public async Task CreateAccountAsync_EmailAlreadyExists_ThrowsCustomException()
        {
            var registerDto = new RegisterDto
            {
                Username = "testuser",
                EmailAddress = "test@example.com",
                Password = "test123123",
            };

            var failedResult = IdentityResult.Failed(new IdentityError { Code = "DuplicateEmail", Description = "Email is already taken" });
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(failedResult);

            var exception = await Assert.ThrowsAsync<CustomException>(() => _accountService.CreateAccountAsync(registerDto));


            Assert.Equal("CANNOT_CREATE_EMAIL_EXISTS", exception.Error);
            Assert.Equal(409, exception.ErrorCode);

        }

        [Fact]
        public async Task CreateAccountAsync_ValidCredentials_ReturnsUserDto()
        {
            var registerDto = new RegisterDto
            {
                Username = "testuser",
                EmailAddress = "test@example.com",
                Password = "test123123",
            };

            _userManagerMock
               .Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
    .ReturnsAsync(IdentityResult.Success);

            // Mock successful role assignment
            _userManagerMock
                .Setup(um => um.AddToRoleAsync(It.IsAny<AppUser>(), "User"))
                .ReturnsAsync(IdentityResult.Success);

            // Mock token service
            _tokenServiceMock
                .Setup(ts => ts.CreateToken(It.IsAny<AppUser>()))
                .Returns("test-jwt-token");

            var result = await _accountService.CreateAccountAsync(registerDto);
            Assert.NotNull(result);
            Assert.Equal(result.UserName, registerDto.Username);
            Assert.Equal("test-jwt-token",result.Token);

        }



    }
}
