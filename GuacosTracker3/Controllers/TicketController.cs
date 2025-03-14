using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuacosTracker3.Models;
using GuacosTracker3.Data;
using GuacosTracker3.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using GuacosTracker3.SharedData;
using System.Collections.Generic;
using static GuacosTracker3.Utilities.Pagination;
using System.Drawing.Printing;

namespace GuacosTracker3.Controllers
{
    [Authorize(Roles = "Employee, Manager, Admin")]
    public class TicketController : Controller
    {
        private readonly TrackerDbContext _context;
        private string _title = "Tickets";
        private string _subtitle = "";

        [ViewData]
        public string Page
        {
            get
            {
                if (_subtitle != "")
                {
                    string title = string.Format("{0} - {1}", _title, _subtitle);
                    return title;
                }
                return _title;
            }

            set { _title = value; }
        }

        public string Subtitle
        {
            get { return _subtitle; }
            set { _subtitle = value; }
        }

        public TicketController(TrackerDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(int? pageNum)
        {
            if (_context.Ticket == null)
            {
                Problem("Entity set 'TrackerDbContext.Ticket'  is null.");
                return View();
            }

            List<TicketViewModel> tickets = await _context.Ticket
                .Where(t => !t.IsClosed)
                .Join(_context.Customers, ticket => ticket.CustomerID,
                    customer => customer.Id,
                    (ticket, customer) => new TicketViewModel(ticket, customer.FName, customer.LName))
                .ToListAsync();

            int pageSize = 15;

            if (tickets == null || tickets.Count == 0)
            {
                return View(PaginatedList<TicketViewModel>.CreatePagination(new List<TicketViewModel>(), pageNum ?? 1, pageSize));
            }

            List<TicketViewModel> groupedTickets = tickets
                .OrderBy(t => ProgressList.GetStatusOrderDict[t.Ticket.CurrentStatus])
                //.ThenByDescending(t => t.Ticket.Priority)
                .ThenBy(t => t.Ticket.RecentChange)
                .ToList();

            return View(PaginatedList<TicketViewModel>.CreatePagination(groupedTickets, pageNum ?? 1, pageSize));
        }

        // GET: Tickets/Details/5
        [HttpGet]
        [Route("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            Subtitle = "Details";

            Ticket _ticket = await _context.Ticket.SingleOrDefaultAsync(t => t.Id == id);

            if (_ticket == null)
            {
                return RedirectToAction("Index");
            }

            Customer _customers = await _context.Customers.SingleOrDefaultAsync(m => m.Id == _ticket.CustomerID);

            if (_customers == null)
            {
                return RedirectToAction("Index");
            }

            List<Note> _notes = await _context.Notes.Where(c => c.TicketID == _ticket.Id).ToListAsync();

            TicketNoteCustomerViewModel _ticketNotesViewModel = new()
            {
                Ticket = _ticket,
                Customer = _customers,
                Note = new Note(),
                Notes = _notes,
                RecentNote = _notes.LastOrDefault() ?? null
            };

            return View(_ticketNotesViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(Guid id, [Bind("Ticket, Customer, Note, RecentNote")] TicketNoteCustomerViewModel _ticketViewModel)
        {
            Ticket _ticket = await _context.Ticket.SingleOrDefaultAsync(t => t.Id == id);
            if (_ticket == null)
            {
                return RedirectToAction("Index");
            }

            Customer _customers = await _context.Customers.SingleOrDefaultAsync(m => m.Id == _ticket.CustomerID);
            if (_customers == null)
            {
                return RedirectToAction("Index");
            }

            _ticketViewModel.Ticket = _ticket;
            _ticketViewModel.Customer = _customers;

            ModelState.Remove("Ticket");
            ModelState.Remove("Customer");
            ModelState.Remove("Notes");

            if (ModelState.IsValid)
            {
                Note _new_note = new()
                {
                    TicketID = _ticket.Id,
                    EmployeeID = _ticketViewModel.Note.EmployeeID,

                    Description = _ticketViewModel.Note.Description,
                    Status = _ticketViewModel.Note.Status,

                    Date = DateTime.Now
                };

                _context.Add(_new_note);
                await _context.SaveChangesAsync();

                _ticket.CurrentStatus = _ticketViewModel.Note.Status;
                _ticket.RecentChange = _new_note.Date;

                _context.Update(_ticket);
                await _context.SaveChangesAsync();

                List<Note> _notes = await _context.Notes.Where(c => c.TicketID == id).ToListAsync();
                Note _recent_note = _new_note;

                TicketNoteCustomerViewModel _newTicketViewModel = new()
                {
                    Ticket = _ticket,
                    Customer = _customers,
                    Note = new Note(),
                    Notes = _notes,
                    RecentNote = _recent_note
                };

                return View(_newTicketViewModel);
            }

            return RedirectToAction("Details", id);
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,EmployeeId,Description,Status,Priority")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Id = Guid.NewGuid();
                //ticket.Status = ProgressList.StatusString(Int32.Parse(ticket.Status));
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(actionName: "Index", controllerName: "Tickets");
            }

            return RedirectToActionPreserveMethod(actionName: "CreateTicket", controllerName: "Customers");
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,EmployeeId,Date,Description,Status,Priority,Customer,Hidden")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ticket.CurrentStatus = ticket.CurrentStatus;
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Ticket== null)
            {
                return Problem("Entity set 'TrackerDbContext.Ticket'  is null.");
            }

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket != null)
            {
                _context.Ticket.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
            return (_context.Ticket?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
