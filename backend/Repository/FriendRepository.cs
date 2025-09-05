using Microsoft.EntityFrameworkCore;
using PalBet.Data;
using PalBet.Interfaces;
using PalBet.Models;

namespace PalBet.Repository
{
    public class FriendRepository : IFriendRepository
    {

        
        private readonly ApplicationDbContext _context;

        public FriendRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<Friendship> CreateFriendship(Friendship friendship)
        {
            await _context.friendships.AddAsync(friendship);
            await _context.SaveChangesAsync();
            return friendship;
        }

        public async Task<List<Friendship>?> GetFriendshipRequested(string requesterId)
        {
            var requests = await _context.friendships.Where(f => f.RequesterId == requesterId && f.state == Enums.FriendshipState.Requested).Include(ee => ee.Requestee).ToListAsync();
            return requests;
        }

        public async Task<List<Friendship>?> GetFriendshipRequests(string UserId)
        {
            var requests = await _context.friendships.Where(f => f.RequesteeId == UserId && f.state == Enums.FriendshipState.Requested).Include(er => er.Requester).ToListAsync();
            return requests;
        }

        //Get accepted friendships
        public async Task<List<Friendship>?> GetFriendships(string UserId)
        {
            var friends = await _context.friendships.Where(f => (f.RequesterId == UserId || f.RequesteeId == UserId) && f.state == Enums.FriendshipState.Friends).Include(ee => ee.Requestee).Include(er => er.Requester).ToListAsync();
            return friends;
        }

        public async Task SaveAync()
        {
            throw new NotImplementedException();
        }
    }
}
