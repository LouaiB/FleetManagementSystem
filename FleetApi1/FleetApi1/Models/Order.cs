using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetApi1.Models
{
    public class Order
    {
       
        [Required]
        public DateTime Time { get; set; }

        public string CompanyName { get; set; } = "Taxi Company";
        public long ClientId { get; set; }
        [Required]
        public double SourceLongtitude { get; set; }
        [Required]
        public double SourceLatitude { get; set; }
        [Required]
        public string SourceCity { get; set; }
        [Required]
        public double DestinationLongtitude { get; set; }
        [Required]
        public double DestinationLatitude { get; set; }
        [Required]
        public string DestinationCity { get; set; }
        [Required]
        public int Quantity { get; set; }

    }
}
