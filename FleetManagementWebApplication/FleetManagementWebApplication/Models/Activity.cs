using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class Activity
    {
        [Required]
        public int Id { get; set; }
        
        public Service Service { get; set; }
        [Required]
        public int Period  { get; set; }

        public List<ScheduledActivity> ScheduledActivities { get; set; }
        public Plan Plan { get; set; }
    }
}
