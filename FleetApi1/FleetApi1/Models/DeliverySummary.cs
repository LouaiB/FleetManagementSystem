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
        public int NumberOfSpeedings { get; set; } = 0;
        [Required]
        public bool NumberOfNoSeatbelts { get; set; }
        [Required]
        public bool NumberOfHarshbreaks { get; set; }

    
    }
}
