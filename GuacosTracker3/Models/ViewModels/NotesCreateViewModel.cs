namespace GuacosTracker3.Models.ViewModels
{
    public class NotesCreateViewModel
    {
        public static Notes Create(Guid TicketId)
        {
            return new Notes
            {
                TicketId = TicketId
            };

        }
    }
}
