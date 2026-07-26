using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SupportTicketSystem.Data;
using SupportTicketSystem.Models;
using System.Security.Claims;

namespace SupportTicketSystem.Pages.Dashboard
{
    public class EmployeeModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EmployeeModel> _logger;
        public EmployeeModel(AppDbContext context, ILogger<EmployeeModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Holds the list of tickets for the current user
        public List<Ticket> UserTickets { get; set; } = new();
        public int pageSize { get; set; } = 10;
        public int totalcount { get; set; }
        public int currentPage { get; set; }
        // This will be shown instead of "پنل کاربر"
        public string DisplayName { get; set; }

        public async Task OnGetAsync(int pagenumber=1)
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            currentPage = pagenumber;
            if (int.TryParse(userIdStr, out int userId))
            {
                // Load the tickets for the current user
                UserTickets = await _context.Tickets.AsNoTracking()
                    .Where(t => t.CreatedByUserId == userId)
                    .OrderByDescending(t => t.CreatedAt)  
                    .Skip((pagenumber-1)*pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                totalcount= await _context.Tickets
                    .Where(t => t.CreatedByUserId == userId).CountAsync()
                    ;
                // Try to get user's full name from the Users table
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                // If found, set DisplayName to full name; fallback to username if not
                DisplayName = user?.FullName ?? User.Identity?.Name ?? "کاربر";
            }
            else
            {
                DisplayName = "کاربر"; // Default fallback
            }
            _logger.LogInformation("Form was submitted");
        }
    }
}
