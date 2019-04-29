using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetApi1.Models
{
    public class FinishDeliveryModel
    {
       
        public long DeliverySummaryId { get; set; }
        public DateTime EndTime { get; set; }
        public float EndFuelLevel { get; set; }
        public float EndOdometer { get; set; }
    }
}
