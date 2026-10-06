using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportTicketSystem.Data;
using SupportTicketSystem.Models;
using static SupportTicketSystem.Pages.RegisterModel;

namespace SupportTicketSystem.Pages.Dashboard
{
    [Authorize(Roles="IT,Admin")]
    public class AddCategoryModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LoginModel> _logger;
        public AddCategoryModel(AppDbContext context, ILogger<LoginModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        public string Result { get; set; }
        [BindProperty]
        public Category Input { get; set; }
        public IList<Category> Categories { get; set; }
        public void OnGet()
        {
            this.Categories = _context.Categories.ToList();
        }
        public void OnPost()
        {
            Category c = new Category() { Name = Input.Name, CreatedBy = User.Identity.Name };
            _context.Categories.Add(c);
            _context.SaveChanges();
            this.Categories = _context.Categories.ToList();
            this.Result = "دسته بندی ایجاد گردید.";
        }
    }
}
