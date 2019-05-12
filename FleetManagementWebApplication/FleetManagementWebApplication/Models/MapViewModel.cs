using FleetManagementWebAplication.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class MapViewModel
    {
        [Required]
        public long CompanyId { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(200)]
        public string CompanyAddress { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyType { get; set; }

        public string OrderType { get; set; }

        [Required]
        public List<Vehicle> ActiveVehicles { get; set; }

        public List<Client> Clients { get; set; }


        public MapLocation[] MapLocations { get; set; }
    }
}
