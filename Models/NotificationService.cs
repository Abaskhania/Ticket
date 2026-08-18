using Microsoft.AspNetCore.SignalR;
using SupportTicketSystem.Data;

namespace SupportTicketSystem.Models
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly AppDbContext _context;

        public NotificationService(
            IHubContext<NotificationHub> hubContext,
            AppDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        public async Task NotifyAsync(
            string userId,
            string title,
            string message,
            int? ticketId = null)
        {
            var notification = new PushNotification
            {
                UserId = userId,
                Title = title,
                Message = message,
                TicketId = ticketId,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.PushNotifications.Add(notification);
            await _context.SaveChangesAsync();

            await _hubContext
                .Clients
                .User(userId)
                .SendAsync("NotificationReceived", new
                {
                    id = notification.Id,
                    title = notification.Title,
                    message = notification.Message,
                    ticketId = notification.TicketId,
                    createdAt = notification.CreatedAt
                });
        }

        public async Task NotifyTicketAsync(
            int ticketId,
            string title,
            string message)
        {
            // این قسمت را بر اساس ساختار Ticket خودت پیاده کن
        }
    }
}
