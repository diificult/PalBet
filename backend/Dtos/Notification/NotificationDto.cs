using PalBet.Enums;
using PalBet.Models;

    namespace PalBet.Dtos.Notification
    {
        public class NotificationDto
        {
            public int Id { get; set; }
            public NotificationType NotificationType { get; set; }
            public DateTime DateTime { get; set; } = DateTime.Now;
            public bool IsRead { get; set; } = false;
            public bool IsCompleted { get; set; } = false;

            public required object Payload { get; set; } 
        }

        public class FriendRequestPayload
        {
            public string FromUserId { get; set; }
            public string FromUserName { get; set; }
        }

        public class BetRequestPayload
        {
            public int BetId { get; set; }
            public string BetTitle { get; set; }
            public string CreatedByUserName { get; set; }
        }

        public class BetInPlayPayload
        { 
            public int BetId { get; set; } 
            public string BetTitle { get; set; }
        }
        public class BetWinnerPayload { 
    
            public int BetId { get; set; }
            public string BetTitle { get; set; }
            public string WinnerUsername { get; set; }
        }

        public class BetDeadlineReachedPayload
        {
            public int BetId { get; set; }
            public string BetTitle { get; set; }
        }   

}
