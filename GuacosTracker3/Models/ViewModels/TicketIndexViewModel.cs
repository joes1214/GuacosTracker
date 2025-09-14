using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuacosTracker3.Models.ViewModels
{
    public class TicketIndexViewModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public string Title { get; set; }
        public DateTime RecentChange { get; set; }

        //public Ticket Ticket { get; set; }
        //public string CustomerName { get; set; }
        //public Customer Customer { get; set; }

        //public TicketViewModel() { }

        //public TicketViewModel(Ticket ticket, string fname, string lname)
        //{
        //    Ticket = ticket;
        //    CustomerName = $"{lname}, {fname}";
        //}
    }
}
