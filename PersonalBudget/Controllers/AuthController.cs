﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PersonalBudget.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PersonalBudget.Controllers
{    
    public class AuthController : Controller
    {
        private readonly PersonalBudgetContext _context;
        private readonly IConfigurationRoot _config;
        public AuthController(PersonalBudgetContext context, IConfigurationRoot config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("api/auth/token")]
        public IActionResult CreateToken([FromBody] CredentialsModel model)
        {
            if (model == null)
            {
                return BadRequest("Request is Null");
            }
            var findusr = _context.AppUsers.FirstOrDefault(m => m.UserName.Equals(model.Username) && m.Password.Equals(model.Password));
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