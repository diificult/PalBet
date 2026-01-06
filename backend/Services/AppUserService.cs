using Hangfire;
using PalBet.Interfaces;

namespace PalBet.Services
{
    public class AppUserService : IAppUserService
    {

        public readonly IAppUserRepository _AppUserRepository;
        private readonly IRedisUserCacheService _cache;
        public AppUserService(IAppUserRepository appUserRepository, IRedisUserCacheService cache)
        {
            _AppUserRepository = appUserRepository;
            _cache = cache;
        }
        public async Task<int> GetCoins(string id)
        {

            var cached = await _cache.GetCoinsAsync(id);
            if (cached != null) return cached.Value;

            var coins = await _AppUserRepository.GetCoins(id);
            await _cache.SetCoinsAsync(id, coins);

            return coins;

        }

    }
}
