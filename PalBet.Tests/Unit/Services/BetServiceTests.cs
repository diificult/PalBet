using Microsoft.AspNetCore.Identity;
using Moq;
using PalBet.Dtos.Account;
using PalBet.Exceptions;
using PalBet.Interfaces;
using PalBet.Models;
using PalBet.Services;

namespace PalBet.Tests.Unit.Services
{
    public class BetServiceTests
    {
        private readonly Mock<IBetRepository> _betRepositoryMock;
        private readonly Mock<IAppUserRepository> _userRepositoryMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<IGroupRepository> _groupRepositoryMock;
        private readonly Mock<IRedisBetsCacheService> _redisBetsCacheServiceMock;



        private readonly BetService _betService;
        public BetServiceTests()
        {

            _userManagerMock = new Mock<UserManager<AppUser>>(
    Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null
);
            _betRepositoryMock = new Mock<IBetRepository>();
            _userRepositoryMock = new Mock<IAppUserRepository>();
            _notificationServiceMock = new Mock<INotificationService>();
            _groupRepositoryMock = new Mock<IGroupRepository>();
            _redisBetsCacheServiceMock = new Mock<IRedisBetsCacheService>();

            


            _betService = new BetService(_betRepositoryMock.Object, _userRepositoryMock.Object, _notificationServiceMock.Object, _userManagerMock.Object, _groupRepositoryMock.Object, _redisBetsCacheServiceMock.Object);

        }

        [Fact]
        public async Task AcceptBet_ValidBet_ReturnsNull()
        {
            var participant1 = new BetParticipant
            {
                AppUser = new AppUser
                {
                    Id = "abc",
                    UserName = "test user",
                    DailyRewardLastClaim = DateTime.Now,
                    Email = "example@email.com",
                    PersonalCoins = 200,

                },
                BetId = 1,
                Accepted = false,
                IsBetHost = false,
            };
            var bet = new Bet
            {
                Id = 1,
                AllowMultipleWinners = true,
                BetDescription = "Test",
                BetStakeType = Enums.BetStakeType.Coins,
                BurnStakeOnNoWnner = true,
                Choices = new List<BetChoice>(),
                Coins = 50,
                Deadline = null,
                Group = null,
                GroupId = null,
                Participants = new List<BetParticipant>
                {
                    participant1

                },
                State = Enums.BetState.Requested,
                OutcomeChoice = Enums.OutcomeChoice.UserSubmitted,





            };

            _betRepositoryMock.Setup(br => br.GetByIdAsync(1)).ReturnsAsync(bet);

             var task = _betService.AcceptBet(participant1.AppUserId, 1, "test choice");

            await task;
        }

        [Fact]
        public async Task AcceptBet_NotEnoughCoins_ReturnsError()
        {
            var participant1 = new BetParticipant
            {
                AppUser = new AppUser
                {
                    Id = "abc",
                    UserName = "test user",
                    DailyRewardLastClaim = DateTime.Now,
                    Email = "example@email.com",
                    PersonalCoins = 20,

                },
                BetId = 1,
                Accepted = false,
                IsBetHost = false,
            };
            var bet = new Bet
            {
                Id = 1,
                AllowMultipleWinners = true,
                BetDescription = "Test",
                BetStakeType = Enums.BetStakeType.Coins,
                BurnStakeOnNoWnner = true,
                Choices = new List<BetChoice>(),
                Coins = 50,
                Deadline = null,
                Group = null,
                GroupId = null,
                Participants = new List<BetParticipant>
                {
                    participant1

                },
                State = Enums.BetState.Requested,
                OutcomeChoice = Enums.OutcomeChoice.UserSubmitted,





            };

            _betRepositoryMock.Setup(br => br.GetByIdAsync(1)).ReturnsAsync(bet);

            var exception = await Assert.ThrowsAsync<CustomException>(() => _betService.AcceptBet(participant1.AppUserId, 1, "test choice"));

            Assert.Equal("BET_INSUFFICIENT_COINS", exception.Error);
            Assert.Equal(400, exception.ErrorCode);

        }

    }
}
