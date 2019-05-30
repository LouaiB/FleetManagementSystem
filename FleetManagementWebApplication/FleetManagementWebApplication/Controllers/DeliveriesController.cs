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
    public class DeliveriesController : FleetController
    {
        

        public DeliveriesController(ApplicationDbContext context):base(context)
        {
            
        }

        // GET: Deliveries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Deliveries.ToListAsync());
        }
        public async Task<IActionResult> DriversDeliveries(long Id)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");

            List<SelectListItem> Drivers = new List<SelectListItem>();
            Driver[] drivers = _context.Drivers.Where(d => d.Company.Id == CompanyId).ToArray();
            foreach (Driver d in drivers)
            {
                if (d.Id == Id)
               Drivers.Add(new SelectListItem { Value = "" + d.Id, Text = d.Name + ", username: " + d.Username, Selected =true });
                else
               Drivers.Add(new SelectListItem { Value = "" + d.Id, Text =d.Name+", username: "+d.Username });
            }
            ViewData["drivers"] = Drivers;
           
            return View(await _context.Deliveries.Where(d=>d.Driver.Id==Id).Include(d=>d.Client).Include(d=>d.Vehicle).ToListAsync());
        }


        public async Task<IActionResult> ClientsDeliveries(long Id)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");

            List<SelectListItem> Clients = new List<SelectListItem>();
            Client[] clients = _context.Deliveries.Where(d => d.Company.Id == CompanyId).Select(d => d.Client)
                                                         .Union(_context.Clients.Where(c => c.Company.Id == CompanyId)).ToArray();
            foreach (Client c in clients)
            {
                if (c.Id == Id)
                   Clients.Add(new SelectListItem { Value = "" + c.Id, Text = c.Name + ", username: " + c.Username, Selected = true });
                else
                   Clients.Add(new SelectListItem { Value = "" + c.Id, Text = c.Name + ", username: " +c.Username });
            }
            ViewData["clients"] = Clients;

            return View(await _context.Deliveries.Where(d => d.Client.Id == Id).Include(d => d.Driver).Include(d => d.Vehicle).ToListAsync());
        }




        // POST: Deliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var delivery = await _context.Deliveries.FindAsync(id);
            _context.Deliveries.Remove(delivery);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryExists(long id)
        {
            return _context.Deliveries.Any(e => e.Id == id);
        }
    }
}
