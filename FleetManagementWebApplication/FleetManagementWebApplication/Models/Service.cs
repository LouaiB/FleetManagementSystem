using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class Service
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public Company Company { get; set; }
        public List<Activity> Activities { get; set; }
    }
}
