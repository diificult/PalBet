using PalBet.Data;
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

        public async Task<Bet> CreateBet(Bet bet)
        {
            await _context.AddAsync(bet);
            await _context.SaveChangesAsync();
            return bet;
        }
    }
}
