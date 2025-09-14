using System.ComponentModel.DataAnnotations;

namespace GuacosTracker3.Models.ViewModels
{
    public class TicketCreateViewModel
    {
        public int? CustomerID { get; set; }

        [Required]
        [MaxLength(4000, ErrorMessage = "Too many characters! Max 4,000.")]
        [MinLength(10, ErrorMessage = "Please write a short description!")]
        public string Description { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Too many characters! Max 100.")]
        [MinLength(5, ErrorMessage = "Please write a short Title!")]
        public string Title { get; set; }

        [Required]
        public string Status { get; set; }

        public string? CustomerName { get; set; }

    }
}
