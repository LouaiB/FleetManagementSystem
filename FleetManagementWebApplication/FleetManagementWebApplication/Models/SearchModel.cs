using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class SearchModel
    {

        [Column(TypeName = "Date")]

        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-30);

        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; } =DateTime.Now;
        public string Vehicle { get; set; }
        public string Provider { get; set; }
        public string Service { get; set; }

    }
}
