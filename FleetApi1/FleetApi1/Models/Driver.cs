using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FleetApi1.Models
{
    public class Driver
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8),MaxLength(12)]
        public string Password { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(20)]
        public string Birthdate { get; set; }
        [Required]
        [StringLength(100)]
        public string Address { get; set; }
        [Required]
        [StringLength(100)]
        public string Phonenumber { get; set; }
        public int Rank { get; set; }
        public float Score { get; set; }
        public string Image { get; set; }
        public Company Company { get; set; }
        public List<Delivery> Deliveries { get; set; }

    }
}
