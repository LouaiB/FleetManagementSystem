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
    public class BillsController : FleetController
    {
       

        public BillsController(ApplicationDbContext context):base(context)
        {
          
        }

   
        public async Task<IActionResult> Index()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
          
            return View(await _context.Bills.Include(b=>b.Vehicle).ToListAsync());
        }
        public async Task<IActionResult> Search(SearchModel S) 
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
         
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
            if (!LogedIn())
                return RedirectToRoute("Home");
           
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
            if (!LogedIn())
                return RedirectToRoute("Home");
            CreateBillModel model = new CreateBillModel();
           model.FillVehiclesAndServices (_context,CompanyId); 
            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBillModel model)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");

            if (ModelState.IsValid)
            {
                Vehicle v = _context.Vehicles.Find(model.SelectedVehicle);
                Service s = _context.Service.Find(model.SelectedService);
                Bill bill = new Bill
                {
                    Service = s.Name,
                    Vehicle = v,
                    DateTime = model.DateTime,
                    Provider = model.Provider,
                    Cost = model.Cost
                };
               
                _context.Add(bill);
                await _context.SaveChangesAsync();                
                return RedirectToAction(nameof(Index));
            }
            model.FillVehiclesAndServices(_context, CompanyId);
            return View(model);
        }

        // GET: Bills/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            
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

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Service,DateTime,Cost,Provider")] Bill bill)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
           
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
            if (!LogedIn())
                return RedirectToRoute("Home");
            
            
            
            
            
            
           
            
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
            if (!LogedIn())
                return RedirectToRoute("Home");
            
            
            
            
            
           
            
            var bill = await _context.Bills.FindAsync(id);
            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillExists(long id)
        { 
            return _context.Bills.Any(e => e.Id == id);
        }
       
    }
}
