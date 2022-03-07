using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SiteOfRefuge.API.Middleware;

namespace SiteOfRefuge.API
{
    public class Program
    {
        public static void Main()
        {
            System.Console.WriteLine( "Configuring host builder");

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(
                    configure=>
                    {
                        configure.UseMiddleware<AuthenticationMiddleware>();
                        configure.UseMiddleware<AuthorizationMiddleware>();
                    }
                )
                .Build();

            host.Run();
        }
    }
}