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

    public class VehiclesDetailsController : Controller
    {
        private int Id;
        private string Name = " Account ";
        private int CompanyId;
        private string CompanyName = " Company ";
        private readonly ApplicationDbContext _context;
        private readonly NotificationManager NotificationManager;

        public VehiclesDetailsController(ApplicationDbContext context)
        {
            _context = context;
            NotificationManager = new NotificationManager();
        }



        [HttpPost]
        public async Task<IActionResult> Index(int id = 0)
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["Id"] = id;
            ViewData["one"] = " v-btn-selected ";
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
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

            return View(vehicle);
        }
        public async Task<IActionResult> ScheduledServices(int id = 0)
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            HttpContext.Session.SetInt32("VehicleId", id);
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["Id"] = id;
            ViewData["two"] = " v-btn-selected ";
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            ScheduledActivity[] scheduledActivities = _context.ScheduledActivities
             .Where(s => s.Vehicle.Id == id).Include(s => s.Activity).OrderBy(s => s.DueDate).ToArray<ScheduledActivity>();
            return View(scheduledActivities);
        }
        public async Task<IActionResult> Check(int [] Checked )
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            int VehicleId = (int)HttpContext.Session.GetInt32("VehicleId");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["Id"] = VehicleId;
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            foreach(int ServiceId in Checked)
            {
                ScheduledActivity S = _context.ScheduledActivities.Where(s=>s.Id==ServiceId )
                                                            .Include(s=>s.Activity).Single();
                S.DueDate = DateTime.Now.AddDays(S.Activity.Period);
                _context.Update(S);
            }
            _context.SaveChanges();
            ScheduledActivity[] scheduledActivities = _context.ScheduledActivities
             .Where(s => s.Vehicle.Id == VehicleId).Include(s => s.Activity).OrderBy(s => s.DueDate).ToArray<ScheduledActivity>();
            return View("/Views/VehiclesDetails/ScheduledServices.cshtml", scheduledActivities);
        }
        public async Task<IActionResult> Costs(int id = 0)
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["Id"] = id;
            ViewData["three"] = " v-btn-selected ";
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            Bill[] bills = _context.Bills.Where(b=> b.Vehicle.Id == id).ToArray<Bill>();
           ViewData["total"]=""+ _context.Bills.Where(b=>b.Vehicle.Id==id).Sum(b => b.Cost);
            return View(bills);
        }
        public async Task<IActionResult> Deliveries(int id = 0)
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["Id"] = id;
            ViewData["four"] = " v-btn-selected ";
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            Delivery[] deliveries = _context.Deliveries.Where(d=> d.Vehicle.Id == id && d.Finished==true).Include(d=>d.Driver).ToArray<Delivery>();
            return View(deliveries);
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
 

       
