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
        public double Langtitude { get; set; }
        [Required]
        public double Latitude { get; set; }
    }
}
