using Microsoft.AspNetCore.Mvc.Rendering;
using static GuacosTracker3.Utilities.Pagination;

namespace GuacosTracker3.Models.ViewModels
{
    public class TicketViewModel
    {
        public Ticket Ticket { get; set; }
        public Customer? Customer { get; set; }

        public PaginatedList<GuacosTracker3.Models.Ticket> Tickets { get; set; }
        //To display multiple tickets.
    }
}
