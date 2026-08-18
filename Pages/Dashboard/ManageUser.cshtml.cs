using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupportTicketSystem.Models;
using SupportTicketSystem.Data;

namespace SupportTicketSystem.Pages.Dashboard
{ 
    [Authorize(Roles = "Admin")]
    public class ManageUserModel : PageModel
    {
        private readonly AppDbContext _context;

        public ManageUserModel(AppDbContext context)
        {
            _context = context;
        }
        public IList<User> satraUsers { get; set; }
        public void OnGet()
        {
            this.satraUsers = _context.Users.AsNoTracking().ToList();
        }
        public IActionResult OnGetChangeState(int id)
        {
            User u = _context.Users.First(t => t.Id == id);
            u.IsValid = !u.IsValid;
            _context.SaveChanges();
            return RedirectToPage("ManageUser");
        }
    }
}
