using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupportTicketSystem.Data;
using SupportTicketSystem.Models;

namespace SupportTicketSystem.Pages.Dashboard
{
    public class TicketDetailModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LoginModel> _logger;
        public TicketDetailModel(AppDbContext context, ILogger<LoginModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        public Ticket TicketDet { get; set; }
        public void OnGet(int id)
        {
            TicketDet=_context.Tickets.Include(t => t.AssignedToUser).Include(t=>t.CreatedByUser).FirstOrDefault(t => t.Id == id);
        }
    }
}
