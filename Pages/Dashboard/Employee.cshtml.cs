using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SupportTicketSystem.Data;
using SupportTicketSystem.Models;
using System.Security.Claims;

namespace SupportTicketSystem.Pages.Dashboard
{
    [Authorize]
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
        public SelectList Categories { get; set; } = default!;
        
        public async Task OnGetAsync(int pagenumber = 1)
        {
            try
            {

                Categories = new SelectList(
                    _context.Categories.ToList(),
                    "Id",
                    "Name"
                );
                var userIdStr = User.FindFirst("UserId")?.Value;
                currentPage = pagenumber;
                if (int.TryParse(userIdStr, out int userId))
                {
                    // Load the tickets for the current user
                    UserTickets = await _context.Tickets.Include(t=>t.AssignedToUser).AsNoTracking()
                        .Where(t => t.CreatedByUserId == userId)
                        .OrderByDescending(t => t.CreatedAt)
                        .Skip((pagenumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

                    totalcount = await _context.Tickets
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خظا در واکشی اطلاعات از دیتابیس در صفحه Employee");

            }


        }
        public async Task<IActionResult> OnGetEmployeeVerifAsync(int id,int pagenumber=1)
        {
            try
            {
                var userIdStr = User.FindFirst("UserId")?.Value;              
                
                currentPage = pagenumber;
                if (int.TryParse(userIdStr, out int userId))
                {
                    var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id && t.CreatedByUserId == userId);
                    if (ticket != null)
                    {
                        ticket.EmployeeVerif = true;
                        ticket.EmployeeVerifAt = DateTime.Now;
                        await _context.SaveChangesAsync();
                    }


                    // Load the tickets for the current user
                    UserTickets = await _context.Tickets.Include(t => t.AssignedToUser).AsNoTracking()
                        .Where(t => t.CreatedByUserId == userId)
                        .OrderByDescending(t => t.CreatedAt)
                        .Skip((pagenumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

                    totalcount = await _context.Tickets
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خظا در واکشی اطلاعات از دیتابیس در صفحه Employee");
                
            }
            return RedirectToPage("Employee",new { pagenumber = (Request.Query["pagenumber"].ToString() ?? "1") });
           
        }
    }
}
