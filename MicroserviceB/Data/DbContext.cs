using MicroserviceB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroserviceB
{
    public class AppDbContext:DbContext
    {
       public DbSet<UserInfo> UserInfos { get; set; }
    }
}
