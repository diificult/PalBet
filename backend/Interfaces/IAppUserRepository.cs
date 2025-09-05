namespace PalBet.Interfaces
{
    public interface IAppUserRepository
    {
        public Task<int> GetCoins(string id);
    }
}
