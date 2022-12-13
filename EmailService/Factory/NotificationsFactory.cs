
using HealthCare_Implementation;
using HealthCare_Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace HealthCare_.Notifications.Factory
{
    public static class NotificationsFactory
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            services.AddScoped<IEmailSender, EmailSender>();
        }
    }
}