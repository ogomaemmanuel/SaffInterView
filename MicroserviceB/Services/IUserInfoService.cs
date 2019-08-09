using MicroserviceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroserviceB.Services
{
   public  interface IUserInfoService
    {

        Task<UserInfo> save(UserInfo userInfo); 
    }
}
