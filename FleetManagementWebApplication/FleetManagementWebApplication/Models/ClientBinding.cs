using FleetManagementWebApplication.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebAplication.Models
{
    public class ClientBinding
    {
        
        [Required]
        [StringLength(20)]
        public string Username { get; set; }
        [Required]
        [MinLength(6),MaxLength(12)]
        public string Password { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Birthdate { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [StringLength(20)]
        public string Phonenumber { get; set; }
     




    }
}
