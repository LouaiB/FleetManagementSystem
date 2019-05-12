using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetApi1.Models
{
    public class AddDeliveryModel
    {
        public long vehicleID {get;set; }
        public long driverID { get; set; } 
        public string startLatitude { get; set; }
       public string startLongitude { get; set; }
       public string endLatitude { get; set; }
        public string endLongitude { get; set; }
        public long quantity { get; set; }
        public long optimalDistance { get; set; }
        public long optimalTime { get; set; }
        public long optimalFuelConsumption { get; set; }
   
        
    }
}
