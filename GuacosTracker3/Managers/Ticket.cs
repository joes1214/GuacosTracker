using GuacosTracker3.Models;
using GuacosTracker3.Data;
using Microsoft.EntityFrameworkCore;

namespace GuacosTracker3.Managers
{
    public class Ticket(Models.Ticket ticket)
    {
        private readonly Models.Ticket? ticket = ticket;

        public static Models.Ticket CreateTicket(string employeeId, string title, string status, Models.Customer customer)
        {
            return new Models.Ticket(title, employeeId, status, customer);
        }

        public Models.Ticket EditTicket(string title, string employeeId, bool isClosed)
        {
            if (this.ticket == null)
            {
                throw new TicketManagerError("Ticket is null");
            }

            Models.Ticket ticket = this.ticket;
            ticket.Title = title;
            // Update the ticket's history with the employee id
            ticket.IsClosed = isClosed;

            return ticket;
        }

        public Models.Note CreateNote(Guid ticketId, string employeeId, string description, string status)
        {
            return new Models.Note(ticketId, employeeId, description, status);
        }

        public Models.Ticket DeleteTicket()
        {
            if (this.ticket == null)
            {
                throw new TicketManagerError("Ticket is null");
            }

            Models.Ticket ticket = this.ticket;
            ticket.IsClosed = true;

            return ticket;
        }
    }

    public class TicketManagerError : Exception
    {
        public TicketManagerError() : base() { }
        public TicketManagerError(string message) : base(message) { }
        public TicketManagerError(string message, Exception inner) : base(message, inner) { }
    }
}
