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
    public class ScheduledActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScheduledActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ScheduledActivities
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScheduledActivities.ToListAsync());
        }

        // GET: ScheduledActivities/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduledActivity = await _context.ScheduledActivities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scheduledActivity == null)
            {
                return NotFound();
            }

            return View(scheduledActivity);
        }

        // GET: ScheduledActivities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScheduledActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DueDate")] ScheduledActivity scheduledActivity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scheduledActivity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scheduledActivity);
        }

        // GET: ScheduledActivities/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduledActivity = await _context.ScheduledActivities.FindAsync(id);
            if (scheduledActivity == null)
            {
                return NotFound();
            }
            return View(scheduledActivity);
        }

        // POST: ScheduledActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,DueDate")] ScheduledActivity scheduledActivity)
        {
            if (id != scheduledActivity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scheduledActivity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduledActivityExists(scheduledActivity.Id))
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
            return View(scheduledActivity);
        }

        // GET: ScheduledActivities/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduledActivity = await _context.ScheduledActivities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scheduledActivity == null)
            {
                return NotFound();
            }

            return View(scheduledActivity);
        }

        // POST: ScheduledActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var scheduledActivity = await _context.ScheduledActivities.FindAsync(id);
            _context.ScheduledActivities.Remove(scheduledActivity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduledActivityExists(long id)
        {
            return _context.ScheduledActivities.Any(e => e.Id == id);
        }
    }
}
