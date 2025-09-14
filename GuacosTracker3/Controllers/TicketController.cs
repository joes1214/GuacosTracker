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
        private readonly IConfiguration _configuration;

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

        public TicketController(TrackerDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int? pageNum)
        {
            if (_context.Ticket == null)
            {
                Problem("Entity set 'TrackerDbContext.Ticket'  is null.");
                return View();
            }

            var tickets = await _context.Ticket
                .Include(t => t.Customer)
                .OrderByDescending(t => t.RecentChange)
                .Select(t => new TicketIndexViewModel
                {
                    Id = t.Id,
                    Status = t.CurrentStatus,
                    CustomerName = t.Customer != null ? t.Customer.GetFullName() : "N/A",
                    Title = t.Title,
                    RecentChange = t.RecentChange,
                })
                .ToListAsync();

            int pageSize = 15;

            if (tickets == null || tickets.Count == 0)
            {
                return View(PaginatedList<TicketIndexViewModel>.CreatePagination(new List<TicketIndexViewModel>(), pageNum ?? 1, pageSize));
            }

            var groupedTickets = tickets
                .OrderBy(t => ProgressList.GetStatusOrderDict[t.Status])
                //.ThenByDescending(t => t.Ticket.Priority)
                //.ThenBy(t => t.RecentChange)
                .ToList();

            return View(PaginatedList<TicketIndexViewModel>.CreatePagination(groupedTickets, pageNum ?? 1, pageSize));
        }

        // GET: Tickets/Details/5
        [HttpGet]
        [Route("[controller]/[action]/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            Subtitle = "Details";

            Ticket _ticket = await _context.Ticket.Include(t => t.Customer).SingleOrDefaultAsync(t => t.Id == id);

            if (_ticket == null)
            {
                return NotFound("Ticket not found");
            }

            List<Note> _notes = await _context.Notes.Where(c => c.TicketID == _ticket.Id).ToListAsync();

            TicketNoteCustomerViewModel _ticketNotesViewModel = new()
            {
                Ticket = _ticket,
                Customer = _ticket.Customer,
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
            Ticket _ticket = await _context.Ticket.Include(t => t.Customer).SingleOrDefaultAsync(t => t.Id == id);
            if (_ticket == null)
            {
                return RedirectToAction("Index");
            }

            _ticketViewModel.Ticket = _ticket;
            _ticketViewModel.Customer = _ticket.Customer;

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
                    Customer = _ticket.Customer,
                    Note = new Note(),
                    Notes = _notes,
                    RecentNote = _recent_note
                };

                return View(_newTicketViewModel);
            }

            return RedirectToAction("Details", id);
        }

        [HttpGet]
        public async Task<IActionResult> Create([FromQuery(Name = "customerID")] int? customerID)
        {
            Subtitle = "Create";

            TicketCreateViewModel _createTicket = new();
            if (customerID.HasValue)
            {
                Customer _customer = await _context.Customers.SingleOrDefaultAsync(m => m.Id == customerID);

                if (_customer != null)
                {
                    _createTicket.CustomerID = customerID;
                    _createTicket.CustomerName = _customer.GetFullName();
                }

            }

            return View(_createTicket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketCreateViewModel _ticketCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                Subtitle = $"Create Ticket - {_ticketCreateViewModel.CustomerName}"; // fix later
                return View(_ticketCreateViewModel);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                Customer customer = await _context.Customers.SingleOrDefaultAsync(c => c.Id == _ticketCreateViewModel.CustomerID);
                if (customer == null)
                {
                    return View(_ticketCreateViewModel);
                }

                Ticket ticket = new(
                    _ticketCreateViewModel.Title,
                    "test",
                    _ticketCreateViewModel.Status,
                    customer
                );
                _context.Ticket.Add(ticket);
                await _context.SaveChangesAsync();

                Note note = new(
                    ticket.Id, 
                    ticket.EmployeeID, 
                    _ticketCreateViewModel.Description, 
                    ticket.CurrentStatus
                );
                _context.Notes.Add(note);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return RedirectToAction("Details", "Ticket", new { id = ticket.Id });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred when creating Ticket.");
            }
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            Subtitle = "Edit";

            var ticket = await _context.Ticket.Include(t => t.Customer).SingleOrDefaultAsync(t => t.Id == id);
            if (ticket == null) return NotFound("Ticket not found.");

            EditTicketViewModel editTicket = new()
            {
                Id = ticket.Id,
                Title = ticket.Title,
                IsPriority = ticket.Priority,
                IsClosed = ticket.IsClosed,
                DateCreated = ticket.DateCreated,
                Customer = ticket.Customer.GetFullName(),
            };
            return View(editTicket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditTicketViewModel editTicket)
        {
            if (!ModelState.IsValid) return View(editTicket);
            
            var ticket = await _context.Ticket.SingleOrDefaultAsync(t => t.Id == editTicket.Id);
            if (ticket == null) return NotFound("Ticket not found.");

            ticket.Title = editTicket.Title;
            ticket.Priority = editTicket.IsPriority;
            ticket.IsClosed = editTicket.IsClosed;
            ticket.RecentChange = DateTime.Now;

            Note note = new()
            {
                TicketID = ticket.Id,
                EmployeeID = ticket.EmployeeID,//update to use current users ID
                Description = "Title of ticket was edited.",
                Status = ticket.CurrentStatus,
                Date = ticket.RecentChange
            };

            _context.Add(note);
            _context.SaveChanges();
            return RedirectToAction("Details", new {id = ticket.Id});
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
