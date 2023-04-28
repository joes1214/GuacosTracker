using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GuacosTracker3.Models.ViewModels
{
    public class TicketNotesViewModel
    {
        public Ticket Ticket;
        public Note Note;

        public TicketNotesViewModel(Ticket viewTicket, Note viewNote)
        {
            Ticket = viewTicket;
            Note = viewNote;
        }
    }
}
