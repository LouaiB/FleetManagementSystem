using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class CreatePlanModel
    {
        public string Title { get; set; }
        public long[] SelectedServices { get; set; }
       public  int[] ActivityPeriod { get; set; }
        public List<SelectListItem> Services { get; set; }
        public void FillServices(ApplicationDbContext context,long companyId)
        {
            Services = new List<SelectListItem>() { new SelectListItem { Value = "0", Text = "Select a service",Selected=true } };
            Service[] services = context.Service.Where(s => s.Company.Id == companyId).ToArray();
            foreach(Service s in services)
            {
                  Services.Add(new SelectListItem { Value = "" + s.Id, Text = s.Name });
            }
        }
    }
}
