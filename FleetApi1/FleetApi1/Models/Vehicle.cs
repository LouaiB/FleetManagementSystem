using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FleetApi1.Models
{
    public class Vehicle
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [StringLength(50)]
        public string LicensePlate { get; set; }
        [Required]
        [StringLength(50)]
        public string Make { get; set; }
        [Required]
        [StringLength(50)]
        public string Model { get; set; }
        
        public double Longtitude { get; set; }
        public double Latitude { get; set; }

        public Company Company { get; set; }
        public Driver CurrentDriver { get; set; }
        public bool isCurrentlyActive { get; set; }
        public bool isOnTheRoad { get; set; }


        [Column(TypeName = "Date")]
        public DateTime purchaseDate { get; set; }   
        
        
        public int PayLoad { get; set; }

        
        public int EmissionsCO2 { get; set; }
        
        public int FuelConsumption { get; set; }
        
        public int FuelLevel { get; set; }
        public int CurrentLoad { get; set; }
        public float Odometer { get; set; }

        public Plan Plan { get; set; }
        public string Status { get; set; }

        public List<Bill> Bills { get; set; }
        public List<ScheduledActivity> ScheduledActivities { get; set; }
        public List<Delivery> Deliveries { get; set; }
        public List<FuelLog> fuelLogs { get; set; }
        public string Image { get; set; }
        public string Icon { get; set; }

    }
}
