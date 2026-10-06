using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupportTicketSystem.Data;
using System.Security.Claims;

namespace SupportTicketSystem.Pages.Dashboard
{
    [Authorize]
    public class ITModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ITModel> _logger;
        public ITModel(AppDbContext context, ILogger<ITModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Display name to show in the UI
        public string DisplayName { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                // Get user ID from claims
                var userIdStr = User.FindFirst("UserId")?.Value;

                if (int.TryParse(userIdStr, out int userId))
                {
                    // Try to find user in the database
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

                    // Set display name from database or fallback to claim name
                    DisplayName = user?.FullName ?? User.Identity?.Name ?? "کاربر";
                }
                else
                {
                    DisplayName = "کاربر";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خظا در واکشی اطلاعات از دیتابیس در صفحه IT");
            }
        }
    }
}
