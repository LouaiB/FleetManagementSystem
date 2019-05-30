using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class AddClient
    {
        [Required]
        [StringLength(20)]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        [MinLength(6), MaxLength(12)]
        public string Password { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
       
        [StringLength(20)]
        public string Phone { get; set; }

        public string Address { get; set; }



    }
}
