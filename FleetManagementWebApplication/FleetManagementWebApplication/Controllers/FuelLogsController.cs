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
    public class FuelLogsController : FleetController
    {
     

        public FuelLogsController(ApplicationDbContext context):base(context)
        {
        
        }

        // GET: FuelLogs
        public async Task<IActionResult> Index()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            Vehicle[] vehicles = _context.Vehicles.Where(v => v.Company.Id == CompanyId).ToArray();
            List<SelectListItem> Vehicles = new List<SelectListItem>();
            foreach(Vehicle v in vehicles)
            {
                Vehicles.Add(new SelectListItem { Value = "" + v.Id, Text = v.Make + " " + v.Model + " " + v.LicensePlate });
            }
            ViewData["vehicles"] = Vehicles;
            return View(await _context.FuelLogs.Include(f=>f.Vehicle).ToListAsync());
        }

        // GET: FuelLogs/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fuelLog = await _context.FuelLogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fuelLog == null)
            {
                return NotFound();
            }

            return View(fuelLog);
        }

      
        [HttpPost]
        
        public async Task<IActionResult> Create(DateTime DateTime,float Quantity,float PricePerLitre,string FuelType,string Provider,long Vehicle)
        {
        
            try
            {
                FuelLog log = new FuelLog
                {
                    DateTime = DateTime,
                    Quantity = Quantity,
                    PricePerLitre = PricePerLitre,
                    FuelType = FuelType,
                    Provider = Provider,
                    Vehicle = _context.Vehicles.Find(Vehicle)
                };
                _context.Add(log);
                await _context.SaveChangesAsync();
              
            }
            catch (Exception)
            {
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: FuelLogs/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fuelLog = await _context.FuelLogs.FindAsync(id);
            if (fuelLog == null)
            {
                return NotFound();
            }
            return View(fuelLog);
        }

        // POST: FuelLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,DateTime,Quantity,PricePerLitre,Provider")] FuelLog fuelLog)
        {
            if (id != fuelLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fuelLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuelLogExists(fuelLog.Id))
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
            return View(fuelLog);
        }

        // GET: FuelLogs/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fuelLog = await _context.FuelLogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fuelLog == null)
            {
                return NotFound();
            }

            return View(fuelLog);
        }

        // POST: FuelLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var fuelLog = await _context.FuelLogs.FindAsync(id);
            _context.FuelLogs.Remove(fuelLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FuelLogExists(long id)
        {
            return _context.FuelLogs.Any(e => e.Id == id);
        }
    }
}
