using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SupportTicketSystem.Pages.Dashboard
{
    [Authorize]
    public class AdminModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
