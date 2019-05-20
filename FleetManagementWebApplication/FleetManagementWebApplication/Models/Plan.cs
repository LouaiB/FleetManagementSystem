using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FleetManagementWebApplication.Models
{
    public class Plan
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Activity> Activities { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public Company Company { get; set; }


    }
}
