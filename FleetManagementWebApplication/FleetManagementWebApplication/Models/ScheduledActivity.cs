using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class ScheduledActivity
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public Vehicle Vehicle { get; set; }
        [Required]
        public Activity Activity { get; set; }
        [Required]
        public DateTime DueDate{ get; set; }
        public long CompanyId { get; set; }

    }
}
