using PalBet.Dtos.Notification;
using PalBet.Models;

namespace PalBet.Mappers.Notifications
{
    public static class BetWinnerNotificationMapper
    {
        public static NotificationDto fromNotifcationToBetWinnerNotificationDto(this Notification notification, Bet bet, string winnerUsername)
        {
            return new NotificationDto
            {
                Id = notification.Id,
                NotificationType = notification.NotificationType,
                DateTime = notification.DateTime,
                IsRead = notification.IsRead,
                IsCompleted = notification.IsCompleted,
                Payload = new BetWinnerPayload
                {
                    BetId = bet.Id,
                    BetTitle = bet.BetDescription,
                    WinnerUsername = winnerUsername,

                }
            };
        }
    }
}
