using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FleetManagementWebApplication.Models;
using Microsoft.AspNetCore.Http;

namespace FleetManagementWebApplication.Controllers
{
    public class VehiclesController : FleetController
    {
      
        public  VehiclesController(ApplicationDbContext context)
            :base(context)
        {
         
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
          
            ViewData["type"] = HttpContext.Session.GetString("OrderType");
                       
            return View(await _context.Vehicles.Where(v => v.Company.Id == CompanyId).Include(v=>v.CurrentDriver).ToListAsync());
   
        }

        public async Task<IActionResult> Search(string Query="")
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
          
            ViewData["type"] = HttpContext.Session.GetString("OrderType");
            

            if (Query == null)
                return View("/Views/Vehicles/Index.cshtml",await _context.Vehicles.Where(v => v.Company.Id == CompanyId).ToListAsync());
            string[] query = Query.Split(" ");

            var infoQuery = (
                          from v in _context.Vehicles
                          where v.Company.Id == CompanyId && (v.Model == query[0] || v.Make == query[0] || v.LicensePlate == query[0])
                          select v);
            if (query.Length > 1)
                infoQuery = infoQuery.Intersect
                       (from v in _context.Vehicles
                        where v.Company.Id == CompanyId && (v.Model == query[1] || v.Make == query[1] || v.LicensePlate == query[1])
                        select v);
            if (query.Length > 2)
                infoQuery = infoQuery.Intersect
                       (from v in _context.Vehicles
                        where v.Company.Id == CompanyId && (v.Model == query[2] || v.Make == query[2] || v.LicensePlate == query[2])
                        select v);

                ViewData["Query"] = Query; 
            return View("/Views/Vehicles/Index.cshtml",infoQuery.ToList<Vehicle>());
        }

        [HttpPost]
        public async Task<IActionResult> Details(int id)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
               

            if (id == 0)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
           
            ViewData["type"]= _context.Companies.FirstOrDefault(c => c.Id == CompanyId).OrderType;
            
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LicensePlate,Make,Model,purchaseDate,Odometer,PayLoad,EmissionsCO2,FuelConsumption,fuelType,FuelLevel,CurrentLoad")] Vehicle vehicle)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
              
            ViewData["type"] = HttpContext.Session.GetString("OrderType");
            
            if (ModelState.IsValid)
            {
               
                vehicle.Company= _context.Companies.FirstOrDefault(c => c.Id == CompanyId);
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

   
        public async Task<IActionResult> GetEdit(long?  id)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
           
            
            ViewData["type"] = _context.Companies.FirstOrDefault(c => c.Id == CompanyId).OrderType;
            if (id == null)
            {
                return NotFound();
            }
     
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View("/Views/Vehicles/Edit.cshtml",vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,LicensePlate,Make,Model,purchaseDate,Odometer,PayLoad,EmissionsCO2,FuelConsumption,fuelType,FuelLevel,CurrentLoad")] Vehicle vehicle)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            
            ViewData["type"] = HttpContext.Session.GetString("OrderType");
            

            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
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
            return View(vehicle);
        }

        public async Task<IActionResult> GetDelete(long? id)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
           
            if (id == null)
            {
                return NotFound();
            }
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            
            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");  

            var vehicle = await _context.Vehicles.FindAsync(id);
            _context.ScheduledActivities.RemoveRange(_context.ScheduledActivities.Where(a => a.Vehicle == vehicle));
            _context.Bills.RemoveRange(_context.Bills.Where(a => a.Vehicle == vehicle));
            Delivery[] deliveries=_context.Deliveries.Where(d => d.Vehicle == vehicle).ToArray();
              long[] Ids= deliveries.Select(d => d.Id).ToArray();
            _context.DeliverySummaries
              .RemoveRange(_context.DeliverySummaries.Where(s => Ids.Contains(s.Delivery.Id)));
            _context.Deliveries.RemoveRange(deliveries);
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(long id)
        {
         
            return _context.Vehicles.Any(e => e.Id == id);
        }
     
    }
}
