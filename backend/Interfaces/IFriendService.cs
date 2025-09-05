using PalBet.Dtos.Friends;
using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface IFriendService
    {

        public Task<Friendship?> FriendshipRequest(string requesterId, string requesteeId);
        
        public Task<Friendship?> AcceptRequest(string requesterId, string AccepteeId);
        public Task<List<FriendDto>?> GetFriendsList(string userId);

    }
}
