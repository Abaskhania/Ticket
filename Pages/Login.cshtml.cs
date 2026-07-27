using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportTicketSystem.Data;
using SupportTicketSystem.Pages.Dashboard;
using System.Security.Claims;

namespace SupportTicketSystem.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LoginModel> _logger;
        public LoginModel(AppDbContext context, ILogger<LoginModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await HttpContext.SignOutAsync("Cookies");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //try
            {


                var user = _context.Users.FirstOrDefault(u => u.Username == Username && u.Password == Password);

                if (user == null)
                {
                    ErrorMessage = "❌ اطلاعات ورود اشتباه است.";
                    return Page();
                }

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role ?? "")
            };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal);

                return user.Role switch
                {
                    "Admin" => RedirectToPage("/Dashboard/Admin"),
                    "IT" => RedirectToPage("/Dashboard/IT"),
                    _ => RedirectToPage("/Dashboard/Employee")
                };
            }
            /*catch (Exception ex)
            {
                _logger.LogError(ex, "خظا در واکشی اطلاعات از دیتابیس در صفحه Login");
                return StatusCode(500, "خطا در سمت سرور");

            }*/
        }
    }
}
