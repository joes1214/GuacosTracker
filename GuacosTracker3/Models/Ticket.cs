using GuacosTracker3.SharedData;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuacosTracker3.Models
{
    public class Ticket
    {
        [Key]
        //See how to create random IDs
        [Display(Name = "Ticket ID")]
        public Guid Id { get; set; } = new Guid();

        [Required]
        [Display(Name = "Employee ID")]
        public string EmployeeID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        [MaxLength(100, ErrorMessage = "Too many characters! Max 100.")]
        [MinLength(5, ErrorMessage = "Please write a short Title!")]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(25)")]
        [Display(Name = "Status")]
        public string CurrentStatus { get; set; }
        //In-Progress | Awaiting Repair | Completed | Awaiting Customer

        [Display(Name = "Priority?")]
        public bool Priority { get; set; } = false;

        //Hides instead of deletes
        public bool IsClosed { get; set; } = false;

        [Required]
        public DateTime DateCreated { get; init; } = DateTime.Now;

        public DateTime RecentChange { get; set; } = DateTime.Now;

        //Relations
        public Customer Customer { get; set; }
        //public int CustomerID { get; set; }

        public Ticket() { }

        public Ticket(string title, string employeeID, string status, Customer customer)
        {
            Title = title;
            EmployeeID = employeeID;
            CurrentStatus = status;
            Customer = customer;
        }
    }
}
