using PalBet.Dtos.Notification;
using PalBet.Models;

namespace PalBet.Mappers.Notifications
{
    public static class FriendRequestNotificationMapper
    {
        public static NotificationDto fromNotifcationToFriendRequestNotificationDto(this Notification notification, AppUser fromUser)
        {
            return new NotificationDto
            {
                Id = notification.Id,
                NotificationType = notification.NotificationType,
                DateTime = notification.DateTime,
                IsRead = notification.IsRead,
                IsCompleted = notification.IsCompleted,
                Payload = new FriendRequestPayload
                {
                    FromUserId = fromUser.Id,
                    FromUserName = fromUser.UserName,
                }
            };
        }
    }
}
