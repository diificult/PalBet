using PalBet.Interfaces;
using PalBet.Models;
using StackExchange.Redis;

namespace PalBet.Services
{

    public class RedisGroupCacheService : IRedisGroupCacheService
    {

        private const string KeyPrefix = "group:";
        private static string LeaderboardKey(int groupId) => $"{KeyPrefix}{groupId}:leaderboard";

        private readonly IDatabase _db;

        public RedisGroupCacheService(IConnectionMultiplexer mux){
            _db = mux.GetDatabase();
        
        }

        public async Task SetGroupLeaderboardAsync(int groupId, List<UserGroup> groupUsers)
        {

            var entries = groupUsers.Select(u =>
                new SortedSetEntry(u.UserId, u.CoinBalance)).ToArray();

            await _db.SortedSetAddAsync(LeaderboardKey(groupId), entries);
        }

        public async Task<List<(string username, double coins)>> GetGroupLoaderboardAsync(int groupId)
        {
            return (await _db.SortedSetRangeByRankWithScoresAsync(LeaderboardKey(groupId), 0, -1, Order.Descending)).Select(s => (s.Element.ToString(), s.Score)).ToList();
        }

        public async Task UpdateUserCoinsAsync(int groupId, string userId, double newCoins)
        {
            await _db.SortedSetAddAsync(LeaderboardKey(groupId), userId, newCoins);
        }
    }
}
