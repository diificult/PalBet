using PalBet.Dtos.Account;

namespace PalBet.Interfaces
{
    public interface IAccountService
    {

        public Task<UserDto> CreateAccountAsync(RegisterDto dto);
        public Task<UserDto?> LoginAsync(LoginDto dto);
    }
}
