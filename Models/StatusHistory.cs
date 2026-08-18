using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupportTicketSystem.Models
{
    public class StatusHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public string Status { get; set; } = "در انتظار بررسی";
        public DateTime? StatusAt { get; set; } = DateTime.Now;
        public int StatusUserId { get; set; }
        public User? StatusUser { get; set; }
    }
}
