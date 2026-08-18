// Pages/Dashboard/Logout.cshtml.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupportTicketSystem.Data;
using System.Security.Claims;

namespace SupportTicketSystem.Pages.Dashboard
{
    public class LogoutModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LoginModel> _logger;
        public LogoutModel(AppDbContext context, ILogger<LoginModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> OnGet()
        {
            var username = User.FindFirst("username")?.Value;
            var login = await _context.UserLoginLogs
           .Where(x => x.UserId == username && x.IsSuccessful)
           .OrderByDescending(x => x.LoginTime)
           .FirstOrDefaultAsync();

            if (login != null)
            {
                login.LogoutTime = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            await HttpContext.SignOutAsync(); 
            return RedirectToPage("/Login");  
        }
    }
}
