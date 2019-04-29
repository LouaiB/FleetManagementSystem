using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManagementWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagementWebApplication.Controllers
{
    public class ReportsController : FleetController
    {
        
        public  ReportsController(ApplicationDbContext context)
      :base(context)
        {  
        }

        public IActionResult DriversRanks()
        {
            if(!LogedIn())
                return RedirectToRoute("Home");

            Driver[] drivers = _context.Drivers.Where(d=>d.Company.Id==CompanyId)
                                        .OrderBy(d=>d.Rank).ToArray();
            return View(drivers);
        }

        public IActionResult PerformanceRanks()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
          
            Driver[] drivers = _context.Drivers.Where(d => d.Company.Id == CompanyId)
                                        .OrderBy(d => d.Rank).ToArray();
            return View(drivers);
        }

     
     

    }
}