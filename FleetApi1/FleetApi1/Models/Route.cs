using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetApi1.Models
{
    public class Route
    {
        [Required]
        public long Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public List<MapLocation> Points { get; set; }
    }
}
