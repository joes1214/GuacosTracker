using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuacosTracker3.Models;
using GuacosTracker3.Data;
using GuacosTracker3.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using GuacosTracker3.SharedData;
using System.Collections.Generic;

namespace GuacosTracker3.Controllers
{
    [Authorize(Roles = "Employee, Manager, Admin")]
    public class TicketController : Controller
    {
        private readonly TrackerDbContext _context;

        public TicketController(TrackerDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(bool ShowHidden = false)
        {
            if (_context.Ticket == null)
            {
                Problem("Entity set 'TrackerDbContext.Ticket'  is null.");
                return View();
            }

            List<Ticket> visibleItems = _context.Ticket.Where(x => x.Hidden != true).ToList();

            //Maybe combine Awaiting Repair and Customer?
            Dictionary<string, List<Ticket>> groupedItems = visibleItems
                .GroupBy(ticket => ticket.RecentStatus)
                .ToLookup(group => group.Key, group => group.OrderBy(ticket => ticket.RecentChange).ToList()!)
                .ToDictionary(lookup => lookup.Key, lookup => lookup.First()!);

            List<Ticket> filteredItems = ProgressList.GetStatusOrder
                .SelectMany(status => groupedItems.ContainsKey(status) ? groupedItems[status] : new List<Ticket>())
                .ToList();

            List<TicketViewModel> _ticketList = new();

            TicketViewModel _ticketViewModel = new();
            Customer _customer = new();

            foreach (Ticket item in filteredItems)
            {
                _ticketViewModel.Ticket = item;
                _ticketViewModel.Customer = await _context.Customers.FindAsync(item.Customer);
                _ticketList.Add(_ticketViewModel);
                _ticketViewModel = new TicketViewModel();
            }

            return View(_ticketList);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Ticket _ticket = await _context.Ticket.SingleOrDefaultAsync(t => t.Id == id);

            if (_ticket == null)
            {
                return RedirectToAction("Index");
            }

            Customer _customers = await _context.Customers.SingleOrDefaultAsync(m => m.Id == _ticket.Customer);

            if (_customers == null)
            {
                return RedirectToAction("Index");
            }

            List<Note> _notes = await _context.Notes.Where(c => c.TicketId == _ticket.Id).ToListAsync();

            TicketNoteCustomerViewModel _ticketNotesViewModel = new TicketNoteCustomerViewModel();

            _ticketNotesViewModel.Ticket = _ticket;
            _ticketNotesViewModel.Customer = _customers;
            _ticketNotesViewModel.Note = new Note();
            _ticketNotesViewModel.Notes = _notes;

            return View(_ticketNotesViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(Guid? id, [Bind("Ticket, Customer, Note")] TicketNoteCustomerViewModel _ticketViewModel)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Ticket _ticket = await _context.Ticket.SingleOrDefaultAsync(t => t.Id == id);
            if (_ticket == null)
            {
                return RedirectToAction("Index");
            }

            Customer _customers = await _context.Customers.SingleOrDefaultAsync(m => m.Id == _ticket.Customer);
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
                Note _note = new Note();
                _note.TicketId = _ticket.Id;
                _note.EmployeeId = _ticketViewModel.Note.EmployeeId;

                _note.Description = _ticketViewModel.Note.Description;
                _note.Status = _ticketViewModel.Note.Status;

                _note.Date = DateTime.Now;

                _context.Add(_note);
                await _context.SaveChangesAsync();

                _ticket.RecentStatus = _ticketViewModel.Note.Status;
                _ticket.RecentChange = _note.Date;

                _context.Update(_ticket);
                await _context.SaveChangesAsync();
            }

            List<Note> _notes = await _context.Notes.Where(c => c.TicketId == id).ToListAsync();

            TicketNoteCustomerViewModel _newTicketViewModel = new TicketNoteCustomerViewModel();

            _newTicketViewModel.Ticket = _ticket;
            _newTicketViewModel.Customer = _customers;
            _newTicketViewModel.Note = new Note();
            _newTicketViewModel.Notes = _notes;

            return View(_newTicketViewModel);
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
                    ticket.RecentStatus = ticket.Status;
                    //ticket.Status = ProgressList.StatusString(Int32.Parse(ticket.Status));
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
