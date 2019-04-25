using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManagementWebAplication.Models;
using FleetManagementWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetManagementWebApplication.Controllers
{
    /**
     * 
     * !!!!!!!!!!!!!NOTE!!!!!!!!!!!
     * !
     * !
     * !
     * !
     * !!!!ALL DATA MUST BE RETURNED AS JSON!!!!
     * !
     * !
     * !
     * !
     * !!!!!!!!!!!!!NOTE!!!!!!!!!!!
     * 
     * */


    public class MapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MapController(ApplicationDbContext context)
        {
            _context = context;
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
            ViewData["Deliveries"] = _context.Deliveries.Where(d => d.Company.Id == 7).ToList();
            return View();
        }

        public IActionResult Index()
        {
            MapViewModel viewModel = new MapViewModel();

            long companyID = 7; // FOR TESTING

            // I want the company object WITH the vehicles it has that are CURRENTLY ACTIVE
            // Also a vehicle's deliveries list should have its CURRENT deliveries
            // Alter MapViewModel if needed
            // "isCurrentlyActive" attribute should be added to the Vehicle table and migrated
            // Below code is my attempt. Delete it all if you want

            
            Company company = _context.Companies.Find(companyID);

            // Fill out company details
            viewModel.CompanyId = company.Id;
            viewModel.CompanyName = company.Name;
            viewModel.CompanyType = company.Type;
            viewModel.CompanyAddress = company.Address;

            List<Vehicle> activeVehicles = new List<Vehicle>();
            activeVehicles = _context.Vehicles
                //.Where(v => v.isCurrentlyActive)
                .Where(v => v.Company.Id == companyID)
                .Include(v=>v.CurrentDriver)
                .Take(2)
                .ToList<Vehicle>();
          foreach(Vehicle v in activeVehicles)
            {
                List<Delivery> Deliveries = new List<Delivery>();
              
                if (v.CurrentDriver != null)
                {
                    Deliveries = _context.Deliveries.Where(d => d.Driver == v.CurrentDriver)
                                                                                              .Include(d => d.Client)
                                                                                              .ToList<Delivery>();
                    v.CurrentDriver.Deliveries = Deliveries;
                }
                else
                {
                    v.CurrentDriver = new Driver
                    {
                        Id = 0,
                        Name = "No Driver",
                        Username = "No Driver",
                        Password = "No Driver",
                        Birthdate = "No Driver",
                        Address = "No Driver",
                        Phonenumber = "No Driver",
                        Company = company,
                        Deliveries = Deliveries
                    };
                }
                   
           

            }

           

        
            
            viewModel.ActiveVehicles = activeVehicles;
            

            return View(viewModel);
        }

        
        public JsonResult AddDeliveryBySupervisor(AddDeliveryModel Model)
        {
            // Add new delivery to DB
            // This action MUST return this new delivery's ID
            // This parameters are the main ones. Extra ones can be added later if needed (eg quantity, etc.)
            // Below code is my attempt. Delete it all if you want

            long newDeliveryID = 100; // whatever

           
            Delivery newDelivery = new Delivery();
            newDelivery.Answered = true;
            newDelivery.SourceLatitude = Double.Parse(Model.startLatitude);
            newDelivery.SourceLongtitude = Double.Parse(Model.startLongitude);
            newDelivery.DestinationLatitude = Double.Parse(Model.endLatitude);
            newDelivery.DestinationLongtitude = Double.Parse(Model.endLongitude);
            newDelivery.Quantity = 10; // Testing
            //newDelivery.Time = DateTime.Parse(orderTime);

            newDelivery.Company = _context.Companies.Find((long)7); // Testing
            newDelivery.Driver = _context.Drivers.Find(Model.driverID); 
            newDelivery.Vehicle = _context.Vehicles.Find(Model.vehicleID);
            _context.SaveChanges();

            // Add delivery to corresponding driver and vehicle
            _context.Vehicles.Find(Model.vehicleID).Deliveries.Add(newDelivery);
            _context.Drivers.Find(Model.driverID).Deliveries.Add(newDelivery);
            _context.SaveChanges();
            

            return Json(new { Result = newDeliveryID });
        }

        public JsonResult CancelDelivery(long vehicleID, long deliveryID)
        {
            // Removes a delivery from the DB
            // This delivery has NOT been completed, but rather assigner to a driver/vehicle but then the supervisor CANCLED it before it reached the destination
            // Returns true on success

            bool isSuccess = true; // whatever

            return Json(new { Result = isSuccess });
        }

        public JsonResult GetVehicles(bool isActive)
        {
            // Get all active vehicles (with their current deliveries if exist)

              

            return Json(new { /* DATA */ });
        }
    }
}