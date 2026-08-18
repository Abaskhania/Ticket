using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SupportTicketSystem.Models
{
    public class UserLoginLog
    {
        public long Id { get; set; }

        public string? UserId { get; set; }

        public DateTime LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }

        public bool IsSuccessful { get; set; }

        public string? LogoutTime_Persian
        {
            get
            {
                if (!LogoutTime.HasValue)
                    return null;
                var date = LogoutTime.Value;
                PersianCalendar pc = new PersianCalendar();
                return $"{pc.GetYear(date)}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}" + $" {pc.GetHour(date):00}:{pc.GetMinute(date):00}";


            }
        }
        public string? LoginTime_Persian
        {
            get
            {
                
                PersianCalendar pc = new PersianCalendar();
                return $"{pc.GetYear(LoginTime)}/{pc.GetMonth(LoginTime):00}/{pc.GetDayOfMonth(LoginTime):00}" + $" {pc.GetHour(LoginTime):00}:{pc.GetMinute(LoginTime):00}";


            }
        }
    }
}
