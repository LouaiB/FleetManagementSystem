using FleetApi1Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetApi1.Models
{
    public class Company
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(200)]
        public string Address { get; set; }
        [Required]
        [StringLength(100)]
        public string Type { get; set; }
        [Required]
        public int Size { get; set; }
        [StringLength(100)]
        public string OrderType { get; set; }
        public bool AutomaticResponse { get; set; }
        public List<Driver> Drivers { get; set; }
        public Manager Manager { get; set; }
        public List<Vehicle> Vehicles { get; set; }
    }
}
