using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class FuelLog
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public Vehicle Vehicle { get; set; }
        [Required]
        public float Quantity { get; set; }
        [Required]
        public string FuelType { get; set; }
        [Required]
        public float PricePerLitre { get; set; }

        [Required]
        public string Provider { get; set; }

    }
}
