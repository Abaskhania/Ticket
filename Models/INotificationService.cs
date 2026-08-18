namespace SupportTicketSystem.Models
{
    public interface INotificationService
    {
        Task NotifyAsync(
        string userId,
        string title,
        string message,
        int? ticketId = null);

        Task NotifyTicketAsync(
            int ticketId,
            string title,
            string message);
    }
}
