using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface INotificationRepository
    {
        public Task<Notification> CreateNotification(Notification notification);
        public Task<List<Notification>> GetNotifications(string notifeeId);
        public Task SaveAsync();
        public Task<Notification?> GetNotification(string notifeeId, string entityId);
    }
}
