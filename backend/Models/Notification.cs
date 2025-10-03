using PalBet.Enums;

namespace PalBet.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string NotifyeeId { get; set; }
        public AppUser Notifyee { get; set; }
        public NotificationType NotificationType { get; set; }
        public string EntityId { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
        public bool IsCompleted { get; set; } = false;

    }
}
