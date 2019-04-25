using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetApi1.Models
{
    public class CancelDelivery
    {
        public long vehicleID { get; set; }
        public long deliveryID { get; set; }
    }
}
