using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Healthcare_hc
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                            .ConfigureWebHostDefaults(webHostBuilder => {
                                webHostBuilder
                                      .UseContentRoot(Directory.GetCurrentDirectory())
                                      .UseIISIntegration()
                                      .UseStartup<Startup>();
                            })
                            .Build();
            host.Run();
            return 0;
        }
    }
}
