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



        [HttpPost]
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
               vehicle  = _context.Vehicles.Where(m => m.Id == id).Include(m => m.Plan).Single<Vehicle>();
            }catch(Exception)
            {
                return NotFound();
            }
            ViewData["Id"] = id;
            ViewData["one"] = " v-btn-selected ";
            return View(vehicle);
        }


        public async Task<IActionResult> ScheduledServices(int id = 0)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
           
            HttpContext.Session.SetInt32("VehicleId", id);
                       ScheduledActivity[] scheduledActivities = _context.ScheduledActivities
             .Where(s => s.Vehicle.Id == id).Include(s => s.Activity).OrderBy(s => s.DueDate).ToArray<ScheduledActivity>();

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
                                                            .Include(s=>s.Activity).Single();
                S.DueDate = DateTime.Now.AddDays(S.Activity.Period);
                _context.Update(S);
            }
            _context.SaveChanges();
            ScheduledActivity[] scheduledActivities = _context.ScheduledActivities
             .Where(s => s.Vehicle.Id == VehicleId).Include(s => s.Activity).OrderBy(s => s.DueDate).ToArray<ScheduledActivity>();
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


        public async Task<IActionResult> Deliveries(int id = 0)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
           
            ViewData["Id"] = id;
            ViewData["four"] = " v-btn-selected ";
           Delivery[] deliveries = _context.Deliveries.Where(d=> d.Vehicle.Id == id && d.Finished==true).Include(d=>d.Driver).ToArray<Delivery>();
            return View(deliveries);
        }
        

        }
    }

 

       
