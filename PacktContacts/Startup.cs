﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PacktContacts.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PacktContacts
{
    public class Startup
    {
        public IConfiguration Configuration;
        public IHostingEnvironment Environment;        

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Environment = env;
            Configuration = configuration;            
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJwtBearerAuthentication("PacktAuthentication", options =>
            {
                options.ClaimsIssuer = Configuration["Tokens:Issuer"];
                options.Audience = Configuration["Tokens:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    ValidAudience = Configuration["Tokens:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                    ValidateLifetime = true
                };
            });


            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearerAuthentication(o =>
            //{
            //    // You also need to update /wwwroot/app/scripts/app.js
            //    o.ClaimsIssuer = Configuration["Tokens:Issuer"];
            //    o.Audience = Configuration["Tokens:Audience"];
            //    o.Events = new JwtBearerEvents()
            //    {
            //        OnAuthenticationFailed = c =>
            //        {
            //            c.Response.StatusCode = 500;
            //            c.Response.ContentType = "text/plain";
            //            if (Environment.IsDevelopment())
            //            {
            //                // Debug only, in production do not share exceptions with the remote host.
            //                return c.Response.WriteAsync(c.Exception.ToString());
            //            }
            //            return c.Response.WriteAsync("An error occurred processing your authentication.");
            //        }
            //    };
            //});
            services.AddMvc();

            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            //Using EF Core InMemory Database instead of real database
            services.AddDbContext<PacktContactsContext>(opt => opt.UseInMemoryDatabase("PacktContactsDB"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //JWT Validation 

            app.UseAuthentication();

            app.UseCors("CorsPolicy");
            //Custom middleware added to pipeline
            //app.UsePacktHeaderValidator();

            app.UseMvc();
        }
    }
}
