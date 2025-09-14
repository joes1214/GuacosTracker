using System.ComponentModel.DataAnnotations;

namespace GuacosTracker3.Models.ViewModels
{
    public class EditTicketViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 10, 
            ErrorMessage = "Title must be between 5 and 100 characters.")]
        public string Title { get; set; }

        [Required]
        public int CustomerID { get; set; }

        public bool IsPriority { get; set; } = false;
        public bool IsClosed { get; set; } = false;

        //Static, not to be edited
        public string? Customer { get; set; }
        public DateTime DateCreated { get; set; }

        [Display(Name = "Created by")]
        public string? EmployeeName { get; set; } = "--";
    }
}
