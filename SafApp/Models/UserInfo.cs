using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SafApp.Models
{
    public class UserInfo
    {
        [Required]
        public int Age { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Gender { get; set; }
        [Required]
        public String Eductaion { get; set; }
    }
}
