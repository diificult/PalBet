using PalBet.Dtos.Notification;
using PalBet.Enums;
using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface INotificationService
    {
        public Task<Notification> CreateNotification(NotificationType type, string EntityId, string notifeeId);

        public Task<List<NotificationDto>> GetNotifications(string notifeeId   );
        public Task<int> GetNotificationCount(string notifeeId);
        public Task MarkAsComplete(string notifee, string entityId);
    }
}
