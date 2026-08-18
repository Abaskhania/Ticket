using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SupportTicketSystem.Models
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; } = null;
        public string Description { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "در انتظار بررسی";
        public DateTime? StatusAt { get; set; } = DateTime.Now;
        public string? Priority { get; set; } = null;
        public string? CategoryId { get; set; } = null;
        public string? CategoryText { get; set; } = null;
        public string? AttachmentPath { get; set; }  


        public int CreatedByUserId { get; set; }
        public User? CreatedByUser { get; set; }

        public int? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }
        public DateTime? AssignedAt { get; set; } = DateTime.Now;
        public string CreatedAtPersion
        {
            get
            {
                PersianCalendar pc = new PersianCalendar();
                return $"{pc.GetYear(CreatedAt)}/{pc.GetMonth(CreatedAt):00}/{pc.GetDayOfMonth(CreatedAt):00}";
            }
        }
        public string CreatedAtPersion2
        {
            get
            {
                PersianCalendar pc = new PersianCalendar();
                return $"{pc.GetYear(CreatedAt)}/{pc.GetMonth(CreatedAt):00}/{pc.GetDayOfMonth(CreatedAt):00}" + $" {pc.GetHour(CreatedAt):00}:{pc.GetMinute(CreatedAt):00}"; ;
            }
        }
        public bool? EmployeeVerif { get; set; }
        public DateTime? EmployeeVerifAt { get; set; }
        public string AssignedAtPersion
        {
            get
            {
                if (!AssignedAt.HasValue)
                    return null;
                var date = AssignedAt.Value;
                PersianCalendar pc = new PersianCalendar();
                return $"{pc.GetYear(date)}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}" + $" {pc.GetHour(date):00}:{pc.GetMinute(date):00}";


            }
        }
        public string EmployeeVerifAtPersion
        {
            get
            {
                if (!EmployeeVerifAt.HasValue)
                    return null;
                var date = EmployeeVerifAt.Value;
                PersianCalendar pc = new PersianCalendar();
                return $"{pc.GetYear(date)}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}" + $" {pc.GetHour(date):00}:{pc.GetMinute(date):00}";

            }
        }
        public string StatusAtPersion
        {
            get
            {
                if (!StatusAt.HasValue)
                    return null;
                var date = StatusAt.Value;
                PersianCalendar pc = new PersianCalendar();
                return $"{pc.GetYear(date)}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}" + $" {pc.GetHour(date):00}:{pc.GetMinute(date):00}";

            }
        }

    }
}
