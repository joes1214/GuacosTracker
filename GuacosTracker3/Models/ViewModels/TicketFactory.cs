namespace GuacosTracker3.Models.ViewModels
{
    public static class TicketFactory
    {
        public static TicketViewModel Create(int _custId, Ticket _ticket)
        {
            return new TicketViewModel
            {
                Tickets = _ticket,
                CustomerId = _custId,
            };
        }
    }
}
