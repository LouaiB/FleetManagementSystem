using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetApi1.Models
{
    public class Company
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Driver> Drivers { get; set; }
    }
}
