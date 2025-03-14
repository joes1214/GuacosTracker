using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuacosTracker3.Models.ViewModels
{
    public class TicketViewModel
    {
        public Ticket Ticket { get; set; }
        public string CustomerName { get; set; }
        public Customer Customer { get; set; }

        public TicketViewModel() { }

        public TicketViewModel(Ticket ticket, string fname, string lname)
        {
            Ticket = ticket;
            CustomerName = $"{lname}, {fname}";
        }
    }
}
