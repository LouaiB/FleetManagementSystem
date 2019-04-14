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
    public class ServiceLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ServiceLogs
        public async Task<IActionResult> Index()
        {
            return View(await _context.ServiceLogs.ToListAsync());
        }

        // GET: ServiceLogs/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceLog = await _context.ServiceLogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceLog == null)
            {
                return NotFound();
            }

            return View(serviceLog);
        }

        // GET: ServiceLogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Provider,Cost")] ServiceLog serviceLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceLog);
        }

        // GET: ServiceLogs/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceLog = await _context.ServiceLogs.FindAsync(id);
            if (serviceLog == null)
            {
                return NotFound();
            }
            return View(serviceLog);
        }

        // POST: ServiceLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Date,Provider,Cost")] ServiceLog serviceLog)
        {
            if (id != serviceLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceLogExists(serviceLog.Id))
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
            return View(serviceLog);
        }

        // GET: ServiceLogs/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceLog = await _context.ServiceLogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceLog == null)
            {
                return NotFound();
            }

            return View(serviceLog);
        }

        // POST: ServiceLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var serviceLog = await _context.ServiceLogs.FindAsync(id);
            _context.ServiceLogs.Remove(serviceLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceLogExists(long id)
        {
            return _context.ServiceLogs.Any(e => e.Id == id);
        }
    }
}
