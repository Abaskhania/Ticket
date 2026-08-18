using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupportTicketSystem.Data;
using SupportTicketSystem.Models;
using System.Globalization;

namespace SupportTicketSystem.Pages.Dashboard
{
    [Authorize(Roles ="Admin")]
    public class ReportAdminModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LoginModel> _logger;
        public ReportAdminModel(AppDbContext context, ILogger<LoginModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        [BindProperty]
        public string? FromDate { get; set; }
        [BindProperty]
        public string? ToDate { get; set; }

        public IList<Report1Result> ReportResult { get; set; }
        public void OnGet()
        {
        }
        public void OnPost()
        {

            DateTime fromDate = PersianToGregorian(this.FromDate);
            DateTime toDate = PersianToGregorian(this.ToDate);
            this.ReportResult = _context.Report1Results.FromSqlInterpolated(
            $"EXEC Sp_Sel_Report1 @FromDate = {fromDate}, @ToDate = {toDate}")
            .ToList();

        }
        private static DateTime PersianToGregorian(string persianDate)
        {
            persianDate = PersianDigitsToEnglish(persianDate);

            var parts = persianDate.Split('/');

            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);

            var pc = new PersianCalendar();

            return pc.ToDateTime(year, month, day, 0, 0, 0, 0);
        }
        private static string PersianDigitsToEnglish(string input)
        {
            return input
                .Replace('۰', '0')
                .Replace('۱', '1')
                .Replace('۲', '2')
                .Replace('۳', '3')
                .Replace('۴', '4')
                .Replace('۵', '5')
                .Replace('۶', '6')
                .Replace('۷', '7')
                .Replace('۸', '8')
                .Replace('۹', '9');
        }
    }
}
