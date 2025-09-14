namespace GuacosTracker3.Models.ViewModels
{
    public class TicketNoteCustomerViewModel
    {
        public Ticket Ticket { get; set; }
        public Customer Customer { get; set; }

        public Note? Note { get; set; }
        public Note? RecentNote { get; set; }

        public List<Note> Notes { get; set; }
    }
}
