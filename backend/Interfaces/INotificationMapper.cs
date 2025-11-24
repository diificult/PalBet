using PalBet.Dtos.Notification;
using PalBet.Models;

namespace PalBet.Interfaces
{
    public interface INotificationMapper
    {
        public Task<NotificationDto> Map(Notification notification);
    }
}