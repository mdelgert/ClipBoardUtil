using System;
using Microsoft.Extensions.Configuration;

namespace ClipboardUtil.CL
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load the appsettings.json and environment-specific file
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Default";
            //var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
            //var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();

            // Bind the configuration to the AppSettings class
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

            // Use the settings
            Console.WriteLine($"Environment: {appSettings.Environment}");
            Console.WriteLine($"Name: {appSettings.Name}");
            Console.WriteLine($"Logging Level: {appSettings.LoggingLevel}");
        }
    }

    class AppSettings
    {
        public string Environment { get; set; }
        public string Name { get; set; }
        public string LoggingLevel { get; set; }
    }
}
