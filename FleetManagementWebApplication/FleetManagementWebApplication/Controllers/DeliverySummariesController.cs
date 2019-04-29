using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FleetManagementWebApplication.Models;

namespace FleetManagementWebApplication.Controllers
{
    public class DeliverySummariesController : FleetController
    {
        

        public DeliverySummariesController(ApplicationDbContext context):base(context)
        {
            
        }

        // GET: DeliverySummaries
        public async Task<IActionResult> Index()
        {
            return View(await _context.DeliverySummaries.ToListAsync());
        }

        // GET: DeliverySummaries/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliverySummary = await _context.DeliverySummaries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deliverySummary == null)
            {
                return NotFound();
            }

            return View(deliverySummary);
        }

        // GET: DeliverySummaries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DeliverySummaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime,StartFuelLevel,EndFuelLevel,StartOdometer,EndOdometer,NumberOfSpeedings,NumberOfNoSeatbelts,NumberOfHarshbreaks")] DeliverySummary deliverySummary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deliverySummary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deliverySummary);
        }

        // GET: DeliverySummaries/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliverySummary = await _context.DeliverySummaries.FindAsync(id);
            if (deliverySummary == null)
            {
                return NotFound();
            }
            return View(deliverySummary);
        }

        // POST: DeliverySummaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,StartTime,EndTime,StartFuelLevel,EndFuelLevel,StartOdometer,EndOdometer,NumberOfSpeedings,NumberOfNoSeatbelts,NumberOfHarshbreaks")] DeliverySummary deliverySummary)
        {
            if (id != deliverySummary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliverySummary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliverySummaryExists(deliverySummary.Id))
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
            return View(deliverySummary);
        }

        // GET: DeliverySummaries/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliverySummary = await _context.DeliverySummaries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deliverySummary == null)
            {
                return NotFound();
            }

            return View(deliverySummary);
        }

        // POST: DeliverySummaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var deliverySummary = await _context.DeliverySummaries.FindAsync(id);
            _context.DeliverySummaries.Remove(deliverySummary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliverySummaryExists(long id)
        {
            return _context.DeliverySummaries.Any(e => e.Id == id);
        }
    }
}
