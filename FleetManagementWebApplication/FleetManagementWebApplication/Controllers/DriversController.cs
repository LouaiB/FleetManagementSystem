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
    public class DriversController : FleetController
    {
        IHostingEnvironment _environment;
        public DriversController(ApplicationDbContext context, IHostingEnvironment environment) : base(context)
        {
            _environment = environment;
        }
      
     
      
    // GET: Drivers
    public async Task<IActionResult> Index()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");

            ViewData["QueryPlaceHolder"] = "Drivers";
            return View(await _context.Drivers.Where(d => d.Company.Id == CompanyId).ToListAsync());
        }



        public async Task<IActionResult> Search(string Query = "")
        {
            if (!LogedIn())
                return RedirectToRoute("Home");

            ViewData["QueryPlaceHolder"] = "Drivers";

            if (Query == null)
                return View("/Views/Drivers/Index.cshtml", _context.Drivers.Where(v => v.Company.Id == CompanyId).ToList<Driver>());
            string[] query = Query.Split(" ");

            var infoQuery = (
                          from v in _context.Drivers
                          where v.Company.Id == CompanyId && (v.Username == query[0] || v.Name == Query)
                          select v);
            ViewData["Query"] = Query;

            return View("/Views/Drivers/Index.cshtml", infoQuery.ToList<Driver>());
        }

        // GET: Drivers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
             ViewData["QueryPlaceHolder"] = "Drivers";
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // GET: Drivers/Create
        public IActionResult Create()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
        ViewData["QueryPlaceHolder"] = "Drivers";
            return View();
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,Name,Birthdate,Address,Phonenumber")] Driver driver,IFormFile file )
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            var filePath = Path.GetTempFileName();

            string image = "";
           
            using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\images\\" + file.FileName))
                    {
                      image= file.FileName;
                
                file.CopyTo(filestream);
                filestream.Flush();
            }
            

          
            if (ModelState.IsValid)

            {
                if ( _context.Drivers.Any(e => e.Username == driver.Username))
                {
                    ViewData["UsernameError"]="Username Already Taken";
                    return View(driver);
                }
        
            driver.Company = _context.Companies.Find(CompanyId);
                int N = _context.Drivers.Where(d => d.Company ==driver.Company).ToArray().Length;
                driver.Rank = N+1;
                driver.Image = image;
                _context.Add(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(driver);
        }

        // GET: Drivers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
        ViewData["QueryPlaceHolder"] = "Drivers";

            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            return View(driver);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed([Bind("Id,Username,Password,Name,Birthdate,Address,Phonenumber")] Driver driver,IFormFile file )
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            Driver d = _context.Drivers.Find(driver.Id);
            string image =d.Image;

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
            

      
            d.Name = driver.Name;
            d.Address = driver.Address;
            d.Birthdate = driver.Birthdate;
            d.Phonenumber = driver.Phonenumber;
            d.Image = image;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(d);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(driver.Id))
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
            return View("/Views/Drivers/Edit.cshtml", driver);
        }

        // GET: Drivers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
           ViewData["QueryPlaceHolder"] = "Drivers";

            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            ViewData["QueryPlaceHolder"] = "Drivers";
            Driver driver =_context.Drivers.Where(d => d.Id == id).Include(d => d.Company).First();

            DeliverySummary[] SummaryDeliveries = _context.DeliverySummaries.Where(d => d.Delivery.Driver.Id == id).ToArray();
            Delivery[] deliveries = _context.Deliveries.Where(d => d.Driver.Id == id).ToArray();
            Vehicle [] Vehicles = _context.Vehicles.Where(d => d.CurrentDriver.Id == id).ToArray();
            foreach (var v in Vehicles)
                v.CurrentDriver = null;
            
            _context.RemoveRange(SummaryDeliveries);
            _context.RemoveRange(deliveries);
            _context.UpdateRange(Vehicles);
            
            _context.Drivers.Remove(driver);
            _context.SaveChanges();
            Driver[] drivers = _context.Drivers.Where(d => d.Company == driver.Company && d.Rank>driver.Rank).ToArray();
            foreach(Driver d in drivers)
            {
                d.Rank--;
            }
            _context.UpdateRange(drivers);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverExists(long id)
        {
            return _context.Drivers.Any(e => e.Id == id);
        }

    }
}
