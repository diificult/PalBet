using PalBet.Models;
using System.Runtime.CompilerServices;

namespace PalBet.Interfaces
{
    public interface IRedisGroupCacheService
    {
        public Task SetGroupLeaderboardAsync(int groupId, List<UserGroup> groupUsers);

        public Task<List<(string username, double coins)>> GetGroupLoaderboardAsync(int groupId);

    }
}
