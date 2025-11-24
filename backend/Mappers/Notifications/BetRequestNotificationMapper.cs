using PalBet.Dtos.Notification;
using PalBet.Models;

namespace PalBet.Mappers.Notifications
{
    public static class BetRequestNotificationMapper
    {
        public static NotificationDto fromNotifcationToBetRequestNotificationDto(this Notification notification, Bet bet)
        {
            return new NotificationDto
            {
                Id = notification.Id,
                NotificationType = notification.NotificationType,
                DateTime = notification.DateTime,
                IsRead = notification.IsRead,
                IsCompleted = notification.IsCompleted,
                Payload = new BetRequestPayload
                {
                    BetId = bet.Id,
                    BetTitle = bet.BetDescription,
                    CreatedByUserName = bet.Participants.Where(p => p.IsBetHost).First().AppUser.UserName
                }
            };
        }
    }
}
