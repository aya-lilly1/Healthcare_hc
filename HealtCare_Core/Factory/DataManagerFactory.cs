using HealthCare_.Notifications.Factory;
using HealthCare_infrastructure.Factory;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace HealthCare_Core.Factory
{
    public class DataManagerFactory
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            InfrastructureFactory.RegisterDependencies(services);
            NotificationsFactory.RegisterDependencies(services);

            Assembly assembly = typeof(DataManagerFactory).GetTypeInfo().Assembly;

            var allManagers = assembly.GetTypes().Where(t => t.Name.EndsWith("Manager"));

            foreach (var type in allManagers)
            {
                var allInterfaces = type.GetInterfaces();
                var mainInterfaces = allInterfaces.Except(allInterfaces.SelectMany(t => t.GetInterfaces()));
                foreach (var itype in mainInterfaces)
                {
                    services.AddScoped(itype, type);
                }
            }
        }

    }
}
