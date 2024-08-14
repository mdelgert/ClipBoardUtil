using System.Threading;
using System.Threading.Tasks;
using ClipboardUtil.Core;
using TextCopy;
using Xunit;

namespace ClipboardUtil.Tests
{
    public class ClipboardMonitorTests
    {
        [Fact]
        public async Task StartMonitoringAsync_ShouldTriggerEventOnClipboardChange()
        {
            // Arrange
            var monitor = new ClipboardMonitor();
            bool eventTriggered = false;
            string expectedText = "Test clipboard text";

            monitor.ClipboardTextChanged += (newText) =>
            {
                eventTriggered = true;
                Assert.Equal(expectedText, newText); // Verify the event carries the correct data
            };

            // Simulate clipboard change
            await ClipboardService.SetTextAsync(expectedText);

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var monitoringTask = monitor.StartMonitoringAsync(cancellationTokenSource.Token);

            // Allow some time for monitoring to detect change
            await Task.Delay(1000);

            // Cancel monitoring task
            cancellationTokenSource.Cancel();
            try
            {
                await monitoringTask;
            }
            catch (TaskCanceledException)
            {
                // Expected, since we're cancelling the task
            }

            // Assert
            Assert.True(eventTriggered);
        }
    }
}
