using Microsoft.AspNetCore.SignalR;

namespace SupportTicketSystem.Models
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendAsync(string userId, string message)
        {
            await _hubContext.Clients.User(userId)
                .SendAsync("ReceiveNotification", new
                {
                    Message = message,
                    CreatedAt = DateTime.UtcNow
                });
        }
    }
}
