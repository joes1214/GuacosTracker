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
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using static GuacosTracker3.Utilities.Pagination;

namespace GuacosTracker3.Controllers
{
    [Authorize(Roles = "Employee, Manager, Admin")]
    public class CustomerController : Controller
    {
        private readonly TrackerDbContext _context;
        private string _title = "Customers";
        private string _subtitle = "";

        [ViewData]
        public string Page
        {
            get {
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

        public CustomerController(TrackerDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(int? pageNum)
        {
            List<Customer> customerList = await _context.Customers.ToListAsync();

            int pageSize = 15;

            return View(PaginatedList<Customer>.CreatePagination(customerList, pageNum ?? 1, pageSize));
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id, int? pageNum)
        {
            Subtitle = "Details";
            if (id == null)
            {
                return NotFound();
            }

            Customer _customer = await _context.Customers.SingleOrDefaultAsync(m => m.Id == id);

            if (_customer == null)
            {
                return NotFound();
            }

            List<Ticket> tickets = await _context.Ticket.Where(c => c.Customer.Id == _customer.Id).OrderByDescending(f => f.RecentChange).ToListAsync();

            int pageSize = 10;
            CustomerTicketDetails CustomerDetails = new()
            {
                Customer = _customer,
                Tickets = PaginatedList<Ticket>.CreatePagination(tickets, pageNum ?? 1, pageSize),
            };

            return View(CustomerDetails);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            Subtitle = "Create";
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

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Subtitle = "Edit";
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

        [Authorize(Roles = "Manager, Admin")]
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

        [HttpGet("[controller]/Customer", Name="Get_Customer")]
        public JsonResult Get([FromQuery(Name= "lname")] string? lname, [FromQuery(Name = "fname")] string? fname)
        {
            lname ??= string.Empty;
            fname ??= string.Empty;            

            var customers = _context.Customers
                .Where(c => c.LName.Contains(lname))
                .Where(c => c.FName.Contains(fname))
                .Select(c => new
                {
                    c.Id,
                    c.FName,
                    c.LName,
                    c.Phone,
                    c.Email,
                })
                .Take(10)
                .ToList();

            return Json(customers);
        }
    }
}
