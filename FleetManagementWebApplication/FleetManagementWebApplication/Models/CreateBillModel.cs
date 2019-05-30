using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class CreateBillModel
    {
        
        public long SelectedService { get; set; }
       public DateTime DateTime { get; set; }       
        public float Cost { get; set; }
        public long SelectedVehicle { get; set; }
         public string Provider { get; set; }

        public List<SelectListItem> Services { get; set; }
        public List<SelectListItem> Vehicles { get; set; }

        public void FillVehiclesAndServices(ApplicationDbContext context,long CompanyId)
        {
            Vehicle[] vehicles = context.Vehicles.Where(v => v.Company.Id == CompanyId).ToArray();
            Service[] services = context.Service.Where(v => v.Company.Id == CompanyId).ToArray();

            Services = new List<SelectListItem>();
            Vehicles = new List<SelectListItem>();

            foreach (Vehicle v in vehicles)
            {
                Vehicles.Add(new SelectListItem { Value = "" + v.Id, Text = v.Make + " " + v.Model + " " + v.LicensePlate });
            }

            foreach (Service v in services)
            {
                Services.Add(new SelectListItem { Value = "" + v.Id, Text = v.Name });
            }

        }


    }
}
