using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SupportTicketSystem.Data;
using SupportTicketSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace SupportTicketSystem.Pages.Dashboard
{
    [Authorize(Roles = "Admin")]
    public class ResetPasswordModel : PageModel
    {
        private readonly AppDbContext _context;

        public ResetPasswordModel(AppDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public RegisterInput Input { get; set; }
        public string Result { get; set; }
        public class RegisterInput
        {
            [Required(ErrorMessage = "نام کاربری الزامی است")]
            [Display(Name = "نام کاربری")]
            public string Username { get; set; }


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

        public void OnGet(int id)
        {
            User uExists = _context.Users.FirstOrDefault(u => u.Id == id)!;
            this.Input = new RegisterInput { Username = uExists.Username };
            
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string hashPassword = this.Input.Password;
            User uExists = _context.Users.FirstOrDefault(u => u.Username == this.Input.Username)!;
            if (uExists != null)
            {
                //User u = new User { Username = this.Input.Username, Password = hashPassword, Role = "User" };
                // _context.SatraUser.Add(u)
                uExists.Password = hashPassword;
                _context.SaveChanges();
                this.Result = "تغییر رمز عبور با موفقیت انجام شد!";
            }
            else
            {
                ModelState.AddModelError("کاربر موجود نیست", "نام کاربری فوق وجود ندارد.");
                return Page();

            }
            // در اینجا منطق ذخیره در دیتابیس را بنویسید


            return Page();
        }
        
    }
}
