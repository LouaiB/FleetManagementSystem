using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FleetApi1.Models
{
    public class Bill
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Service { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime DateTime { get; set; }
        [Required]
        public float Cost { get; set; }
        public Vehicle Vehicle { get; set; }
        [StringLength(150)]
        public string Provider { get; set; }

    }
}
