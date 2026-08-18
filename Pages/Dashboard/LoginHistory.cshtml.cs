using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupportTicketSystem.Data;
using SupportTicketSystem.Models;


namespace SupportTicketSystem.Pages.Dashboard
{
    [Authorize(Roles = "Admin")]
    public class LoginHistoryModel : PageModel
    {
        private readonly AppDbContext _context;

        public LoginHistoryModel(AppDbContext context)
        {
            _context = context;
        }
        public IList<UserLoginLog> UserLoginHistory { get; set; }
        public void OnGet(int id)
        {
            User _user = _context.Users.AsNoTracking().Where(u=>u.Id==id).FirstOrDefault();
            this.UserLoginHistory = _context.UserLoginLogs.Where(l => l.UserId == _user.Username).OrderByDescending(t=>t.LoginTime).ToList();
        }
    }
}
