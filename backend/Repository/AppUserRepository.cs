using Microsoft.EntityFrameworkCore;
using PalBet.Data;
using PalBet.Interfaces;

namespace PalBet.Repository
{
    public class AppUserRepository : IAppUserRepository
    {

        private readonly ApplicationDbContext _context;

        public AppUserRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<int> GetCoins(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user.PersonalCoins;
        }
    }
}
