using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SafApp.Models;

namespace SafApp.Services
{
    public class UserInforService: IUserInfoService
    {
        private readonly IHttpClientFactory _clientFactory;
        public UserInforService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async void makeApiRequest(UserInfo userIfo)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
             "https://api.github.com/repos/aspnet/AspNetCore.Docs/branches");
            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                UserInfo user = await response.Content
                    .ReadAsAsync<UserInfo>();
            }
           
        }
    }
}
