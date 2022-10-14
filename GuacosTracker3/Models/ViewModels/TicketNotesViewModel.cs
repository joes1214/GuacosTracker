using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GuacosTracker3.Models.ViewModels
{
    public class TicketNotesViewModel
    {
        public Ticket Ticket;
        public Notes Note;

        public TicketNotesViewModel()
        {

        }

        public TicketNotesViewModel(Ticket viewTicket, Notes viewNote)
        {
            Ticket = viewTicket;
            Note = viewNote;
        }
    }
}
