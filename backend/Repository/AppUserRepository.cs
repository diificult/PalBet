using Microsoft.EntityFrameworkCore;
using PalBet.Data;
using PalBet.Exceptions;
using PalBet.Interfaces;
using PalBet.Models;

namespace PalBet.Repository
{
    public class AppUserRepository : IAppUserRepository
    {

        private readonly ApplicationDbContext _context;

        public AppUserRepository(ApplicationDbContext context)
        {
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

        public async Task UpdateCoinsBatchAsync(List<BetParticipant> participants, int adjustment)
        {
            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var users = await _context.Users.Where(u => participants.Select(p => p.AppUserId).Contains(u.Id)).ToListAsync();

                foreach (var  p in users)
                {
                    if (p == null)
                        throw new CustomException("User not found when adjusting coins", "USER_NOT_FOUND", 404);

                    if (p.PersonalCoins + adjustment < 0)
                        throw new CustomException($"User {p.UserName} does not have enough coins", "NOT_ENOUGH_COINS", 400);

                }

                foreach (var p in users)
                {
                    p.PersonalCoins += adjustment;
                }
                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch (CustomException)
            {
                await tx.RollbackAsync();
                throw;

            } catch (Exception ex)
            {
                await tx.RollbackAsync();
                throw new CustomException("UNKNOWN_ERROR", ex.Message, 500);
            }
        }

    }
}

