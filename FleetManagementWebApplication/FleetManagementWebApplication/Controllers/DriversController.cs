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
    public class DriversController : Controller
    {
        private int Id;
        private string Name = " Account ";
        private long CompanyId;
        private string CompanyName = " Company ";
        private readonly ApplicationDbContext _context;

        public DriversController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Drivers
        public async Task<IActionResult> Index()
        {
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] ="Drivers" ;
            return View(await _context.Drivers.Where(d=>d.Company.Id==CompanyId).ToListAsync());
        }
        public async Task<IActionResult> Search(string Query="")
        {
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] = "Drivers";

            if (Query == null)
                return View("/Views/Drivers/Index.cshtml", _context.Drivers.Where(v => v.Company.Id == CompanyId).ToList<Driver>());
            string[] query = Query.Split(" ");

            var infoQuery = (
                          from v in _context.Drivers
                          where v.Company.Id == CompanyId && (v.Username==query[0] || v.Name == Query)
                          select v);
            ViewData["Query"] = Query;

            return View("/Views/Drivers/Index.cshtml",infoQuery.ToList<Driver>());
        }

        // GET: Drivers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
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
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] = "Drivers";
            return View();
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,Name,Birthdate,Address,Phonenumber")] Driver driver)
        {
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] = "Drivers";
            if (ModelState.IsValid)
            {
                driver.Company = _context.Companies.Find(CompanyId);
                    _context.Add(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(driver);
        }

        // GET: Drivers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
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

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(long id, [Bind("Id,Username,Password,Name,Birthdate,Address,Phonenumber")] Driver driver)
        {
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] = "Drivers";

            if (id != driver.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driver);
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
            return View("/Views/Drivers/Edit.cshtml",driver);
        }

        // GET: Drivers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
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
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] = "Drivers";

            var driver = await _context.Drivers.FindAsync(id);
            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverExists(long id)
        {
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            return _context.Drivers.Any(e => e.Id == id);
        }
    }
}
