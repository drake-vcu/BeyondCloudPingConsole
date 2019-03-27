using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace BeyondCloudPingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // setup config file
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddCommandLine(args);
            var config = configBuilder.Build();

            // setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                //.AddOptions() // don't seem to need this setting
                .Configure<BeyondCloudOptions>(config)
                .AddSingleton<IPingService, PingService>()
                .AddLogging(lb => lb.AddConsole())
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogInformation("Starting application");
            
            // do the actual work here
            var service = serviceProvider.GetService<IPingService>();
            service.Ping();

            logger.LogInformation("All done!");
            Console.ReadLine();
        }
    }
}
