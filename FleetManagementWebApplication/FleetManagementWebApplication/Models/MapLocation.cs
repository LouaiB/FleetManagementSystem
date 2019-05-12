using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class MapLocation
    {
        [Required]
        public long Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public float Longtitude { get; set; }
        [Required]
        public float Latitude { get; set; }
        public Company Company { get; set; }

        [StringLength(100)]
        public string Image { get; set; }
        public Route Route { get; set; }
    }
}
