using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SqlDbApplication.Configurations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SqlDbApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly AuthenticationConfiguration authenticationConfiguration;
        private readonly ILogger<AuthenticationController> logger;

        public AuthenticationController(
            IOptions<AuthenticationConfiguration> authenticationConfiguration,
            ILogger<AuthenticationController> logger)
        {
            this.authenticationConfiguration = authenticationConfiguration.Value;
            this.logger = logger;
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequest authenticationRequest)
        {
            //1. Validate
            var user = ValidateCredentials(authenticationRequest);
            if (user == null)
            {
                return Unauthorized($"{authenticationRequest.Username} is has provided credentials. Please try again.");
            }

            //2.create a token for validated user
            var keyInBytes = Encoding.ASCII.GetBytes(authenticationConfiguration.SecretForKey);
            var securityKey = new SymmetricSecurityKey(keyInBytes);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userIdentityClaimed = new List<Claim>()
            {
                new Claim("sub", user.UserId.ToString()),
                new Claim("given_name", user.FirstName),
                new Claim("family_name", user.LastName),
                new Claim("city", user.City)
            };

            var jwtSecurityToken = new JwtSecurityToken(
                authenticationConfiguration.Issuer,
                authenticationConfiguration.Audience,
                userIdentityClaimed,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(token);
        }

        private User ValidateCredentials(AuthenticationRequest authenticationRequest)
        {
            // to demonstrate invalid user scenario
            if (authenticationRequest.Username.Equals("Mike", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            return new User(1, "madmax", "John", "Wick", "Seatte");
        }
    }

    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }

        public User()
        {
        }

        public User(int userId, string userName, string firstName, string lastName, string city)
        {
            UserId = userId;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            City = city;
        }
    }

    public class AuthenticationRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
