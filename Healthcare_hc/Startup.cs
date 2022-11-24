using AutoMapper;
using HealtCare_Core.Managers;
using HealtCare_Core.Managers.Interfaces;
using HealtCare_Core.Mapper;
using HealthCare_Core.Managers.Interfaces;
using Healthcare_hc.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare_hc
{
    public class Startup
    {
        private MapperConfiguration _mapperConfiguration;
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            _mapperConfiguration = new MapperConfiguration(a => {
                a.AddProfile(new Mapping());
            });

            Configuration = configuration;
        }

     

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Healthcare_hc", Version = "v1" });
               
                c.AddSecurityDefinition("Bearer",
                         new OpenApiSecurityScheme()
                         {
                             Description = "Please insert Bearer JWT token into field. Example: 'Bearer {token}'",
                             Name = "Authorization",
                             In = ParameterLocation.Header,
                             Type = SecuritySchemeType.ApiKey
                         });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                    });

            });

            services.AddDbContext<healthcare_hcContext>();

            services.AddScoped<IUserManager, UserManager>();

            services.AddScoped<ICityManager, CityManager>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                  .AddJwtBearer(c =>
                  {
                      c.TokenValidationParameters = new TokenValidationParameters
                      {
                          ValidateIssuer = true,
                          ValidateAudience = true,
                          ValidateLifetime = true,
                          ValidateIssuerSigningKey = true,
                          ValidIssuer = Configuration["Jwt:Issuer"],
                          ValidAudience = Configuration["Jwt:Issuer"],
                          ClockSkew = TimeSpan.Zero,
                          IssuerSigningKey = new SymmetricSecurityKey(
                                                 Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                      };
                  });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Healthcare_hc v1"));
              
            }

            Log.Logger = new LoggerConfiguration()
                          .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Minute)
                          .CreateLogger();
           

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
