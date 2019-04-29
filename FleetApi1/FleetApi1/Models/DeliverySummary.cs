using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetApi1.Models
{
    public class DeliverySummary
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public Delivery Delivery { get; set; }

        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public float StartFuelLevel { get; set; }
        [Required]
        public float EndFuelLevel { get; set; }

        [Required]
        public float StartOdometer { get; set; }
        [Required]
        public float EndOdometer { get; set; }

		
        [Required]
        public float HarshAccelerationAndDeceleration{ get; set; } 
        [Required]
        public float HarshBreakingsRate { get; set; }
        [Required]
        public float HardCorneringRate { get; set; }
        [Required]
        public float SpeedingsRate { get; set; } 
        [Required]
        public float SeatBeltRate { get; set; }
        [Required]
        public float OverRevving { get; set; }
        [Required]
        public float OnTimeDeliveryRate { get; set; }
        [Required]
        public float FuelConsumptionRate { get; set; }
        [Required]
        public float Idling { get; set; }
         public float PerformanceScore { get; set; }
        public float ComplianceScore { get; set; }
        public float SafetyScore{ get; set; }

    
    }
}
