﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuacosTracker3.Models;
using GuacosTracker3.Data;
using GuacosTracker3.SharedData;

namespace GuacosTracker3.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TrackerDbContext _context;

        public TicketsController(TrackerDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
              return _context.Ticket != null ? 
                          View(await _context.Ticket.ToListAsync()) :
                          Problem("Entity set 'TrackerDbContext.Ticket'  is null.");
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        [HttpPost]
        [Route("Tickets/Create/{Id}")]
        /*
         * I have no idea why the routing for Tickets/Create/ will send you to Page Not Found. Maybe I'll figure it out?
         */
        public IActionResult Create(int? Id = 0)
        {
            if (Id == 0 || Id == null)
            {
                return Redirect("Customer/Index");
            }

            ViewBag.CustomerId = Id;
            return View();
        }


        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("CreateTicket")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTicket([Bind("Id,CustomerId,Title,EmployeeId,Date,Description,Status,Priority")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Id = Guid.NewGuid();
                ticket.Status = ProgressList.StatusString(Int32.Parse(ticket.Status));
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return Redirect("/Customers/Index");
        }

        // GET: Tickets/Edit/5
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
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CustomerId,Title,EmployeeId,Date,Description,Status,Priority")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ticket.Status = ProgressList.StatusString(Int32.Parse(ticket.Status));
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
