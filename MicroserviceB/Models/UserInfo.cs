using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MicroserviceB.Models
{
    public class UserInfo
    {
        [Required]
        public int Age { get; set; }
        [Required]
        public int Name { get; set; }
        [Required]
        public int Genger { get; set; }
        [Required]
        public int Education { get; set; } 
    }
}
