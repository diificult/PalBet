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

        public async Task<List<Notification>> GetNotifications(string notifeeId)
        {
            return await _context.notification.Where(n => n.NotifyeeId == notifeeId).ToListAsync();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
