using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuacosTracker3.Models
{
    public class Customer
    {
        [Key]
        [Display(Name = "Customer ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [Column(TypeName = "nvarchar(100)")]
        [MaxLength(100, ErrorMessage = "Too many characters! Max 100.")]
        public string FName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [Column(TypeName = "nvarchar(100)")]
        [MaxLength(100, ErrorMessage = "Too many characters! Max 100.")]
        public string LName { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [MaxLength(100, ErrorMessage = "Too many characters! Max 150.")]
        public string? Address { get; set; }

        [Required]
        [Display(Name = "Primary Phone Number")]
        [Column(TypeName = "nvarchar(10)")]
        [MaxLength(10, ErrorMessage = "Too many characters! Max 10.")]
        [MinLength(10, ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Alternate Phone Number")]
        [Column(TypeName = "nvarchar(10)")]
        [MaxLength(10, ErrorMessage = "Too many characters! Max 10.")]
        [MinLength(10, ErrorMessage = "Invalid Phone Number")]
        public string? AltPhone { get; set; }

        [Display(Name = "Email Address")]
        [Column(TypeName = "nvarchar(100)")]
        [MaxLength(100, ErrorMessage = "Too many characters! Max 100.")]
        public string? Email { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }

        public string GetFullName()
        {
            return $"{LName}, {FName}";
        }

    }
}
