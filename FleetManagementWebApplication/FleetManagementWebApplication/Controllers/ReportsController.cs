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

        public IActionResult DriversEvolution(DateTime d1,DateTime d2 )

        {

            if(!LogedIn())
                return RedirectToRoute("Home");
            if(d1 == DateTime.MinValue)
            {
                d1 = DateTime.Parse("2018/1/1");
            }

            if (d2 == DateTime.MinValue)
            {
                d2 = DateTime.Now;
            }

            var query = from summary in _context.DeliverySummaries
                        where (summary.StartTime >=d1 && summary.StartTime<=d2 
                                      && summary.Delivery.Company.Id==CompanyId)
                        group summary by summary.StartTime into g
                        select new
                        {
                            Date = g.Key,
                            AveragePerformance = g.Average(summary => summary.PerformanceScore),
                            AverageCompliance = g.Average(summary => summary.ComplianceScore),
                            AverageSafety = g.Average(summary => summary.SafetyScore)
                        };

            DateTime[] date = new DateTime[query.Count()];
            float[] performance = new float[date.Length];
            float[] compliance = new float[date.Length];
            float[] safety = new float[date.Length];
            int i = 0;
            foreach(var summary in query)
            {
                date[i] = summary.Date;
                performance[i] = summary.AveragePerformance;
                compliance[i] = summary.AverageCompliance;
                safety[i] = summary.AverageSafety;
                i++;
             }

            ViewData["Date"] = date;
            ViewData["Performance"] = performance;
            ViewData["Compliance"] = compliance;
            ViewData["Safety"] = safety;
            ViewData["d1"] = d1.ToString("yyyy-MM-dd");
            ViewData["d2"] = d2.ToString("yyyy-MM-dd"); ;


            return View();
        }

        public IActionResult DriversRanks()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
          
            Driver[] drivers = _context.Drivers.Where(d => d.Company.Id == CompanyId)
                                        .OrderBy(d => d.Rank).ToArray();
            return View(drivers);
        }




        public IActionResult Costs(DateTime d1, DateTime d2)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");

            if (d1 == DateTime.MinValue)
            {
                d1 = DateTime.Parse("2018/1/1");
            }

            if (d2 == DateTime.MinValue)
            {
                d2 = DateTime.Now;
            }

            var query = from bill in _context.Bills
                        where (bill.DateTime >= d1 && bill.DateTime <= d2 && bill.Vehicle.Company.Id == CompanyId)
                        group bill by bill.Service into g
                        select new
                        {
                            Service = g.Key,
                            Cost = g.Sum(bill =>bill.Cost)
                       };

            string[] services = new string [query.Count()];
            float[] costs = new float[services.Length];
            
            int i = 0;
            foreach (var b in query)
            {
                services[i] = b.Service;
                costs[i] = b.Cost;
               i++;
            }

            ViewData["services"] = services;
            ViewData["costs"] = costs;            
            ViewData["d1"] = d1.ToString("yyyy-MM-dd");
            ViewData["d2"] = d2.ToString("yyyy-MM-dd"); ;


            return View();
        }


        public IActionResult FuelConsumption(int year=2019)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");

            var query = from summary in _context.DeliverySummaries
                        join d in _context.Deliveries on summary.Delivery.Id equals d.Id
                        where d.Company.Id == CompanyId && summary.StartTime.Year==year
                        group new { summary, d } by   new { summary.StartTime.Month}  into g
                        select new
                        {
                            Month = g.Key,
                            ActualFuel = g.Sum(p => (p.summary.StartFuelLevel -p. summary.EndFuelLevel)),
                            OptimizedFuel= g.Sum(p=>p.d.OptimalFuelConsumption)
                       };

            var query1 = from summary in _context.DeliverySummaries
                         where summary.Delivery.Company.Id == CompanyId && summary.StartTime.Year == year
                         group  summary  by summary.StartTime.Month  into g
                        select new
                        {
                            Month = g.Key,
                            Score=g.Average(summary=>(summary.PerformanceScore+summary.SafetyScore+summary.ComplianceScore)/3)
                        };




            int[] months = new int[query.Count()];
           float[] optimFuel = new float[months.Length];
           float[] actFuel = new float[months.Length];
            float[] score = new float[months.Length];
            int i = 0;
            foreach(var s in query)
            {
                months[i] = s.Month.Month;
                optimFuel[i] = s.OptimizedFuel;
                actFuel[i] = s.ActualFuel;
                i++;
            }
            i = 0;
            foreach (var s in query1)
            {
                score[i] = s.Score;
                i++;
            }

            ViewData["months"] = months;
            ViewData["optimFuel"] = optimFuel;
            ViewData["actFuel"] = actFuel;
            ViewData["score"] = score;
           
            return View();
        }



    }
}