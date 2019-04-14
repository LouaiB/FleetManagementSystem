﻿using System;
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

        public PlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Plans
        public async Task<IActionResult> Index()
        {
            return View(await _context.Plan.ToListAsync());
        }

        // GET: Plans/Details/5
        public async Task<IActionResult> Details(long? id)
        {
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

        // GET: Plans/Create
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult AddPlanToVehicles(string PlanName,string[] ActivityType,int[] ActivityPeriod)
        {
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
            CompanyId = (int)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["type"] = HttpContext.Session.GetString("OrderType");
            ViewData["QueryPlaceHolder"] = "Vehicles";
           ViewData["Vehicles"]= _context.Vehicles.Where(v => v.Company.Id == CompanyId).ToArray<Vehicle>();
            return View();
        }

        public async Task<IActionResult> FinishAddPlanToVehicles(long [] SelectedVehicles)
        {
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
                for(int j = 0; j <M; j++)
                {
                    scheduledActivities[i*M+j] = new ScheduledActivity();
                    scheduledActivities[i * M + j].Activity = activities[j];
                    scheduledActivities[i * M + j].Vehicle = vehicles[i];
                    scheduledActivities[i * M + j].DueDate = dateTime.AddDays(activities[j].Period);
                  }

               
            }
            _context.UpdateRange(vehicles);
            _context.AddRange(scheduledActivities);
            await _context.SaveChangesAsync();
            
            return View();
        }
        // POST: Plans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Plan plan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        // GET: Plans/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plan.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }
            return View(plan);
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name")] Plan plan)
        {
            if (id != plan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanExists(plan.Id))
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
            return View(plan);
        }

        // GET: Plans/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var plan = await _context.Plan.FindAsync(id);
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
