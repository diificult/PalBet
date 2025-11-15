using Microsoft.EntityFrameworkCore;
using PalBet.Data;
using PalBet.Interfaces;
using PalBet.Models;

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

        public async Task<int> UpdateCoins(string id, int value)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            user.PersonalCoins += value;
            await _context.SaveChangesAsync();
            return user.PersonalCoins;
        }

        public async Task<AppUser> GetUserAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
