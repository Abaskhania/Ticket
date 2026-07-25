using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

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
        //public string AssignedAtPersion
        //{
        //    get
        //    {
        //        PersianCalendar pc = new PersianCalendar();
        //        return $"{pc.GetYear(AssignedAt)}/{pc.GetMonth(AssignedAt):00}/{pc.GetDayOfMonth(AssignedAt):00}";
        //    }
        //}
        //public string StatusAtPersion
        //{
        //    get
        //    {
        //        PersianCalendar pc = new PersianCalendar();
        //        return $"{pc.GetYear(StatusAt)}/{pc.GetMonth(StatusAt):00}/{pc.GetDayOfMonth(StatusAt):00}";
        //    }
        //}

    }
}
