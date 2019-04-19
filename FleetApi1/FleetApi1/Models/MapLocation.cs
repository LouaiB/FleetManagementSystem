using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetApi1.Models
{
    public class MapLocation
    {
        [Required]
        public long Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public float Langtitude { get; set; }
        [Required]
        public float Latitude { get; set; }
        public Route Route { get; set; }
    }
}
