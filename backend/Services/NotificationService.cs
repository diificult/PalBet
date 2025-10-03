using Microsoft.AspNetCore.Identity;
using PalBet.Dtos.Notification;
using PalBet.Enums;
using PalBet.Interfaces;
using PalBet.Mappers;
using PalBet.Models;

namespace PalBet.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBetRepository _betRepository;


        public NotificationService(INotificationRepository notificationRepository, UserManager<AppUser> userManager, IBetRepository betRepository)
        {
            _notificationRepository = notificationRepository;
            _userManager = userManager;
            _betRepository = betRepository;
        }

        public async Task<Notification> CreateNotification(NotificationType type, string EntityId, string notifeeId)
        {
            var notification = new Notification
            {
                DateTime = DateTime.Now,
                EntityId = EntityId,
                NotificationType = type,
                NotifyeeId = notifeeId,

            };

            var CreatedNotification = await _notificationRepository.CreateNotification(notification);
            return CreatedNotification;
        }

        public async Task<int> GetNotificationCount(string notifeeId)
        {

            var notifications = await _notificationRepository.GetNotifications(notifeeId);
            var count = notifications.Where(n => !n.IsRead).Count();
            return count;
        }

        public async Task<List<NotificationDto>> GetNotifications(string notifeeId)
        {
            var notifications = await _notificationRepository.GetNotifications(notifeeId);
            var notificationDtos = new List<NotificationDto>();
            foreach (Notification notification in notifications) {
                switch (notification.NotificationType)
                {
                    case (NotificationType.FriendRequest):
                        var appUser = await _userManager.FindByIdAsync(notification.EntityId);
                        notificationDtos.Add(notification.fromNotifcationToFriendRequestNotificationDto(appUser));
                        break;
                    case (NotificationType.BetRequest):
                        var bet = await _betRepository.GetByIdAsync(int.Parse(notification.EntityId));
                        notificationDtos.Add(notification.fromNotifcationToBetRequestNotificationDto(bet));
                        break;
                    case (NotificationType.BetInPlay):
                        var bet1 = await _betRepository.GetByIdAsync(int.Parse(notification.EntityId));
                        notificationDtos.Add(notification.fromNotifcationToBetInPlayNotificationDto(bet1));
                        break;
                    case (NotificationType.WinnerChosen):
                        var bet2 = await _betRepository.GetByIdAsync(int.Parse(notification.EntityId));
                        var winner = await _userManager.FindByIdAsync(bet2.UserWinner);
                        notificationDtos.Add(notification.fromNotifcationToBetWinnerNotificationDto(bet2, winner.UserName));
                        break;
                }
                                    
            }
            await MarkAsRead(notifications);
            notificationDtos.Reverse();
            return notificationDtos;
        }

        public async Task<List<Notification>> MarkAsRead(List<Notification> notifications)
        {
            foreach (Notification n in notifications)
            {
                n.IsRead = true;
            }
            await _notificationRepository.SaveAsync();
            return notifications;
        }
    }
}
