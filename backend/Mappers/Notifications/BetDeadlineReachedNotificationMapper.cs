using PalBet.Dtos.Notification;
using PalBet.Models;

namespace PalBet.Mappers.Notifications
{
    public static class BetDeadlineReachedNotificationMapper
    {
        public static NotificationDto fromNotificationToBetDeadlineReachedNotificationDto(this Notification notification, Bet bet)
        {
            return new NotificationDto
            {
                Id = notification.Id,
                NotificationType = notification.NotificationType,
                DateTime = notification.DateTime,
                IsRead = notification.IsRead,
                IsCompleted = notification.IsCompleted,
                Payload = new BetDeadlineReachedPayload
                {
                    BetId = bet.Id,
                    BetTitle = bet.BetDescription,
                }
            };
        }
    }
}
