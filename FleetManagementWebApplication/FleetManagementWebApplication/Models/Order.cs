using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class Order
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public DateTime Time { get; set;}
        [Required]
        public double Longtitude { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
