using GuacosTracker3.SharedData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuacosTracker3.Models
{
    public class Note
    {
        [Key]
        [Required]
        //See how to create random IDs
        [Display(Name = "Notes ID")]
        public Guid Id { get; set; } = new Guid();

        [Required]
        [ForeignKey("Tickets")]
        [Display(Name = "Ticket ID")]
        public Guid TicketId { get; set; }

        [Required]
        [ForeignKey("Employee")]
        [Display(Name = "Employee ID")]
        public int EmployeeId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        [Display(Name = "Description")]
        [MaxLength(4000, ErrorMessage = "Too many characters! Max 4,000.")]
        [MinLength(10, ErrorMessage = "Please write a short description!")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Status")]
        [Column(TypeName = "nvarchar(25)")]
        public string Status { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
