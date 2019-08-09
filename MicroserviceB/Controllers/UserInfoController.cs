using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroserviceB.Models;
using MicroserviceB.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceB.Controllers
{
    [Route("api/[controller]")]
   
    public class UserInfoController : Controller
    {
        private readonly IUserInfoService _userInfoService;
        public UserInfoController(IUserInfoService userInfoService) {
            _userInfoService = userInfoService;
        }
        public async Task<IActionResult>Post(UserInfo userInfo) {
          UserInfo savedData=  await this._userInfoService.save(userInfo);
          return new OkObjectResult(savedData);
        }
    }
}