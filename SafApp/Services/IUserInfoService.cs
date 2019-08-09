using SafApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SafApp.Services
{
    interface IUserInfoService
    {
         void makeApiRequest(UserInfo userIfo);
    }
}
