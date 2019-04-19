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
    public class BillsController : Controller
    {
        private int Id;
        private string Name = " Account ";
        private long CompanyId;
        private string CompanyName = " Company ";
        private readonly NotificationManager NotificationManager;
       

        private readonly ApplicationDbContext _context;

        public BillsController(ApplicationDbContext context)
        {
            NotificationManager = new NotificationManager();         
                _context = context;
        }

   
        public async Task<IActionResult> Index()
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] = "Vehicles";
            return View(await _context.Bills.Include(b=>b.Vehicle).ToListAsync());
        }
        public async Task<IActionResult> Search(SearchModel S) 
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            ViewData["Name"] = Name;
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] = "Vehicles";
           IQueryable<Bill> infoQuery =_context.Bills.Include(b => b.Vehicle);
            ViewData["test"] = S.StartDate + " " + S.EndDate + " ; " + S.Service;
            if (S.StartDate != null)
                infoQuery = infoQuery.Where(b => b.DateTime >= S.StartDate);
            if (S.EndDate != null)
                infoQuery = infoQuery.Where(b => b.DateTime <= S.EndDate);
            if (S.Service != null)
                infoQuery = infoQuery.Where(b => b.Service == S.Service);
            if (S.Provider != null)
                infoQuery = infoQuery.Where(b => b.Provider== S.Provider);
            if (S.Vehicle != null) {
                string[] query = S.Vehicle.Split(" ");
                infoQuery =infoQuery.Where (b=>b.Vehicle.Model == query[0] 
                                                                        || b.Vehicle.Make == query[0] ||
                                                                         b.Vehicle.LicensePlate == query[0]);
                if (query.Length > 1)
                    infoQuery.Where(b => b.Vehicle.Model == query[1]
                                                                         || b.Vehicle.Make == query[1] ||
                                                                          b.Vehicle.LicensePlate == query[1]);
                if (query.Length > 2)
                    infoQuery.Where(b => b.Vehicle.Model == query[2]
                                                                         || b.Vehicle.Make == query[2] ||
                                                                          b.Vehicle.LicensePlate == query[2]);


            }
            ViewData["start"] = S.StartDate.ToShortDateString();
            ViewData["end"] = S.EndDate.ToShortDateString();
            ViewData["vehicle"] = S.Vehicle;
            ViewData["service"] = S.Service;
            ViewData["provider"] = S.Provider;
            return View("/Views/Bills/Index.cshtml",await infoQuery.ToListAsync());
        }
        // GET: Bills/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            ViewData["Name"] = Name;
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] = "Vehicles";
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        // GET: Bills/Create
        public IActionResult Create()
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            ViewData["Name"] = Name;
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] = "Vehicles";
            Vehicle[] V = _context.Vehicles.Where(v => v.Company.Id == CompanyId).ToArray();
            string[] B = _context.Bills.Select(e => e.Service).Distinct().ToArray();
            SelectListItem[] X = new SelectListItem[V.Length];
            for(int i=0;i< V.Length;i++) 
            X[i]=new SelectListItem { Value =""+V[i].Id, Text = V[i].Make+" "+ V[i].Model+" "+ V[i].LicensePlate};
            ViewData["Vehicles"] = X;
            ViewData["Services"] = B;
            return View();
        }

        // POST: Bills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Service,DateTime,Cost,Provider")] Bill bill,long VehicleSelect)
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            ViewData["Name"] = Name;
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] = "Vehicles";
            Vehicle[] V = _context.Vehicles.Where(v => v.Company.Id == CompanyId).ToArray();
            string[] B = _context.Bills.Select(e => e.Service).Distinct().ToArray();
            SelectListItem[] X = new SelectListItem[V.Length];
            for (int i = 0; i < V.Length; i++)
                X[i] = new SelectListItem { Value = "" + V[i].Id, Text = V[i].Make + " " + V[i].Model + " " + V[i].LicensePlate };
            ViewData["Vehicles"] = X;
            ViewData["Services"] = B;
            if (ModelState.IsValid)
            {
                Vehicle v = _context.Vehicles.Find(VehicleSelect);
                bill.Vehicle = v;
                _context.Add(bill);
                await _context.SaveChangesAsync();                
                return RedirectToAction(nameof(Index));
            }
            return View(bill);
        }

        // GET: Bills/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            ViewData["Name"] = Name;
            ViewData["QueryPlaceHolder"] = "Vehicles";
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills.FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }
            return View(bill);
        }

        // POST: Bills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Service,DateTime,Cost,Provider")] Bill bill)
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            ViewData["Name"] = Name;
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] = "Vehicles";
            if (id != bill.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillExists(bill.Id))
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
            return View(bill);
        }

        // GET: Bills/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            ViewData["Name"] = Name;
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] = "Vehicles";
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        // POST: Bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] = "Vehicles";
            var bill = await _context.Bills.FindAsync(id);
            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillExists(long id)
        { 
            return _context.Bills.Any(e => e.Id == id);
        }
        private bool isLogedIn()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == null)
                return false;
            else
                return true;

        }
    }
}
