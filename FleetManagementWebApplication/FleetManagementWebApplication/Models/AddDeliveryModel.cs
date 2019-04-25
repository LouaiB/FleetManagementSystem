using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class AddDeliveryModel
    {
        public long vehicleID {get;set; }
        public long driverID { get; set; } 
        public string startLatitude { get; set; }
       public string startLongitude { get; set; }
       public string endLatitude { get; set; }
        public string endLongitude { get; set; }
    }
}
