using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuacosTracker3.Models.ViewModels
{
    public class TicketViewModel
    {
        public Ticket Ticket { get; set; }
        public Customer Customer { get; set; }
    }
}
