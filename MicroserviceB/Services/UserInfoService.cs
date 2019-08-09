using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroserviceB.Models;
using Microsoft.Extensions.Logging;

namespace MicroserviceB.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<UserInfoService> _logger;

        public UserInfoService(AppDbContext appDbContext, ILogger<UserInfoService> logger) {
            _dbContext = appDbContext;
            _logger = logger;
         
        }
        public async Task<UserInfo> save(UserInfo userInfo)
        {
            _logger.LogInformation("Saving Started");
            await  _dbContext.AddAsync(userInfo);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Saving User details saved");
            return userInfo;
        }
    }
}
