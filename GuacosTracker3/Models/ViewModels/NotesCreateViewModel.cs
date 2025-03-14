namespace GuacosTracker3.Models.ViewModels
{
    public class NotesCreateViewModel
    {
        public static Note Create(Guid TicketId)
        {
            return new Note
            {
                TicketID = TicketId
            };

        }
    }
}
