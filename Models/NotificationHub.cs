using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SupportTicketSystem.Data;

namespace SupportTicketSystem.Models
{
    public class NotificationHub : Hub
    {
        private readonly AppDbContext _context;
        

        public NotificationHub(AppDbContext context)
        {
            _context = context;
            
        }
        public async Task SendNotification(string userId, string message)
        {
            await Clients.User(userId)
                .SendAsync("ReceiveNotification", message);
        }
        public async Task MarkAsRead(int notificationId)
        {
            var userId = Context.UserIdentifier;

            var notification = await _context.PushNotifications
                .FirstOrDefaultAsync(x =>
                    x.Id == notificationId &&
                    x.UserId == userId);

            if (notification == null)
                return;

            if (!notification.IsRead)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
