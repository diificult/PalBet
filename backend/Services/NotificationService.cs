using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using PalBet.Dtos.Notification;
using PalBet.Enums;
using PalBet.Hubs;
using PalBet.Interfaces;
using PalBet.Mappers;
using PalBet.Mappers.Notifications;
using PalBet.Models;

namespace PalBet.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBetRepository _betRepository;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly INotificationMapper _notificationMapper;


        public NotificationService(INotificationRepository notificationRepository, UserManager<AppUser> userManager, IBetRepository betRepository, IHubContext<NotificationHub> hub, INotificationMapper notificationMapper)
        {
            _notificationRepository = notificationRepository;
            _userManager = userManager;
            _betRepository = betRepository;
            _hub = hub; 
            _notificationMapper = notificationMapper;
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

            await _hub.Clients.User(notifeeId).SendAsync("RecievedNotification", await _notificationMapper.Map(CreatedNotification));

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
            foreach (Notification notification in notifications) 
                notificationDtos.Add(await _notificationMapper.Map(notification));
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

        public async Task MarkAsComplete(string notifee, string entityId)
        {
            var notification = await _notificationRepository.GetNotification(notifee, entityId);
            if (notification != null)
            {
                notification.IsCompleted = true;
                await _notificationRepository.SaveAsync();
            }
            
            

        }
    }
}
