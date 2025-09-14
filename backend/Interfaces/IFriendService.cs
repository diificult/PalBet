using PalBet.Dtos.Friends;
using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface IFriendService
    {

        public Task<Friendship?> FriendshipRequest(string requesterId, string requesteeId);
        
        public Task<bool> AcceptRequest(string requesterId, string AccepteeId);
        public Task<List<FriendDto>?> GetFriendsList(string userId);
        public Task<List<FriendDto>?> GetFriendRequests(string userId);
        public Task<List<FriendDto>?> GetFriendRequested(string userId);
        public Task<bool> CancelRequest(string requesterId, string requesteeId); 

    }
}
