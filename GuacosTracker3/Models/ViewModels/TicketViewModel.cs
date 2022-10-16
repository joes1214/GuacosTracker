using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuacosTracker3.Models.ViewModels
{
    public class TicketViewModel
    {
        public Ticket Ticket { get; set; }
        public Customer? Customer { get; set; }

        public List<Ticket> Tickets { get; set; }
        //To display multiple tickets.
    }
}
