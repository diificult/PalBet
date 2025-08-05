using PalBet.Interfaces;
using PalBet.Models;

namespace PalBet.Services
{
    public class FriendService : IFriendService
    {

        private readonly IFriendRepository _friendRepository;
        public FriendService(IFriendRepository friendRepository)
        {
            _friendRepository = friendRepository;
        }

        public async Task<Friendship?> AcceptRequest(string requesterId, string AccepteeId)
        {
            var requesterFriendships = await _friendRepository.GetFriendshipRequests(requesterId);
            var request = requesterFriendships.FirstOrDefault(f => f.RequesteeId == AccepteeId);
            if (request == null) return null;

            request.state = Enums.FriendshipState.Friends;
            request.FriendshipTime = DateTime.Now;

            await  _friendRepository.SaveAync();

            return request;


        }

        public async Task<Friendship?> FriendshipRequest(string requesterId, string requesteeId)
        {
            //Check to see if friendship exists

            var requesterFriendships = await _friendRepository.GetFriendshipRequested(requesterId);
            var requesteeFriendship = await _friendRepository.GetFriendshipRequested(requesteeId); 
            if (requesterFriendships.Any(f => f.RequesteeId == requesteeId) || requesteeFriendship.Any(f => f.RequesteeId == requesterId)) return null;

            Console.WriteLine("Checked and cleared");
            //Creates new friendship
            Friendship newFriendship = new Friendship{
                RequesterId = requesterId,
                RequesteeId = requesteeId,
            };

            Console.WriteLine("Now adding");

            return await _friendRepository.CreateFriendship(newFriendship);
        }

        public async Task<List<Friendship>?> GetFriendsList(string userId)
        {
            return await _friendRepository.GetFriendships(userId);
        }

        
    }
}
