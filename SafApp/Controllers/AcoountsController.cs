using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SafApp.Models;

namespace SafApp.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
       
        private SignInManager<User> _signInManager;
        private RoleManager<Role> _roleManager;
        private IConfiguration _config;
        public AccountController(UserManager<User> userManager,
          
           SignInManager<User> signInManager,
           RoleManager<Role> roleManager,
           IConfiguration config
            )
        {
            _userManager = userManager;
          
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }
        // POST api/account
        [HttpPost("register")]

        public async Task<IActionResult> Post([FromBody] RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email,
                    
                };
                var password = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
                IdentityResult result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                   // var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                   // await _emailSender.SendEmailConfirmationAsync(registerViewModel.Email, callbackUrl, password);
                    var userViewModel = this.MapUserToViewModel(user);
                    return new OkObjectResult(userViewModel);
                }
            }

            return new BadRequestResult();

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("InvalidLogin", "User does not exist");
                    return new BadRequestObjectResult(ModelState);
                }
                if (!await _userManager.CheckPasswordAsync(user, loginViewModel.Password))
                {
                    ModelState.AddModelError("InvalidLogin", "Username and  password do not match");
                    return new BadRequestObjectResult(ModelState);
                }
                var token = await BuildToken(user);
                return new OkObjectResult(token);
            }
            return new BadRequestObjectResult(ModelState);

        }

        [HttpGet("confirm-email", Name = "ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {

            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return new OkObjectResult(result.Succeeded ? "Email Confirmation successful" : "Error");
        }


        

        private async Task<object> BuildToken(User user)
        {

            var claims = new List<Claim> {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),


        
       // new Claim(JwtRegisteredClaimNames.)
        //new Claim(JwtRegisteredClaimNames.Birthdate, user.Birthdate.ToString("yyyy-MM-dd")),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var epiryTime = DateTime.Now.AddHours(1);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: epiryTime,
              signingCredentials: creds);
            var authToken = new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                expires_in = 60 * 60,
                UserDetails = new SystemUserViewModel
                {
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Id = user.Id,
                    UserName = user.Email,
                }
            };
            return authToken;
        }

        //public async Task<IActionResult> GetAllUsers() {
        //    this._roleManager.

        //}


        private SystemUserViewModel MapUserToViewModel(User user)
        {
            return new SystemUserViewModel
            {
                Email = user.Email,
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
            };
        }
    }
}