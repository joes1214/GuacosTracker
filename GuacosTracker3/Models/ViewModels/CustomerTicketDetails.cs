using static GuacosTracker3.Utilities.Pagination;

namespace GuacosTracker3.Models.ViewModels
{
    public class CustomerTicketDetails
    {
        public Customer Customer { get; set; }
        public PaginatedList<Ticket> Tickets { get; set; }
    }
}
