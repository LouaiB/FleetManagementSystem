﻿using FleetManagementWebAplication.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class Delivery
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public DateTime Time { get; set; }

        [Required]
        public Client Client { get; set; }
        public Vehicle Vehicle { get; set; }
        public Driver Driver { get; set; }

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
      
        public bool Answered { get; set; } = false;
        public bool Started { get; set; } = false;
        public bool Finished { get; set; } = false;

        public float OptimalDistance { get; set; }
        public int OptimalTime { get; set; }
        public float OptimalFuelConsumption { get; set; }

    }
}
