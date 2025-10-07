using Microsoft.EntityFrameworkCore;
using PalBet.Data;
using PalBet.Enums;
using PalBet.Interfaces;
using PalBet.Models;

namespace PalBet.Repository
{
    public class NotificationRepository : INotificationRepository
    {

        private readonly ApplicationDbContext _context;
        public NotificationRepository(ApplicationDbContext context) {
            _context = context;
        }
        public async Task<Notification> CreateNotification(Notification notification)
        {
            await _context.AddAsync(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public Task<Notification?> GetNotification(string notifeeId, string entityId)
        {
            return _context.notification.FirstOrDefaultAsync(n => n.NotifyeeId == notifeeId && n.EntityId == entityId && !n.IsCompleted);
        }

        public async Task<List<Notification>> GetNotifications(string notifeeId)
        {
            return await _context.notification.Where(n => n.NotifyeeId == notifeeId && !n.IsCompleted).ToListAsync();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

       
    }
}
