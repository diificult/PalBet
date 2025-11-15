using PalBet.Dtos.Notification;
using PalBet.Models;

namespace PalBet.Mappers
{
    public static class NotificationMapper
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
                    CreatedByUserName = bet.Participants.Where(p => p.isBetHost).First().appUser.UserName
                }
            };
        }
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
