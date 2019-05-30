using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FleetManagementWebApplication.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace FleetManagementWebApplication.Controllers
{
    public class VehiclesController : FleetController
    {
        IHostingEnvironment _environment;
        public  VehiclesController(ApplicationDbContext context, IHostingEnvironment environment)
            :base(context)
        {
            _environment = environment;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");

            VehiclesIndexModel model = new VehiclesIndexModel();
            model.FillSelectListDrivers(_context,CompanyId);
            model.FillVehicles(_context, CompanyId);
            return View(model);
   
        }

        public async Task<IActionResult> Search(string Model="" ,string Make="", string LicensePlate="" ,long SelectDriverId=0,int Status=0)
        { 
            
            if (!LogedIn())                                           
                return RedirectToRoute("Home");
            VehiclesIndexModel model = new VehiclesIndexModel
            {
                Model=Model,
                Make=Make,
                LicensePlate=LicensePlate,
                SelectDriverId=SelectDriverId,
                Status=Status
            };
            var infoQuery = _context.Vehicles.Include(v=>v.CurrentDriver).Where(v => v.Company.Id == CompanyId);

            if (model.LicensePlate != null)
            {
                infoQuery = infoQuery.Where(v => v.LicensePlate == model.LicensePlate);
                return View("/Views/Vehicles/Index.cshtml", infoQuery.ToList<Vehicle>());
            }
               
            if (model.Make != null)
                infoQuery = infoQuery.Where(v => v.Make == model.Make);
            if (model.Model != null)
                infoQuery = infoQuery.Where(v => v.Model == model.Model);
            if (model.SelectDriverId!=0)
                infoQuery = infoQuery.Where(v => v.CurrentDriver.Id == model.SelectDriverId);
            if (model.Status != 0)
            {
                if (model.Status == 1)
                    infoQuery = infoQuery.Where(v =>v.isCurrentlyActive);
                else
                    if (model.Status == 2)
                        infoQuery = infoQuery.Where(v => v.isOnTheRoad);
                    else
                        infoQuery = infoQuery.Where(v => !v.isCurrentlyActive);
            }
          
            model.Vehicles = infoQuery.ToArray();
            model.FillSelectListDrivers(_context, CompanyId);
            return View("/Views/Vehicles/Index.cshtml",model);
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
        public async Task<IActionResult> Create([Bind("Id,LicensePlate,Make,Model,purchaseDate,Odometer,PayLoad,EmissionsCO2,FuelConsumption,fuelType,FuelLevel,CurrentLoad,Icon")] Vehicle vehicle,IFormFile file)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            var filePath = Path.GetTempFileName();

            string image = "";

            using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\images\\" + file.FileName))
            {
                image = file.FileName;
                file.CopyTo(filestream);
                filestream.Flush();
            }



            ViewData["type"] = HttpContext.Session.GetString("OrderType");
            
            if (ModelState.IsValid)
            {
               
                vehicle.Company= _context.Companies.FirstOrDefault(c => c.Id == CompanyId);
                vehicle.Image = image;
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
        public async Task<IActionResult> Edit([Bind("Id,LicensePlate,Make,Model,purchaseDate,Odometer,PayLoad,EmissionsCO2," +
                                                                              "FuelConsumption")] Vehicle vehicle,IFormFile file)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            Vehicle v = _context.Vehicles.Find(vehicle.Id);
            string image =v.Image;

            if (file != null)
            {
                var filePath = Path.GetTempFileName();

                using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\images\\" + file.FileName))
                {
                    image = file.FileName;
                    file.CopyTo(filestream);
                    filestream.Flush();
                }
            }
            if (!VehicleExists(vehicle.Id))
            {
                return NotFound();
            }

       
            v.LicensePlate = vehicle.LicensePlate;
            v.Make = vehicle.Make;
            v.Model = vehicle.Model;
            v.purchaseDate = vehicle.purchaseDate;
            v.Odometer = vehicle.Odometer;
            v.PayLoad = vehicle.PayLoad;
            v.FuelConsumption = vehicle.FuelConsumption;
            v.EmissionsCO2 = vehicle.EmissionsCO2;
            v.Image = image;
           
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(v);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
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
