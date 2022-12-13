using AutoMapper;
using ExceptionsMid.Extenstions;
using HealthCare_Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Text;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using HealthCare_EmailService;
using HealtCare_Core.Mapper;
using Healthcare_hc.Factory;
using Healthcare_hc.Models;

namespace Healthcare_hc
{
    public class Startup
    {
        private MapperConfiguration _mapperConfiguration;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(env.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            _mapperConfiguration = new MapperConfiguration(a => {
                a.AddProfile(new Mapping());
            });

            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var emailConfig = Configuration
                                .GetSection("EmailConfiguration")
                                .Get<EmailConfiguration>();

            services.AddSingleton(emailConfig);

            services.AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddCors(o => o.AddPolicy("HealthCarePolicy", policy =>
            {
                policy.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));


            services.AddDbContext<healthcare_hcContext>();

            services.AddSingleton(sp => _mapperConfiguration.CreateMapper());
            services.AddLogging();

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service  
                // note: the specified format code will format the version as "'v'major[.minor][-status]"  
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by URL segment. the SubstitutionFormat  
                // can also be used to control the format of the API version in route templates  
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();

                options.AddSecurityDefinition("Bearer",
                       new OpenApiSecurityScheme()
                       {
                           Description = "Please insert Bearer JWT token into field. Example: 'Bearer {token}'",
                           Name = "Authorization",
                           In = ParameterLocation.Header,
                           Type = SecuritySchemeType.ApiKey
                       });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
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

                options.UseAllOfToExtendReferenceSchemas();

                options.IncludeXmlCommentsFromInheritDocs(includeRemarks: true, excludedTypes: typeof(string));

                options.AddEnumsWithValuesFixFilters(services, o =>
                {
                    // add schema filter to fix enums (add 'x-enumNames' for NSwag or its alias from XEnumNamesAlias) in schema
                    o.ApplySchemaFilter = true;

                    // alias for replacing 'x-enumNames' in swagger document
                    o.XEnumNamesAlias = "x-enum-varnames";

                    // alias for replacing 'x-enumDescriptions' in swagger document
                    o.XEnumDescriptionsAlias = "x-enum-descriptions";

                    // add parameter filter to fix enums (add 'x-enumNames' for NSwag or its alias from XEnumNamesAlias) in schema parameters
                    o.ApplyParameterFilter = true;

                    // add document filter to fix enums displaying in swagger document
                    o.ApplyDocumentFilter = true;

                    // add descriptions from DescriptionAttribute or XML-comments to fix enums (add 'x-enumDescriptions' or its alias from XEnumDescriptionsAlias for schema extensions) for applied filters
                    o.IncludeDescriptions = true;

                    // add remarks for descriptions from XML-comments
                    o.IncludeXEnumRemarks = true;

                    // get descriptions from DescriptionAttribute then from XML-comments
                    o.DescriptionSource = DescriptionSources.DescriptionAttributesThenXmlComments;
                });

                options.AddEnumsWithValuesFixFilters();
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
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


            ApiFactory.RegisterDependencies(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Log.Logger = new LoggerConfiguration()
                          .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Minute)
                          .CreateLogger();

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer { Url = $"{Configuration.GetSection("Domain").Value}" }
                    };
                });
            });

            app.UseSwaggerUI(
            options =>
            {
                // build a swagger endpoint for each discovered API version  
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });

            app.ConfigureExceptionHandler(Log.Logger, env);

            app.UseCors("HealthCarePolicy");

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
