using HealthCare_Core.Factory;
using Microsoft.Extensions.DependencyInjection;

namespace Healthcare_hc.Factory
{
    public class ApiFactory
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            DataManagerFactory.RegisterDependencies(services);
        }
    }
}
