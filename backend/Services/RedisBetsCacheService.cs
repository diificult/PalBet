using PalBet.Dtos.Bet;
using PalBet.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace PalBet.Services
{

    public class RedisBetsCacheService : IRedisBetsCacheService
    {

        private readonly IDatabase _db;
        private const string KeyPrefix = "user:bets:";
        private static string Key(string userId, string betState) => $"{KeyPrefix}{userId}:{betState}";
        public RedisBetsCacheService(IConnectionMultiplexer mux)
        {
            _db = mux.GetDatabase();
        }


        public async Task<List<BetDto>?> GetBetsByStateAsync(string userId, string state)
        {
            var json = await _db.StringGetAsync(Key(userId, state)).ConfigureAwait(false);

            return json.IsNullOrEmpty ? null : JsonSerializer.Deserialize<List<BetDto>>(json); 
        }

        public async Task SetBetsByStateAsync(string userId, string state, List<BetDto> bets)
        {
            await _db.StringSetAsync(Key(userId, state), JsonSerializer.Serialize(bets), TimeSpan.FromMinutes(1));
        }

        public async Task InvalidateBetsAsync(string userId, string state)
        {
            await _db.KeyDeleteAsync(Key(userId, state)).ConfigureAwait(false);
        }
    }
}
