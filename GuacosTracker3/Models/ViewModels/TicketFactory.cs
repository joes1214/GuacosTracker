namespace GuacosTracker3.Models.ViewModels
{
    public static class TicketFactory
    {
        public static TicketViewModel Create(Customers _customers, Ticket _ticket)
        {
            return new TicketViewModel
            {
                Tickets = _ticket,
                Customer = _customers,
            };
        }
    }
}
