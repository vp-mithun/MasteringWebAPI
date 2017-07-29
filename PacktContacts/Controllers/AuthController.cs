using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PacktContacts.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PacktContacts.Controllers
{    
    public class AuthController : Controller
    {
        private readonly PacktContactsContext _context;
        private readonly IConfiguration _config;
        public AuthController(PacktContactsContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        private List<AppUser> AppUserList()
        {
            var listUsr = new List<AppUser>();
            var testUser1 = new AppUser
            {
                Id = 1,
                UserName = "mithunvp",
                Password = "abcd123",
                IsSuperUser = true
            };

            var testUser2 = new AppUser
            {
                Id = 2,
                UserName = "Yogi",
                Password = "abcd123",
                IsSuperUser = false
            };

            listUsr.Add(testUser1);
            listUsr.Add(testUser2);

            return listUsr;
        }

        [HttpPost("api/auth/token")]
        public IActionResult CreateToken([FromBody] CredentialsModel model)
        {
            if (model == null)
            {
                return BadRequest("Request is Null");
            }
            var findusr = AppUserList().FirstOrDefault(m => m.UserName.Equals(model.Username) && m.Password.Equals(model.Password));
            if(findusr != null)
            {
                var claims = new[]
                {
                  new Claim(JwtRegisteredClaimNames.Sub, findusr.UserName),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim("SuperUser", findusr.IsSuperUser.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                  issuer: _config["Tokens:Issuer"],
                  audience: _config["Tokens:Audience"],
                  claims: claims,
                  expires: DateTime.UtcNow.AddMinutes(12),
                  signingCredentials: creds
                  );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),                    
                    expiration = token.ValidTo
                });
            }
            return BadRequest("Failed to generate Token");
        }
    }
}