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

    public class VehiclesDetailsController :FleetController
    {
           public VehiclesDetailsController(ApplicationDbContext context)
            :base(context)
           {     
           }



      
        public async Task<IActionResult> Index(int id = 0)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
   
            if (id == 0)
            {
                return NotFound();
            }
            Vehicle vehicle = null;
            try
            {
               vehicle  = _context.Vehicles.Where(m => m.Id == id)
                    .Include(m => m.Plan).Include(d=>d.CurrentDriver).Single<Vehicle>();
            }catch(Exception)
            {
                return NotFound();
            }
            ViewData["Id"] = id;
            ViewData["one"] = " v-btn-selected ";
            ViewData["plans"] = getPlans();
            ViewData["drivers"] = getDrivers();
            return View(vehicle);
        }


        public async Task<IActionResult> ScheduledServices(int id = 0)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
           
            HttpContext.Session.SetInt32("VehicleId", id);
                       ScheduledActivity[] scheduledActivities = _context.ScheduledActivities
             .Where(s => s.Vehicle.Id == id).Include(s => s.Activity).ThenInclude(s=>s.Service).OrderBy(s => s.DueDate).ToArray<ScheduledActivity>();

            ViewData["Id"] = id;
            ViewData["two"] = " v-btn-selected ";
            return View(scheduledActivities);
        }


        public async Task<IActionResult> Check(int [] Checked )
        {
            if (!LogedIn())
                return RedirectToRoute("Home");         
            int VehicleId = (int)HttpContext.Session.GetInt32("VehicleId");
            foreach (int ServiceId in Checked)
            {
                ScheduledActivity S = _context.ScheduledActivities.Where(s=>s.Id==ServiceId )
                                                            .Include(s=>s.Activity).ThenInclude(s=>s.Service).Single();
                S.DueDate = DateTime.Now.AddDays(S.Activity.Period);
                _context.Update(S);
            }
            _context.SaveChanges();
            ScheduledActivity[] scheduledActivities = _context.ScheduledActivities
             .Where(s => s.Vehicle.Id == VehicleId).Include(s => s.Activity).ThenInclude(s => s.Service).OrderBy(s => s.DueDate).ToArray<ScheduledActivity>();
            ViewData["Id"] = VehicleId;
            return View("/Views/VehiclesDetails/ScheduledServices.cshtml", scheduledActivities);
        }


        public async Task<IActionResult> Costs(int id = 0)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");     
            Bill[] bills = _context.Bills.Where(b=> b.Vehicle.Id == id).ToArray<Bill>();
            ViewData["Id"] = id;
            ViewData["three"] = " v-btn-selected ";
           ViewData["total"]=""+ _context.Bills.Where(b=>b.Vehicle.Id==id).Sum(b => b.Cost);
            return View(bills);
        }
        public async Task<IActionResult> FuelLogs(int id = 0)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            FuelLog[] logs = _context.FuelLogs.Where(b => b.Vehicle.Id == id).ToArray();
            ViewData["Id"] = id;
            ViewData["five"] = " v-btn-selected ";
            ViewData["total"] = "" + _context.FuelLogs.Where(b => b.Vehicle.Id == id).Sum(b => b.Quantity*b.PricePerLitre);
            return View(logs);
        }


        public async Task<IActionResult> Deliveries(int id = 0)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
           
            ViewData["Id"] = id;
            ViewData["four"] = " v-btn-selected ";
           Delivery[] deliveries = _context.Deliveries.Where(d=> d.Vehicle.Id == id && d.Finished==true).Include(d=>d.Driver).ToArray<Delivery>();
            return View(deliveries);
        }

        public async Task<IActionResult> UpdateStatus(long id,string status)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            Vehicle V = _context.Vehicles.Include(v => v.CurrentDriver).Include(v => v.Plan).Where(d => d.Id == id).First();
            if (status == "active")
                V.isCurrentlyActive = true;
            else
                V.isCurrentlyActive = false;
            _context.Update(V);
            _context.SaveChanges();
            ViewData["Id"] = id;
            ViewData["one"] = " v-btn-selected ";
            ViewData["plans"] = getPlans();
            ViewData["drivers"] = getDrivers();
            return View("~/Views/VehiclesDetails/Index.cshtml", V);
        }

        public async Task<IActionResult> UpdatePlan(long id, long plan=0)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            Vehicle V = _context.Vehicles.Include(v => v.CurrentDriver).Include(v => v.Plan).Where(d => d.Id == id).First();
            if (plan > 0)
            {
                Plan p = _context.Plan.Find(plan);
                V.Plan = p;
                _context.Update(V);
                _context.SaveChanges();
            }
            ViewData["Id"] = id;
            ViewData["one"] = " v-btn-selected ";
            ViewData["plans"] = getPlans();
            ViewData["drivers"] = getDrivers();
            return View("~/Views/VehiclesDetails/Index.cshtml", V);
        }

        public async Task<IActionResult> UpdateDriver(long id, long driver=0)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            Vehicle V = _context.Vehicles.Include(v => v.CurrentDriver).Include(v => v.Plan).Where(d => d.Id == id).First();
            if (driver > 0)
            {
                Driver p = _context.Drivers.Find(driver);
                V.CurrentDriver = p;
                _context.Update(V);
                _context.SaveChanges();
            }

            ViewData["Id"] = id;
            ViewData["one"] = " v-btn-selected ";
            ViewData["plans"] = getPlans();
            ViewData["drivers"] = getDrivers();

            return View("~/Views/VehiclesDetails/Index.cshtml", V);
        }

        public SelectListItem[] getPlans()
        {
            Plan[] plans = _context.Plan.Where(p => p.Company.Id == CompanyId).ToArray();
            SelectListItem[] items = new SelectListItem[plans.Length];
            for(int i = 0; i < plans.Length; i++)
            {
                items[i] = new SelectListItem() { Value = ""+plans[i].Id, Text = plans[i].Name };
            }
            return items;
        }

        public SelectListItem[] getDrivers()
        {
            Driver[] drivers = _context.Drivers.Where(p => p.Company.Id == CompanyId).ToArray();
            SelectListItem[] items = new SelectListItem[drivers.Length];
            for (int i = 0; i < drivers.Length; i++)
            {
                items[i] = new SelectListItem() { Value = "" + drivers[i].Id, Text = drivers[i].Name };
            }
            return items;
        }

    }
}

 

       
