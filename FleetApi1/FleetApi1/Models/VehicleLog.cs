using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetApi1.Models
{
    public class VehicleLog
    {
        [Required]
        public long DeliverySummaryId { get; set; }     
        [Required]
        public bool HarshAccelerationAndDeceleration { get; set; }
        [Required]
        public bool HarshBreaking { get; set; }
        [Required]
        public bool HardCornering { get; set; }
        [Required]
        public bool Speeding { get; set; }
        [Required]
        public bool SeatBelt { get; set; }
        [Required]
        public bool OverRevving { get; set; }
       [Required]
       public float Odometer { get; set; }
        [Required]
        public bool EngineRunning { get; set; }


        public double Longtitude { get; set; }
        public double Latitude { get; set; }


    }
}
