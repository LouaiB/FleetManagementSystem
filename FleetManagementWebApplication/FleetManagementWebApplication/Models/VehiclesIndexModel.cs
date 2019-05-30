using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class VehiclesIndexModel
    {
        public string Model { get; set; }
        public string Make { get; set; }
        public string LicensePlate { get; set; }
        public long SelectDriverId { get; set; }
        public int Status { get; set; }
        public Vehicle[] Vehicles { get; set; }
        public List<SelectListItem> Drivers;
        public List<SelectListItem> Statuses = new List<SelectListItem>() {new SelectListItem { Value="0",Text="Any"} ,
                                                                                                                                new SelectListItem { Value="1",Text="Active"} ,
                                                                                                                               new SelectListItem { Value="2",Text="On The Road"} ,
                                                                                                                              new SelectListItem { Value="3",Text="Inactive"}  };
       public void FillSelectListDrivers(ApplicationDbContext context,long CompanyId)
        {
            Driver[] drivers= context.Drivers.Where(d => d.Company.Id == CompanyId).ToArray();
            Drivers = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "Any" }
            };
            foreach (Driver d in drivers)
            {
                Drivers.Add(new SelectListItem { Value =""+ d.Id, Text = d.Name });
            }

        }
        public void FillVehicles(ApplicationDbContext context, long CompanyId)
        {
            Vehicles = context.Vehicles.Where(v => v.Company.Id == CompanyId).Include(v => v.CurrentDriver).ToArray();

        }

    }
}
