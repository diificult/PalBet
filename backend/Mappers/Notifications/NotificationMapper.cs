using Microsoft.AspNetCore.Identity;
using PalBet.Dtos.Notification;
using PalBet.Enums;
using PalBet.Interfaces;
using PalBet.Models;

namespace PalBet.Mappers.Notifications
{
    public class NotificationMapper : INotificationMapper
    {

        public readonly UserManager<AppUser> _userManager;
        public readonly IBetRepository _betRepository;


        public  NotificationMapper(UserManager<AppUser> userManager, IBetRepository betRepository)
        {
            _userManager = userManager;
            _betRepository = betRepository;
        }


        public async Task<NotificationDto> Map(Notification notification)
        {
            return notification.NotificationType switch
            {
                NotificationType.FriendRequest =>
                    notification.fromNotifcationToFriendRequestNotificationDto(await _userManager.FindByIdAsync(notification.EntityId)),
                NotificationType.BetRequest =>
                    notification.fromNotifcationToBetRequestNotificationDto(await _betRepository.GetByIdAsync(int.Parse(notification.EntityId))),
                NotificationType.BetInPlay =>
                    notification.fromNotifcationToBetInPlayNotificationDto(await _betRepository.GetByIdAsync(int.Parse(notification.EntityId))),
                NotificationType.WinnerChosen =>
                    notification.fromNotifcationToBetWinnerNotificationDto(await _betRepository.GetByIdAsync(int.Parse(notification.EntityId)), null),
                NotificationType.BetDeadlineReached =>
                    notification.fromNotificationToBetDeadlineReachedNotificationDto(await _betRepository.GetByIdAsync(int.Parse(notification.EntityId))),

            };

        }

    }
}
