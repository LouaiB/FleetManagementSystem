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
        public string LicensePlate{ get; set; }
        [Required]
        [StringLength(50)]
        public string Make { get; set; }
        [Required]
        [StringLength(50)]
        public string Model { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime purchaseDate { get; set; }
        public decimal Odometer { get; set; }
        public Company Company { get; set; }
        public Driver Driver { get; set; }
    
        [Required]
        public int PayLoad { get; set; }
        [Required]
        public int EmissionsCO2 { get; set; }
       [Required]
        public int FuelConsumption  { get; set; }
        [Required]
        
        public int fuelType { get; set; }

        public int FuelLevel { get; set; }
        public int CurrentLoad { get; set; }
        public Driver CurrentDriver { get; set; }


    }
}
