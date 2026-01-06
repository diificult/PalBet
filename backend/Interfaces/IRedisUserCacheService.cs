namespace PalBet.Interfaces
{
    public interface IRedisUserCacheService
    {
        Task<int?> GetCoinsAsync(string userId);
        Task SetCoinsAsync(string userId, int coins);
        Task InvalidateCoinsAsync(string userId);
    }
}
