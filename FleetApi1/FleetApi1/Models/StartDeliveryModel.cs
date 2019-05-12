using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetApi1.Models
{
    public class StartDeliveryModel
    {
       public long DeliveryId { get; set; }
        public DateTime StartTime { get; set; }
       public float StartFuelLevel { get; set; }
        public float StartOdometer { get; set; }

        public double Longtitude { get; set; }
        public double Latitude { get; set; }

    }
}
