using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuacosTracker3.Models;
using GuacosTracker3.Data;
using GuacosTracker3.SharedData;
using GuacosTracker3.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Index()
        {
            if (_context.Ticket == null)
            {
                Problem("Entity set 'TrackerDbContext.Ticket'  is null.");
            }

            List<TicketViewModel> _ticketList = new List<TicketViewModel>();

            TicketViewModel _ticketViewModel = new TicketViewModel();
            Customer _customer = new Customer();

            List<Ticket> tickets = await _context.Ticket.ToListAsync();

            foreach (Ticket item in tickets)
            {
                _ticketViewModel.Ticket = item;
                _ticketViewModel.Customer = await _context.Customers.FindAsync(item.Customer);
                _ticketList.Add(_ticketViewModel);
                _ticketViewModel = new TicketViewModel();
            }

            return View(_ticketList);


            //return _context.Ticket != null ? 
            //            View(await _context.Ticket.ToListAsync()) :
            //            Problem("Entity set 'TrackerDbContext.Ticket'  is null.");
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

        // GET: Tickets/Create
        //[HttpGet]
        //public IActionResult Create(int Id = 0)
        //{
        //    Customers _customer = _context.Customers.Find(Id);

        //    if (Id == 0 || _customer == null)
        //    {
        //        return RedirectToAction(actionName: "Index", controllerName: "Customers");
        //    }
        //    return View(TicketFactory.Create(Id, new Ticket()));
        //}


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
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,zTitle,EmployeeId,Date,Description,Status,Priority")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Ticket == null)
            {
                return Problem("Entity set 'TrackerDbContext.Ticket'  is null.");
            }
            var ticket = await _context.Ticket.FindAsync(id);
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
