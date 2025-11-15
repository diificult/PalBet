using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface IAppUserRepository
    {
        public Task<int> GetCoins(string id);
        public Task<int> UpdateCoins(string id, int value);
        public Task<AppUser> GetUserAsync(string id);

        public Task SaveAsync();
    }
}
