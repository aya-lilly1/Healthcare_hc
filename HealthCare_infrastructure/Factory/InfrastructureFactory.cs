using HealthCare_infrastructure.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace HealthCare_infrastructure.Factory
{
    public class InfrastructureFactory
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            services.AddScoped<IConfigurationSettings, ConfigurationSettings>();
        }
    }
}
