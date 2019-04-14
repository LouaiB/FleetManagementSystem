using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class ServiceLog
    { 
        [Required]
        public long Id { get; set; }
        [Required]
        public Vehicle Vehicle { get; set; }
        [Required]
        public Activity Activity { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [StringLength(100)]
        public string Provider { get; set; }
        public float Cost { get; set; }
    }
}
