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
    public class ServicesController : FleetController
    {
       

        public ServicesController(ApplicationDbContext context):base(context)
        {         
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            if (!LogedIn())
            {
                return RedirectToRoute("Home");
            }

            return View(await _context.Service.ToListAsync());
        }






        public async Task<IActionResult> Create(string Name,string Description)
        {
            if (!LogedIn())
            {
                return RedirectToRoute("Home");
            }
          Service service = new Service
         {
           Name=Name,
            Description=Description
             };
            service.Company = _context.Companies.Find(CompanyId);        
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
          
           
        }

        
        // POST: Services/Delete/5
        [HttpGet, ActionName("Delete")]      
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var service = await _context.Service.FindAsync(id);
            _context.Service.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

      
    }
}
