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
    public class PlansController : FleetController
    {
        

        public PlansController(ApplicationDbContext context):base(context)
        {
        }

        // GET: Plans
        public async Task<IActionResult> Index()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
          

            return View(await _context.Plan
                .Include(p=>p.Vehicles)
                .Include(p=>p.Activities)
                .ToListAsync());
        }

       
        public async Task<IActionResult> Details(long? id)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
             
            
            if (id == null)
            {
                return NotFound();
            }
            Plan plan=null;
            try
            {
                 plan = _context.Plan.Where(m => m.Id == id).Include(p => p.Vehicles).
                                                        Include(p => p.Activities).ThenInclude(p=>p.Service).Single<Plan>();
            }catch(Exception)
            {
                return NotFound();
            }

            return View(plan);
        }

        // GET: Plans/Create
        public IActionResult Create()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            CreatePlanModel model = new CreatePlanModel();
            model.FillServices(_context, CompanyId);
            return View(model);
        }
        public IActionResult AddPlanToVehicles(CreatePlanModel model)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            Plan plan = new Plan();
            plan.Name = model.Title;
            plan.Company = _context.Companies.Find(CompanyId);
            _context.Add(plan);
            _context.SaveChanges();
            plan.Activities = new List<Activity>();
            for(int i = 0; i < model.SelectedServices.Length; i++)
            {
                if (model.SelectedServices[i] == 0 || model.ActivityPeriod[i] == 0)
                    continue;
                Activity A = new Activity();
                A.Service = _context.Service.Find(model.SelectedServices[i]);
                A.Period = model.ActivityPeriod[i];
                
                plan.Activities.Add(A);
                _context.Add(A);
            }
            _context.SaveChanges();
            HttpContext.Session.SetInt32("PlanId", (int)plan.Id);
            ViewData["type"] = HttpContext.Session.GetString("OrderType");
            
           ViewData["Vehicles"]= _context.Vehicles.Where(v => v.Company.Id == CompanyId).ToArray<Vehicle>();
            return View();
        }
        public IActionResult AddVehiclesToPlan(long id) 
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
                   
            HttpContext.Session.SetInt32("PlanId", (int)id);         
            ViewData["type"] = HttpContext.Session.GetString("OrderType");
            
            ViewData["Vehicles"] = _context.Vehicles.Where(v => v.Company.Id == CompanyId && v.Plan.Id!=id).ToArray<Vehicle>();
            return View("/Views/Plans/AddPlanToVehicles.cshtml");
        }
        public IActionResult SearchAddPlanToVehicles(string Query1)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
           ViewData["type"] = HttpContext.Session.GetString("OrderType");
            
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
            if (!LogedIn())
                return RedirectToRoute("Home");
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
            
           
            return RedirectToAction("Index") ;
        }
        


        
        // GET: Plans/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
       
            
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
            if (!LogedIn())
                return RedirectToRoute("Home");
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
        
    }
}
