using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetApi1.Models
{
    public class Client
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Birthdate { get; set; }
        public string Address { get; set; }
        public string Phonenumber { get; set; }
        
    }
}
