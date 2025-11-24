using PalBet.Dtos.Notification;
using PalBet.Models;

namespace PalBet.Mappers.Notifications
{
    public static class BetInPlayNotificationMapper
    {

        public static NotificationDto fromNotifcationToBetInPlayNotificationDto(this Notification notification, Bet bet)
        {
            return new NotificationDto
            {
                Id = notification.Id,
                NotificationType = notification.NotificationType,
                DateTime = notification.DateTime,
                IsRead = notification.IsRead,
                IsCompleted = notification.IsCompleted,
                Payload = new BetInPlayPayload
                {
                    BetId = bet.Id,
                    BetTitle = bet.BetDescription,

                }
            };
        }
    }
}
