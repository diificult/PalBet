using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface IAppUserService
    {
        public Task<int> GetCoins(string id);
    }
}
