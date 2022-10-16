using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuacosTracker3.Models;
using GuacosTracker3.Data;
using GuacosTracker3.Models.ViewModels;
using GuacosTracker3.SharedData;

namespace GuacosTracker3.Controllers
{
    public class CustomerController : Controller
    {
        private readonly TrackerDbContext _context;

        public CustomerController(TrackerDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
              return _context.Customers != null ? 
                          View(await _context.Customers.ToListAsync()) :
                          Problem("Entity set 'TrackerDbContext.Customers'  is null.");
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer _customers = await _context.Customers.SingleOrDefaultAsync(m => m.Id == id);

            if (_customers == null)
            {
                return NotFound();
            }

            TicketViewModel _ticketViewModel = new TicketViewModel();

            _ticketViewModel.Customer = _customers;

            List<Ticket> tickets = await _context.Ticket.Where(c => c.Customer == _customers.Id).ToListAsync();

            _ticketViewModel.Tickets = tickets;

            return View(_ticketViewModel);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FName,LName,Address,Phone,AltPhone,Email")] Customer customers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customers);
        }

        public async Task<IActionResult> CreateTicket(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Customer _customers = await _context.Customers.SingleOrDefaultAsync(m => m.Id == id);

            if (_customers == null)
            {
                return RedirectToAction("Index");
            }

            TicketViewModel _ticketViewModel = new TicketViewModel();

            _ticketViewModel.Customer = _customers;
            //_ticketViewModel.StatusItemList = ProgressList.GetStatusList();

            List<Ticket> tickets = new List<Ticket>();

            return View(_ticketViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTicket([Bind("Ticket, Customer, Tickets")] TicketViewModel _ticketViewModel)
        {
            if (ModelState.IsValid)
            {
                Ticket ticket = new Ticket();

                ticket.Title = _ticketViewModel.Ticket.Title;
                ticket.EmployeeId = _ticketViewModel.Ticket.EmployeeId; //Find how to grab through Auth
                ticket.Description = _ticketViewModel.Ticket.Description;
                ticket.Status = _ticketViewModel.Ticket.Status;
                ticket.Priority = _ticketViewModel.Ticket.Priority;
                ticket.Customer = _ticketViewModel.Ticket.Customer;

                _context.Ticket.Add(ticket);
                await _context.SaveChangesAsync();

                return RedirectToAction(controllerName: "Ticket", actionName: "Index");

            } else
            {
                return RedirectToAction("Index");
            }

        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }
            return View(customers);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FName,LName,Address,Phone,AltPhone,Email")] Customer customers)
        {
            if (id != customers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomersExists(customers.Id))
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
            return View(customers);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customers == null)
            {
                return NotFound();
            }

            return View(customers);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'TrackerDbContext.Customers'  is null.");
            }
            var customers = await _context.Customers.FindAsync(id);
            if (customers != null)
            {
                _context.Customers.Remove(customers);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomersExists(int id)
        {
          return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
