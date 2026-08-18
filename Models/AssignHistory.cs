using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupportTicketSystem.Models
{
    public class AssignHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public int? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }
        public DateTime? AssignedAt { get; set; } = DateTime.Now;
    }
}
