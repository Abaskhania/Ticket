using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupportTicketSystem.Data;
using SupportTicketSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace SupportTicketSystem.Pages
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LoginModel> _logger;
        public RegisterModel(AppDbContext context, ILogger<LoginModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        public string Result { get; set; }
        [BindProperty]
        public RegisterInput Input { get; set; }

        public class RegisterInput
        {
            [Required(ErrorMessage = "نام کاربری الزامی است")]
            [Display(Name = "نام کاربری")]
            public string Username { get; set; }

            [Required(ErrorMessage = "نام و نام خانوادگی الزامی است")]
            [Display(Name = "نام و نام خانوادگی")]
            public string FullName { get; set; }


            [Required(ErrorMessage = "رمز عبور الزامی است")]
            [DataType(DataType.Password)]
            [Display(Name = "رمز عبور")]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور باید حداقل ۶ کاراکتر باشد")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تکرار رمز عبور")]
            [Required(ErrorMessage = "تکرار رمز عبور الزامی است")]
            [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن مطابقت ندارند")]
            public string ConfirmPassword { get; set; }
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            User uExists = _context.Users.FirstOrDefault(u => u.Username == this.Input.Username)!;
            if (uExists == null)
            {
                User u = new User { Username = this.Input.Username,FullName=this.Input.FullName, Password = this.Input.Password, Role = "Employee" };
                _context.Users.Add(u);
                _context.SaveChanges();
                this.Result = "ثبت‌ نام با موفقیت انجام شد!";
                
            }
            else
            {
                ModelState.AddModelError("کاربر تکراری", "نام کاربری فوق قبلا در سامانه ثبت شده است.");
                return Page();

            }
            // در اینجا منطق ذخیره در دیتابیس را بنویسید


            return Page();
        }
    }
}
