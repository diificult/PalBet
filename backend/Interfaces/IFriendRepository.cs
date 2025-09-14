using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface IFriendRepository
    {
        public Task<Friendship> CreateFriendship(Friendship friendship);
        public Task<List<Friendship>?> GetFriendshipRequested(string requesterId);
        public Task<List<Friendship>?> GetFriendshipRequests(string UserId);
        public Task<List<Friendship>?> GetFriendships(string UserId);
        public Task<Friendship?> DeleteAsync(string requesterId, string requesteeId);
        public Task SaveAync();
    }
}
