using Microsoft.EntityFrameworkCore;
using PalBet.Data;
using PalBet.Interfaces;
using PalBet.Models;

namespace PalBet.Repository
{
    public class GroupRepository : IGroupRepository
    {

        public readonly ApplicationDbContext _context;
        public GroupRepository(ApplicationDbContext context) {
        
            _context = context;

        }

        public async Task<Group> CreateAsync(Group group)
        {
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<List<Group>> GetGroupsAsync(string userId)
        {
            var groups = await _context.Groups
                .Where(g => g.UserGroups.Any(ug => ug.UserId == userId))
                .Include(g => g.UserGroups).ThenInclude(ug => ug.User)
                .ToListAsync();
            return groups;
        }
    }
}
