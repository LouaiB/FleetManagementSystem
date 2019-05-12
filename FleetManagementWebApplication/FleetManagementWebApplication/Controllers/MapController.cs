using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FleetManagementWebAplication.Models;
using FleetManagementWebApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FleetManagementWebApplication.Controllers
{
        public class MapController : FleetController
    {
        IHostingEnvironment _environment;
        public MapController(ApplicationDbContext context, IHostingEnvironment environment):base(context)
        {
            _environment = environment;
        }
        [HttpGet]
         public IActionResult Index(long Id=0)
        {
            if (!LogedIn())
                return RedirectToRoute("home");
            
            if (Id > 0)
            {
      
                ViewData["NotificationDelivery"] = _context.Deliveries.Where(d => d.Id == Id).Include(d=>d.Client).First();
                ViewData["NotificationFlag"] = true;

            }
            MapViewModel viewModel = new MapViewModel();

            

            Company company = _context.Companies.Find(CompanyId);

            viewModel.CompanyId = company.Id;
            viewModel.CompanyName = company.Name;
            viewModel.CompanyType = company.Type;
            viewModel.CompanyAddress = company.Address;
            viewModel.OrderType = company.OrderType;

            List<Vehicle> activeVehicles = new List<Vehicle>();
            activeVehicles = _context.Vehicles
                .Include(v => v.CurrentDriver)
                .Include(v => v.CurrentDriver.Company)
                .Where(v => v.isCurrentlyActive)
                .Where(v => v.Company.Id == CompanyId)
                .ToList();

            activeVehicles.ForEach(v =>
            {
                v.CurrentDriver.Deliveries = _context.Deliveries
                    .Include(d => d.Client)
                    .Include(d => d.Company)
                    .Where(d => (d.Driver == v.CurrentDriver && d.Company == company))
                    .Where(d => d.Finished == false && d.Started== true)
                    .ToList();

            });




            viewModel.ActiveVehicles = activeVehicles;

            // Fill out clients
            viewModel.Clients = _context.Clients
                .Include(c => c.Deliveries)
                .ToList();
                viewModel.MapLocations = _context.MapLocations.Where(m => m.Company.Id == CompanyId).ToArray();

            return View(viewModel);
        }

        
       

  



        public JsonResult CancelDelivery(string vehicleID, string deliveryID)
        {
            // Removes a delivery from the DB
            // This delivery has NOT been completed, but rather assigned to a driver/vehicle but then the supervisor CANCLED it before it reached the destination
            // Returns true on success
          
            bool isSuccess = true;         
            var delivery = _context.Deliveries.Find(Convert.ToInt64(deliveryID));
            try
            {
                DeliverySummary deliverySummary = _context.DeliverySummaries.Where(s => s.Delivery == delivery).Single();
                _context.Remove(deliverySummary);
            }
            catch (Exception)
            {

            }
           
           _context.Remove(delivery);
            _context.SaveChanges();

            return Json(new { Result = isSuccess });
        }

        


        [HttpPost]
        public JsonResult GetValues(string x)
        {
            Result r = new Result();
            r.vehicleID = x;
            return Json(r);
        }

        public JsonResult AddDeliveryBySupervisor(
          string vehicleID,
          string driverID,
          string clientID,
          string startLatitude,
          string startLongitude,
          string endLatitude,
          string endLongitude,
          string quantity,
          string date,
          string sourceCity,
          string destinationCity
          )
        {
           
            Delivery newDelivery = new Delivery
            {
                Answered = true,
                SourceLatitude = Double.Parse(startLatitude),
                SourceLongtitude = Double.Parse(startLongitude),
                DestinationLatitude = Double.Parse(endLatitude),
                DestinationLongtitude = Double.Parse(endLongitude),
                Quantity = Convert.ToInt32(quantity),
                Company = _context.Companies.Find(CompanyId),
                Driver = _context.Drivers.Find(Convert.ToInt64(driverID)),
                Vehicle = _context.Vehicles.Find(Convert.ToInt64(vehicleID)),
                Client = _context.Clients.Find(Convert.ToInt64(clientID)),
                Time = DateTime.Now,
                SourceCity = sourceCity,
                DestinationCity = destinationCity
            };

            // Add delivery to corresponding driver and vehicle
            _context.Add(newDelivery);
            _context.SaveChanges();

            return Json(new { Result = newDelivery.Id });
        }



        // Testing [ No need to code anything here ]
        public IActionResult ShowCompanies()
        {
            List<Company> companies = _context.Companies.ToList<Company>();
            ViewData["Companies"] = companies;
            return View();
        }

        // Testing [ No need to code anything here ]
        public IActionResult ShowDeliveries()
        {
            ViewData["Deliveries"] = _context.Deliveries
                .Include(d => d.Company)
                .Include(d => d.Driver)
                .Include(d => d.Vehicle)
                .Where(d => d.Company.Id == 2)
                .ToList();
            return View();
        }
        [HttpGet]
        public IActionResult CreateMapLocation()
        {
            if (!LogedIn())
                return RedirectToRoute("/home");
           return View();
        }

        [HttpPost]
        public IActionResult CreateMapLocation(MapLocation mapLocation, IFormFile file)
        {
            if (!LogedIn())
                return RedirectToRoute("/home");

            var filePath = Path.GetTempFileName();

            string image = "";

            using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\images\\" + file.FileName))
            {
                image = file.FileName;

                file.CopyTo(filestream);
                filestream.Flush();
            }

            if (ModelState.IsValid)
            {
                mapLocation.Company = _context.Companies.Find(CompanyId);
                mapLocation.Image = image;
                _context.MapLocations.Add(mapLocation);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mapLocation);
        }

        [HttpGet]
        public IActionResult AddClient()
        {
            if (!LogedIn())
                return RedirectToRoute("/home");
            return View();
        }

        [HttpPost]
        public IActionResult AddClient(AddClient model)
        {
            if (!LogedIn())
                return RedirectToRoute("/home");

            if (ModelState.IsValid)
            {
                Client c = new Client()
                {Name=model.Name,
                 Username=model.Username,
                 Password=model.Password,
                 Phonenumber=model.Phone
                };
                _context.Clients.Add(c);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpPost]
        public JsonResult UpdateDeliveryBySupervisor(string deliveryId,string vehicleId, string driverId)
        {
            long id;
            try
            {
                Delivery delivery = _context.Deliveries.Find(Convert.ToInt64(deliveryId));
                delivery.Driver = _context.Drivers.Find(Convert.ToInt64(driverId));
                delivery.Vehicle = _context.Vehicles.Find(Convert.ToInt64(vehicleId));            
                delivery.Answered = true;
               _context.Update(delivery);
                _context.SaveChanges();
                id =delivery.Id;
            }
            catch (Exception)
            {
                id = 0;
            }

            return Json(new { Result = id });
        }




        public JsonResult GetVehicles(bool isActive)
        {
            long companyID = (long)HttpContext.Session.GetInt32("CompanyId"); // FOR TESTING
            Company company = _context.Companies.Find(companyID);

            // Get all active vehicles (with their current deliveries if exist)
            List<Vehicle> activeVehicles = new List<Vehicle>();
            activeVehicles = _context.Vehicles
                .Include(v => v.CurrentDriver)
                .Include(v => v.CurrentDriver.Company)
                .Where(v => v.isCurrentlyActive)
                .Where(v => v.Company.Id == companyID)
                .ToList();

            activeVehicles.ForEach(v =>
            {
                v.CurrentDriver.Deliveries = _context.Deliveries
                    .Include(d => d.Client)
                    .Include(d => d.Company)
                    .Where(d => (d.Driver == v.CurrentDriver && d.Company == company))
                    .Where(d => d.Finished == false && d.Started == true)
                    .ToList();
            });

            // Create JSON result string
            string json = "[";
            activeVehicles.ForEach(v => {
                json += "{";
                json += "\"Id\": \"" + v.Id + "\",";
                json += "\"Latitude\": \"" + v.Latitude + "\",";
                json += "\"Longtitude\": \"" + v.Longtitude + "\",";
                json += "\"Make\": \"" + v.Make + "\",";
                json += "\"Model\": \"" + v.Model + "\",";
                json += "\"LicensePlate\": \"" + v.LicensePlate + "\",";
                json += "\"CurrentDriverId\": \"" + v.CurrentDriver.Id + "\",";
                json += "\"CurrentDriverName\": \"" + v.CurrentDriver.Name + "\",";
                json += "\"CurrentDriverPhonenumber\": \"" + v.CurrentDriver.Phonenumber + "\",";
                json += "\"CurrentDriverBirthdate\": \"" + v.CurrentDriver.Birthdate + "\",";

                json += "\"Deliveries\": [";
                v.Deliveries?.ForEach(d => {
                    json += "{";
                    json += "\"deliveryID\": \"" + d.Id + "\",";
                    json += "\"SourceLatitude\": \"" + d.SourceLatitude + "\",";
                    json += "\"SourceLongtitude\": \"" + d.SourceLongtitude + "\",";
                    json += "\"DestinationLatitude\": \"" + d.DestinationLatitude + "\",";
                    json += "\"DestinationLongtitude\": \"" + d.DestinationLongtitude + "\",";
                    json += "\"SourceCity\": \"" + d.SourceCity + "\",";
                    json += "\"DestinationCity\": \"" + d.DestinationCity + "\",";
                    json += "\"info\": {";
                    json += "\"customerName\": \"" + d.Client.Name + "\",";
                    json += "\"customerEmail\": \"" + d.Client.Username + "\",";
                    json += "\"quantity\": \"" + d.Quantity + "\",";
                    json += "\"orderTime\": \"" + d.Time + "\"";
                    json += "}";

                    json += "},";
                });
                json = json.TrimEnd(',');
                json += "]";

                json += "},";
            });
            json = json.TrimEnd(',');
            json += "]";

            return Json(new { Result = json });
        }
        [HttpGet]
        public IActionResult AutomaticResponse()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            Company company = _context.Companies.Find(CompanyId);
           ViewData["switch"] = company.AutomaticResponse;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SwitchMode(bool AutomaticResponse = false)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            Company company = _context.Companies.Find(CompanyId);
            company.AutomaticResponse = AutomaticResponse;
            _context.Update(company);
            _context.SaveChanges();
            return RedirectToRoute("Home");
        }



public class Result
        {
            public string vehicleID;
        }
       
    }
}