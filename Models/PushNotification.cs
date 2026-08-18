namespace SupportTicketSystem.Models
{
    public class PushNotification
    {
        public long Id { get; set; }

        public string UserId { get; set; } = null!;

        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;

        public string? Type { get; set; }

        public int? TicketId { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
