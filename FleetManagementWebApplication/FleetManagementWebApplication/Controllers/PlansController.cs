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
    public class PlansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private int Id;
        private string Name = " Account ";
        private int CompanyId;
        private string CompanyName = " Company ";
        private readonly NotificationManager NotificationManager;
        public PlansController(ApplicationDbContext context)
        {
            _context = context;
            NotificationManager = new NotificationManager();
        }

        // GET: Plans
        public async Task<IActionResult> Index()
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] = "Vehicles";
            return View(await _context.Plan.Include(p=>p.Vehicles).Include(p=>p.Activities).ToListAsync());
        }

        // GET: Plans/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            ViewData["QueryPlaceHolder"] = "Vehicles";
            if (id == null)
            {
                return NotFound();
            }
            Plan plan=null;
            try
            {
                 plan = _context.Plan.Where(m => m.Id == id).Include(p => p.Vehicles).
                                                        Include(p => p.Activities).Single<Plan>();
            }catch(Exception)
            {
                return NotFound();
            }

            return View(plan);
        }

        // GET: Plans/Create
        public IActionResult Create()
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
            return View();
        }
        public IActionResult AddPlanToVehicles(string PlanName,string[] ActivityType,int[] ActivityPeriod)
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            Plan plan = new Plan();
            plan.Name = PlanName;
            _context.Add(plan);
            _context.SaveChanges();
            plan.Activities = new List<Activity>();
            for(int i = 0; i < ActivityType.Length; i++)
            {
                if (ActivityType[i] == null || ActivityPeriod[i] == 0)
                    continue;
                Activity A = new Activity();
                A.Type = ActivityType[i];
                A.Period = ActivityPeriod[i];
                
                plan.Activities.Add(A);
                _context.Add(A);
            }
            _context.SaveChanges();
            HttpContext.Session.SetInt32("PlanId", (int)plan.Id);
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");       
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["type"] = HttpContext.Session.GetString("OrderType");
            ViewData["QueryPlaceHolder"] = "Vehicles";
           ViewData["Vehicles"]= _context.Vehicles.Where(v => v.Company.Id == CompanyId).ToArray<Vehicle>();
            return View();
        }
        public IActionResult AddVehiclesToPlan(long id) 
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            HttpContext.Session.SetInt32("PlanId", (int)id);
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["type"] = HttpContext.Session.GetString("OrderType");
            ViewData["QueryPlaceHolder"] = "Vehicles";
            ViewData["Vehicles"] = _context.Vehicles.Where(v => v.Company.Id == CompanyId && v.Plan.Id!=id).ToArray<Vehicle>();
            return View("/Views/Plans/AddPlanToVehicles.cshtml");
        }
        public IActionResult SearchAddPlanToVehicles(string Query1)
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["type"] = HttpContext.Session.GetString("OrderType");
            ViewData["QueryPlaceHolder"] = "Vehicles";
            IQueryable<Vehicle> infoQuery = _context.Vehicles.Where(v => v.Company.Id == CompanyId);
            if (Query1!= null)
            {
                string[] query = Query1.Split(" ");
                infoQuery = infoQuery.Where(v => v.Model == query[0]
                                                                        ||v.Make == query[0] ||
                                                                            v.LicensePlate == query[0]);
                if (query.Length > 1)
                    infoQuery.Where(v =>v.Model == query[1]
                                                                         || v.Make == query[1] ||
                                                                         v.LicensePlate == query[1]);
                if (query.Length > 2)
                    infoQuery.Where(v =>v.Model == query[2]
                                                                         || v.Make == query[2] ||
                                                                         v.LicensePlate == query[2]);


            }
            ViewData["Vehicles"] =infoQuery.ToArray<Vehicle>();
            return View("/Views/Plans/AddPlanToVehicles.cshtml");
        }

        public async Task<IActionResult> FinishAddPlanToVehicles(long [] SelectedVehicles)
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            int N = SelectedVehicles.Length;
            Vehicle[] vehicles = new Vehicle[N];       
            long PlanId = (long)HttpContext.Session.GetInt32("PlanId");
            Plan plan = _context.Plan.Find(PlanId);
            DateTime dateTime = DateTime.Now;
            Activity[] activities =_context.Activities.Where(a=>a.Plan.Id==PlanId).ToArray<Activity>();
            int M = activities.Length;
            ScheduledActivity[] scheduledActivities = new ScheduledActivity[N * M];
          
            for (int i = 0; i < N; i++)
            {
                vehicles[i] = await _context.Vehicles.FindAsync(SelectedVehicles[i]);
                vehicles[i].Plan =plan;
                _context.ScheduledActivities.RemoveRange(_context.ScheduledActivities.Where(s => s.Vehicle == vehicles[i]));
               
                for (int j = 0; j <M; j++)
                {  
                    scheduledActivities[i*M+j] = new ScheduledActivity();
                    scheduledActivities[i * M + j].Activity = activities[j];
                    scheduledActivities[i * M + j].Vehicle = vehicles[i];
                    scheduledActivities[i * M + j].CompanyId = CompanyId;
                    scheduledActivities[i * M + j].DueDate = dateTime.AddDays(activities[j].Period);
                  }

               
            }
            _context.UpdateRange(vehicles);
            _context.AddRange(scheduledActivities);
            await _context.SaveChangesAsync();
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] = "Vehicles";
            return RedirectToAction("Index") ;
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Plan plan)
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
            if (ModelState.IsValid)
            {
                _context.Add(plan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        
        // GET: Plans/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["QueryPlaceHolder"] = "Vehicles";
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // POST: Plans/Delete/5
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            var plan = await _context.Plan.FindAsync(id);
            ScheduledActivity[] SA = _context.ScheduledActivities.Where(s => s.Activity.Plan == plan).ToArray();
            Activity[]  A = _context.Activities.Where(a => a.Plan == plan).ToArray();
            Vehicle[] V = _context.Vehicles.Where(v => v.Plan == plan).ToArray();
            foreach(Vehicle v in V)
                v.Plan = null;
            _context.Vehicles.UpdateRange(V);
            _context.ScheduledActivities.RemoveRange(SA);
            _context.Activities.RemoveRange(A);
            _context.Plan.Remove(plan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanExists(long id)
        {
            return _context.Plan.Any(e => e.Id == id);
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
