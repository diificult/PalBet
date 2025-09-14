using PalBet.Dtos.Friends;
using PalBet.Interfaces;
using PalBet.Mappers;
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

        public async Task<bool> AcceptRequest(string requesterId, string AccepteeId)
        {
            var requesterFriendships = await _friendRepository.GetFriendshipRequested(requesterId);
            var request = requesterFriendships.FirstOrDefault(f => f.RequesteeId == AccepteeId);
            if (request == null) return false;

            request.state = Enums.FriendshipState.Friends;
            request.FriendshipTime = DateTime.Now;

            await  _friendRepository.SaveAync();

            return true;


        }

        public  async Task<bool> CancelRequest(string requesterId, string requesteeId)
        {
            var friendship = await _friendRepository.DeleteAsync(requesterId, requesteeId);
            if (friendship == null) return false;
            else return true;

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

        public  async Task<List<FriendDto>?> GetFriendRequested(string userId)
        {
            var freiendRequests = await _friendRepository.GetFriendshipRequested(userId);
            return freiendRequests?.Select(f => new FriendDto { UserId = f.RequesteeId, Username = f.Requestee.UserName }).ToList();
        }

        public async Task<List<FriendDto>?> GetFriendRequests(string userId)
        {

            var freiendRequests = await _friendRepository.GetFriendshipRequests(userId);
            return freiendRequests?.Select(f => new FriendDto { UserId = f.RequesterId, Username = f.Requester.UserName }).ToList();
        }

        public async Task<List<FriendDto>?> GetFriendsList(string userId)
        {
            var friendships = await _friendRepository.GetFriendships(userId);
            List<FriendDto> friends = new List<FriendDto>();

           foreach (Friendship f in friendships)
            {
                if (f.RequesterId == userId)
                {
                    friends.Add(f.Requestee.fromFreindshipToFriendDto());
                } else
                {
                    friends.Add(f.Requester.fromFreindshipToFriendDto());
                }
            }
            return friends;

        }

        

        
    }
}
