﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SafApp.Models
{
    public class SystemUserViewModel
    {
        public Int64 Id { get; set; }
        public string  UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName  => $"{ FirstName} {LastName}"; 



    }
}
