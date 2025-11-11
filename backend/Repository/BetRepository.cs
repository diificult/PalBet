using Microsoft.EntityFrameworkCore;
using PalBet.Data;
using PalBet.Enums;
using PalBet.Interfaces;
using PalBet.Models;

namespace PalBet.Repository
{
    public class BetRepository : IBetRepository
    {

        private readonly ApplicationDbContext _context;

        public BetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Bet> AcceptBet(int betId, string userId)
        {
    
            var bet = await _context.bets.Include(x => x.Participants).FirstOrDefaultAsync(b => b.Id == betId);
            bet.Participants.FirstOrDefault(p => p.appUserId == userId).Accepted = true;
            await _context.SaveChangesAsync();
            return bet;
        }

        public async Task<Bet> CreateBet(Bet bet)
        {
            await _context.AddAsync(bet);
            await _context.SaveChangesAsync();
            return bet;
        }

        public Task<Bet?> DeleteAsync(int betId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Bet>?> GetBetRequests(string userId)
        {
            var bets = await _context.bets.Where(b => b.Participants.Any(p => p.appUserId == userId && !p.Accepted) && b.State == BetState.Requested).Include(b=> b.Participants).ThenInclude(p=>p.appUser).ToListAsync();
            return bets;
        }

        public async Task<Bet?> GetByIdAsync(int id)
        {
            return await _context.bets.Include(x => x.Participants).ThenInclude(p => p.appUser).FirstOrDefaultAsync(b => b.Id == id);
        }

        public Task<List<Bet>?> GetRequestedBets(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Bet>?> GetUsersBets(string userId)
        {
            return await _context.bets.Where(b => b.Participants.Any(p => p.appUserId == userId && p.Accepted)).Include(b => b.Participants).ThenInclude(p => p.appUser).Include(b => b.Group).ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
