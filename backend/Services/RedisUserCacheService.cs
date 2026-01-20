using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using PalBet.Interfaces;

using PalBet.Models;
using StackExchange.Redis;

namespace PalBet.Services
{
    public class RedisUserCacheService : IRedisUserCacheService
    {

        private readonly IDatabase _db;
        private const string CoinsKeyPrefix = "user:coins:";
        private static string Key(string userId) => $"{CoinsKeyPrefix}{userId}";


        public RedisUserCacheService(IConnectionMultiplexer mux)
        { 
            _db = mux.GetDatabase();
        }

        public async Task<int?> GetCoinsAsync(string userId)
        {

            var json = await _db.StringGetAsync(Key(userId)).ConfigureAwait(false);

            return json.IsNullOrEmpty ? null : JsonSerializer.Deserialize<int>(json);
        }
    

        public async Task InvalidateCoinsAsync(string userId)
        {
            await _db.KeyDeleteAsync(Key(userId)).ConfigureAwait(false);
        }

        public async Task SetCoinsAsync(string userId, int coins)
        {
            //Currently hard coded time span - might change if need it adjustable.
           await _db.StringSetAsync(Key(userId), JsonSerializer.Serialize(coins), TimeSpan.FromMinutes(1));
        }
    }
}
