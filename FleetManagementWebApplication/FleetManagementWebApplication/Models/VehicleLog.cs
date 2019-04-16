using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class VehicleLog
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public Vehicle Vehicle { get; set; }
        [Required]
        public Driver Driver { get; set; }
        [Required]
        public double Langtitude { get; set; }
        [Required]
        public double  Latitude { get; set; }
        [Required]
        public float Fuel { get; set; }
        [Required]
        public int Speed { get; set; }
        [Required]
        public float Odometer { get; set; }
        [Required]
        public bool Seatbelt { get; set; }
        [Required]
        public bool Harshbreak { get; set; }

    }
   
}
