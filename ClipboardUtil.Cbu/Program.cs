using System;
using System.Threading;
using System.Threading.Tasks;
using ClipboardUtil.Core;

namespace ClipboardUtil.Cbu
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var clipboardMonitor = new ClipboardMonitor();

            var cancellationTokenSource = new CancellationTokenSource();

            Console.WriteLine("Starting clipboard monitoring. Press Enter to stop...");
            var monitoringTask = clipboardMonitor.StartMonitoringAsync(cancellationTokenSource.Token);

            Console.ReadLine(); // Wait for user input to stop
            cancellationTokenSource.Cancel();

            try
            {
                await monitoringTask;
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Clipboard monitoring stopped.");
            }
        }
    }
}
