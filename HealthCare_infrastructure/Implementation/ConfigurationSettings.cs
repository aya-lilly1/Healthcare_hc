using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace HealthCare_infrastructure.Implementation
{
    public class ConfigurationSettings : IConfigurationSettings
    {
        #region Private Variables
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        #endregion Private Variables

        public ConfigurationSettings(IWebHostEnvironment env, IConfiguration config)
        {
            _config = config;
            _env = env;
        }

        public string JwtKey => _config["Jwt:Key"];

        public string Issuer => _config["Jwt:Issuer"];

        public string WebSiteURl => _config["URL:WebSiteURl"];

    }
}
