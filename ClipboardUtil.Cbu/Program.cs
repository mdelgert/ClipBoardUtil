using System;
using System.Threading;
using Microsoft.Extensions.Configuration;
using ClipboardUtil.Core;

namespace ClipboardUtil.Cbu
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var clipboardMqttPublisher = new ClipboardMqttPublisher(configuration);

            var cts = new CancellationTokenSource();

            // Start the clipboard monitor and MQTT publisher
            //await clipboardMqttPublisher.StartAsync(cts.Token);
            clipboardMqttPublisher.StartAsync(cts.Token);

            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            cts.Cancel(); // Cancel the monitoring when the user presses a key
        }
    }
}
