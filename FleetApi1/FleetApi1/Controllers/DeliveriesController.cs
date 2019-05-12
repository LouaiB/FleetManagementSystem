﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FleetApi1.Models;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleetApi1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        

        public DeliveriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public Vehicle getNearestVehicle (double startLatitude,double startLongitude,long company)
        {
             int distance;
            using (var webClient = new WebClient())
            {
           
              
                Vehicle[] vehicles = _context.Vehicles.Where(v => v.isCurrentlyActive && 
                                                                                        v.Latitude > 0 && v.Longtitude > 0
                                                                                        &&v.Company.Id==company)
                                                                                        .ToArray();
              
                string url = "https://matrix.route.api.here.com/routing/7.2/calculatematrix.json"
                                   + "?app_id=ORWs1MBbnXAyzlgdPGpw"
                                    + "&app_code=ftEQwIdOxSdxiRv6pd1Rvw";

                                 url += "&start0"+"="+startLatitude+","+startLongitude;
                                  for(int i=0;i<vehicles.Length;i++)
                                       url += "&destination"+i+"="+vehicles[i].Latitude+","+vehicles[i].Longtitude;
                                  url+= "&summaryAttributes=distance,traveltime&mode=fastest;car;traffic:disabled";

               var rawJSON = webClient.DownloadStringTaskAsync(url).Result;
                
               JObject rss = JObject.Parse(rawJSON);
                int min = (int)rss["response"]["matrixEntry"][0]["summary"]["distance"]; 
                distance=min;
                int index=0;
                for (int i = 1; i < vehicles.Length; i++)
                {
                   distance = (int)rss["response"]["matrixEntry"][i]["summary"]["distance"];
                    if (distance >= min) continue;
                    
                    min = distance;
                    index = i;
                }
                
                return vehicles[index];

            }
            
        }



    [HttpPost]
        public async Task<ActionResult<Delivery>> GetDelivery(BindingModel B)
        {
            Delivery D = null;
            try
            {
                D = await _context.Deliveries.Where(d=>d.Id==B.id
                                                                                       && d.Driver.Username==B.username
                                                                                       && d.Driver.Password==B.password
                                                                                      ).SingleAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid");
            }

            if (D == null)
            {
                return BadRequest("Invalid");
            }

            return D;
        }

        [HttpPost]
        public async Task<ActionResult<Delivery>> GetClientDelivery(BindingModel B)
        {
            Delivery D = null;
            try
            {
                D = await _context.Deliveries.Where(d => d.Id == B.id
                                                                                       && d.Client.Username == B.username
                                                                                       && d.Client.Password == B.password
                                                                                      ).Include(d=>d.Vehicle).SingleAsync();
                D.Vehicle.Deliveries = null;
              
                D.Vehicle.ScheduledActivities = null;
                D.Vehicle.Bills = null;
            }
            catch (Exception)
            {
                return BadRequest("Invalid");
            }

            if (D == null)
            {
                return BadRequest("Invalid");
            }

            return D;
        }



        [HttpGet]
        public async Task<ActionResult<Result>>  GetTest(Result X)
        {

            return new Result(X.DeliveryId);
        }





        [HttpPost]
        public async Task<ActionResult<IEnumerable<Delivery>>> AnsweredDeliveries(Login L)
        {

            return await _context.Deliveries.Where(d => d.Driver.Username == L.username
                                                                                        && d.Driver.Password == L.password
                                                                                        && d.Answered == true
                                                                                        && d.Started == false).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Delivery>>> StartedDeliveries(Login L)
        {

            return await _context.Deliveries.Where(d => d.Driver.Username == L.username
                                                                                        && d.Driver.Password == L.password
                                                                                        && d.Answered == true
                                                                                        && d.Started == true
                                                                                        && d.Finished == false).ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Delivery>>> FinishedDeliveries(Login L)
        {

            return await _context.Deliveries.Where(d => d.Driver.Username == L.username
                                                                                        && d.Driver.Password == L.password
                                                                                        && d.Answered == true
                                                                                        && d.Started == true
                                                                                        && d.Finished == true).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Delivery>> MakeAnOrder(Order order)
        {
            Company company = _context.Companies.Where(c=>c.Name==order.CompanyName).Single();

            Delivery delivery = new Delivery
            {
                Time = order.Time,
                Client = _context.Clients.Find(order.ClientId),
                Company = company,
                Quantity = order.Quantity,
                SourceCity = order.SourceCity,
                SourceLatitude = order.SourceLatitude,
                SourceLongtitude = order.SourceLongtitude,
                DestinationCity = order.DestinationCity,
                DestinationLatitude = order.DestinationLatitude,
                DestinationLongtitude = order.DestinationLongtitude
            };

            if (company.AutomaticResponse)
            {
                Vehicle v=getNearestVehicle(order.SourceLatitude, order.SourceLongtitude,company.Id);
                delivery.Vehicle = v;
                delivery.Driver = _context.Vehicles.Include(v1 => v1.CurrentDriver).Where(v1 => v1.Id == v.Id).First().CurrentDriver;

                using (var webClient = new WebClient())
                {
                    string url = "https://matrix.route.api.here.com/routing/7.2/calculatematrix.json"
                                       + "?app_id=ORWs1MBbnXAyzlgdPGpw"
                                        + "&app_code=ftEQwIdOxSdxiRv6pd1Rvw";
                    url += "&start0" + "=" + order.SourceLatitude + "," + order.SourceLongtitude;
                    url += "&destination0=" + order.DestinationLatitude + "," + order.DestinationLongtitude;
                    url += "&summaryAttributes=distance,traveltime&mode=fastest;car;traffic:disabled";
                    var rawJSON = webClient.DownloadStringTaskAsync(url).Result;
                    JObject rss = JObject.Parse(rawJSON);              
                    delivery.OptimalDistance = (float)rss["response"]["matrixEntry"][0]["summary"]["distance"]/1000 ;
                   delivery.OptimalTime = (int)rss["response"]["matrixEntry"][0]["summary"]["travelTime"]/60;
                }
                delivery.OptimalFuelConsumption = delivery.OptimalDistance * v.FuelConsumption;
                delivery.Answered = true;
                delivery.Vehicle.isCurrentlyActive = true;
                _context.Vehicles.Update(delivery.Vehicle);
                _context.Deliveries.Add(delivery);

                await _context.SaveChangesAsync();
                delivery.Company = null;
                delivery.Client = null;
                delivery.Vehicle.Company = null;
                delivery.Vehicle.ScheduledActivities = null;
                delivery.Vehicle.Plan = null;
                delivery.Vehicle.Bills = null;
                delivery.Vehicle.CurrentDriver = null;
                delivery.Vehicle.Deliveries = null;
                delivery.Driver.Deliveries = null;
                delivery.Driver.Company = null;

            }
            else
            {
                // Add delivery to table 
                // Manager can later select unanswered deliveries
                delivery.Driver = null; //not known yet
                delivery.Vehicle = null; //not known yet
                _context.Deliveries.Add(delivery);
                await _context.SaveChangesAsync();
                delivery.Company = null;
                delivery.Client = null;
              

            }
           
            
          

            return delivery;
        }

        public async Task<ActionResult<Result>> AddDeliveryBySupervisor(AddDeliveryModel Model)
        {
            // Add new delivery to DB
            // This action MUST return this new delivery's ID
            // This parameters are the main ones. Extra ones can be added later if needed (eg quantity, etc.)
            // Below code is my attempt. Delete it all if you want

        
            Driver driver = _context.Drivers.Find(Model.driverID);
            Vehicle vehicle = _context.Vehicles.Find(Model.vehicleID);
            Delivery newDelivery = new Delivery();
            newDelivery.Answered = true;
            newDelivery.SourceLatitude = Double.Parse(Model.startLatitude);
            newDelivery.SourceLongtitude = Double.Parse(Model.startLongitude);
            newDelivery.DestinationLatitude = Double.Parse(Model.endLatitude);
            newDelivery.DestinationLongtitude = Double.Parse(Model.endLongitude);
            newDelivery.Driver = driver;
            newDelivery.Vehicle = vehicle;
            newDelivery.Company = null;
            newDelivery.Quantity = (int)Model.quantity;
            newDelivery.OptimalDistance = (float)Model.optimalDistance;
            newDelivery.OptimalTime = (int)Model.optimalTime;
            newDelivery.OptimalFuelConsumption = (float)Model.optimalFuelConsumption;
            
            //newDelivery.Time = DateTime.Parse(orderTime);

            
            _context.Deliveries.Add(newDelivery);
           
            _context.SaveChanges();


            return new Result(newDelivery.Id) ;
        }
        public async Task<ActionResult<Result1>>  CancelDelivery(CancelDelivery Model)
        {
            try
            {
                Delivery delivery = _context.Deliveries.Find(Model.deliveryID);
                _context.Deliveries.Remove(delivery);
                _context.SaveChanges();
                return new Result1(true);
            }
            catch(Exception)
            {
                return new Result1(false);
            }
            
        }

        public async Task<ActionResult<Result2>> GetVehicles(GetVehicles Model)
        {
            // Get all active vehicles (with their current deliveries if exist)
            List<Vehicle> vehicles = _context.Vehicles.Where(v=>v.Company.Id==Model.CompanyId
                                                                                                    &&v.isCurrentlyActive)
                                                                                                 .Include(v=>v.CurrentDriver).ToList<Vehicle>();
            foreach(Vehicle v in vehicles)
            {
                
                
                    List<Delivery> deliveries = _context.Deliveries.Where(d => d.Vehicle.Id== v.Id
                                                                                                                               && d.Started && !d.Finished)
                                                                                                                              .Include(d => d.Client)
                                                                                                                              .ToList<Delivery>();
                    foreach (Delivery d in deliveries)
                    {
                        if (d.Client != null)
                            d.Client.Deliveries = null;
                        if (d.Company != null)
                            d.Company = null;
                        if (d.Vehicle != null)
                            d.Vehicle = null;
                    }
                    v.Deliveries = deliveries;
                    if (v.CurrentDriver != null)
                        v.CurrentDriver.Deliveries = null;
                }   
            

            return new Result2(vehicles);
        }


        private bool DeliveryExists(long id)
        {
            return _context.Deliveries.Any(e => e.Id == id);
        }
        public class Result
        {  
            public long DeliveryId { get; set; }
            public Result(long x) { DeliveryId = x; }
        }
        public class Result1
        {
            public bool Success { get; set; }
            public Result1(bool x) { Success = x; }
        }
        public class Result2
        {
            public List<Vehicle> Vehicles { get; set; }
            public Result2(List<Vehicle> x) {Vehicles = x; }
        }

        public class Result3
        {
            public string Vehicles { get; set; }
            public Result3(string x) { Vehicles = x; }
        }
    }
}


