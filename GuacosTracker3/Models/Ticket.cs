using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuacosTracker3.Models
{
    public class Ticket
    {
        [Key]
        //See how to create random IDs
        [Display(Name = "Ticket ID")]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Customers")]
        [Display(Name = "Customer ID")]
        public int CustomerId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        [MaxLength(100, ErrorMessage = "Too many characters! Max 100.")]
        [MinLength(5, ErrorMessage = "Please write a short Title!")]
        public string Title { get; set; }

        [Required]
        [ForeignKey("Employee")]
        [Display(Name = "Employee ID")]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        [MaxLength(4000, ErrorMessage = "Too many characters! Max 4,000.")]
        [MinLength(10, ErrorMessage = "Please write a short description!")]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(25)")]
        public string Status { get; set; }
        //In-Progress | Awaiting Repair | Completed | Awaiting Customer

        [Display(Name = "Priority?")]
        public bool Priority { get; set; } = false;

        //Notes Relationship
        public virtual ICollection<Notes> Notes { get; set; }
    }
}
