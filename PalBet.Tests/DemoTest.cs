using Moq;
using PalBet.Interfaces;
using PalBet.Models;

namespace PalBet.Tests
{
    public class DemoTest
    {
        [Fact]
        public void GetUserById_ShouldReturnUser_WhenUserExists()
        {
            var mockRep = new Mock<IAppUserRepository>();
            var testUser = new AppUser
            {
                Id = "test123",
                UserName = "testuser",
                Email = "test@example.com",
            };

            mockRep.Setup(repo => repo.GetUserAsync("test123"))
                   .ReturnsAsync(testUser);

            
            var userRepository = mockRep.Object;
            var result = userRepository.GetUserAsync("test123").Result;
            Assert.NotNull(result);
            Assert.Equal("testuser", result.UserName);
            


        }
    }
}