using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FleetApi1.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FleetApi1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DeliverySummariesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeliverySummariesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public  JsonResult StartDeliverySummary(StartDeliveryModel Start)
        {
            string error = "";
            Delivery delivery;
            try
            {
                delivery = _context.Deliveries.Where(d => d.Id == Start.DeliveryId)
                                                                           .Include(d => d.Vehicle).First();
            }
            catch (Exception)
            {
                error = "Delivery Not Found";
                return new JsonResult(error);
            }

            if (delivery == null)
            {
                error = "Delivery Not Found";
                return new JsonResult(error);
            }
            delivery.Started = true;
            try
            {
                using (var webClient = new WebClient())
                {
                    string url = "https://matrix.route.api.here.com/routing/7.2/calculatematrix.json"
                                       + "?app_id=ORWs1MBbnXAyzlgdPGpw"
                                        + "&app_code=ftEQwIdOxSdxiRv6pd1Rvw";
                    url += "&start0" + "=" + delivery.SourceLatitude + "," + delivery.SourceLongtitude;
                    url += "&destination0=" + delivery.DestinationLatitude + "," + delivery.DestinationLongtitude;
                    url += "&summaryAttributes=distance,traveltime&mode=fastest;car;traffic:disabled";
                    var rawJSON = webClient.DownloadStringTaskAsync(url).Result;
                    JObject rss = JObject.Parse(rawJSON);
                    delivery.OptimalDistance = (float)rss["response"]["matrixEntry"][0]["summary"]["distance"] / 1000;
                    delivery.OptimalTime = (int)rss["response"]["matrixEntry"][0]["summary"]["travelTime"] / 60;
                }

            }
            catch (Exception)
            {
              //  error = "Optimal Distance Error";
                //return new JsonResult(error);

            }
           
            delivery.OptimalFuelConsumption = delivery.OptimalDistance * delivery.Vehicle.FuelConsumption;


            DeliverySummary summary = new DeliverySummary()
            {
                Delivery = delivery,
                StartTime = Start.StartTime,
                EndTime = Start.StartTime,
                StartFuelLevel = Start.StartFuelLevel,
                EndFuelLevel = Start.StartFuelLevel,
                StartOdometer = Start.StartOdometer,
                EndOdometer = Start.StartOdometer,
                HardCorneringRate = 5,
                HarshAccelerationAndDeceleration=5,
                HarshBreakingsRate=5,
                FuelConsumptionRate=5,
                OnTimeDeliveryRate=5,
                Idling=5,
                OverRevving=5,
                SpeedingsRate=5,
                SeatBeltRate=5
            };
            Vehicle V = delivery.Vehicle;

            if (V != null)
            {
                V.Latitude = Start.Latitude;
                V.Longtitude = Start.Longtitude;
                V.isCurrentlyActive = true;
            }
            else
            {
                error = "Vehicle Error";
                return new JsonResult(error);
            }
            _context.Deliveries.Update(delivery);
            _context.DeliverySummaries.Add(summary);
            _context.Vehicles.Update(V);
             _context.SaveChanges();

            long DeliverySummaryId = summary.Id;
            return Json(new {DeliverySummaryId });
        }

        [HttpPost]
        public async Task<ActionResult<Result1>> UpdateDeliveryInfo(VehicleLog info)
        {
            try
            {
                DeliverySummary summary = _context.DeliverySummaries
                                                                .Where(s=>s.Id==info.DeliverySummaryId)
                                                                .Include(s=>s.Delivery)
                                                                .ThenInclude(d=>d.Vehicle)
                                                                .First();

                Vehicle V = summary.Delivery.Vehicle;
                if (V != null)
                {
                    V.Latitude = info.Latitude;
                    V.Longtitude = info.Longtitude;
                }

                if (info.HardCornering)
                {
                    summary.HardCorneringRate--;
                    if (summary.HardCorneringRate < 0) summary.HardCorneringRate = 0;
                }
                if (info.HarshAccelerationAndDeceleration)
                {
                    summary.HarshAccelerationAndDeceleration--;
                    if (summary.HarshAccelerationAndDeceleration < 0) summary.HarshAccelerationAndDeceleration = 0;
                }
                if (info.HarshBreaking)
                {
                    summary.HarshBreakingsRate--;
                    if (summary.HarshBreakingsRate < 0) summary.HarshBreakingsRate = 0;
                }
                if (info.OverRevving)
                {
                    summary.OverRevving--;
                    if (summary.OverRevving < 0) summary.OverRevving = 0;
                }
                if (info.Speeding)
                {
                    summary.SpeedingsRate--;
                    if (summary.SpeedingsRate < 0) summary.SpeedingsRate = 0;
                }
                if (info.SeatBelt)
                {
                    summary.SeatBeltRate--;
                    if (summary.SeatBeltRate < 0) summary.SeatBeltRate = 0;
                }
                if (info.EngineRunning && info.Odometer == summary.EndOdometer)
                {
                    summary.Idling--;
                    if (summary.Idling < 0) summary.Idling = 0;
                }
                summary.EndOdometer = info.Odometer;

                _context.DeliverySummaries.Update(summary);
                _context.Vehicles.Update(V);
                await _context.SaveChangesAsync();
                return new Result1(true);

            }
            catch (Exception)
            {
                return new Result1(false);
            } 
        }

        [HttpPost]
        public async Task<ActionResult<Result2>> FinishDeliverySummary(FinishDeliveryModel finish)
        {
            try
            {
                DeliverySummary summary = _context.DeliverySummaries
                                                                       .Where(d => d.Id == finish.DeliverySummaryId).Include(d => d.Delivery).Single();
                summary.Delivery.Finished = true;
                Driver driver = _context.Deliveries.Where(d => d.Id == summary.Delivery.Id)
                                                                               .Include(d => d.Driver).First().Driver;
                Company company = _context.Drivers.Where(d => d.Id == driver.Id).Include(d => d.Company).First().Company;

                summary.EndFuelLevel = finish.EndFuelLevel;
                summary.EndOdometer = finish.EndOdometer;
                summary.EndTime = finish.EndTime;



                summary.FuelConsumptionRate = (summary.Delivery.OptimalFuelConsumption /
                                                                            (summary.StartFuelLevel - summary.EndFuelLevel)) * 5;
                summary.OnTimeDeliveryRate = (summary.Delivery.OptimalTime /
                                                                            (summary.EndTime - summary.StartTime).Minutes) * 5;
                float DistanceRate = (summary.Delivery.OptimalDistance /
                                                     (summary.EndOdometer - summary.StartOdometer)) * 5;
                //Remove later
                summary.FuelConsumptionRate = 5;
                summary.OnTimeDeliveryRate = 5;
                DistanceRate = 5;
                //

                summary.PerformanceScore = (summary.OverRevving
                                                          + summary.HardCorneringRate
                                                          + summary.HarshAccelerationAndDeceleration
                                                          + summary.HarshBreakingsRate
                                                          + summary.Idling
                                                          + summary.FuelConsumptionRate) / 6;

                summary.ComplianceScore = (summary.OnTimeDeliveryRate
                                                            + summary.SpeedingsRate
                                                            + DistanceRate) / 3;

                summary.SafetyScore = (summary.SeatBeltRate
                                                   + summary.SpeedingsRate
                                                   + summary.HarshBreakingsRate
                                                   + summary.HarshAccelerationAndDeceleration) / 4;

                float DeliveryScore = (summary.PerformanceScore + summary.ComplianceScore + summary.SafetyScore) / 3;

                driver.Score = (_context.DeliverySummaries.Where(d => d.Delivery.Driver == driver)
                                                                    .Average(d => d.PerformanceScore)
                                            + _context.DeliverySummaries.Where(d => d.Delivery.Driver == driver)
                                                                    .Average(d => d.ComplianceScore)
                                            + _context.DeliverySummaries.Where(d => d.Delivery.Driver == driver)
                                                                    .Average(d => d.SafetyScore)) / 3;


                _context.DeliverySummaries.Update(summary);
                //_context.Deliveries.Update(summary.Delivery);
                _context.Drivers.Update(driver);
                _context.SaveChanges();

                Driver[] drivers = _context.Drivers.Where(d => d.Company == company)
                                                 .OrderByDescending(d => d.Score).ToArray();
                for (int i = 0; i < drivers.Length; i++)
                {
                    drivers[i].Rank = i + 1;
                }
                _context.UpdateRange(drivers);
                _context.SaveChanges();


                await _context.SaveChangesAsync();
                return new Result2()
                {
                    DeliveryScore = DeliveryScore,
                    OverallScore = driver.Score,
                    Rank = driver.Rank,
                    PerformanceScore = summary.PerformanceScore,
                    ComplianceScore = summary.ComplianceScore,
                    SafetyScore = summary.SafetyScore,
                    NbOfDrivers = drivers.Length
                };

            }
            catch (Exception)
            {
                return new Result2()
                {
                    DeliveryScore = 0,
                    OverallScore = 0,
                    Rank =0,
                    PerformanceScore = 0,
                    ComplianceScore = 0,
                    SafetyScore = 0,
                    NbOfDrivers =0
                };
            }


        }

        public class Result
        {
            public long DeliverySummaryId { get; set; }
            public Result(long x) { DeliverySummaryId = x; }
        }
        public class Result1
        {
            public bool Success { get; set; }
            public Result1(bool x) { Success = x; }
        }
        public class Result2
        {
            public float PerformanceScore { get; set; }
            public float ComplianceScore { get; set; }
            public float SafetyScore { get; set; }

            public float DeliveryScore { get; set; }
            public float OverallScore { get; set; } 
            public int Rank { get; set; }
            public int NbOfDrivers { get; set; }

            
        }
    }
}
