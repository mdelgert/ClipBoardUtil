using System;
using System.Threading;
using System.Threading.Tasks;
using TextCopy;

namespace ClipboardUtil.Core
{
    public class ClipboardMonitor
    {
        private string _lastText;

        public ClipboardMonitor()
        {
            _lastText = string.Empty;
        }

        public event Action<string> ClipboardTextChanged;

        public async Task StartMonitoringAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                string currentText = await ClipboardService.GetTextAsync();

                if (currentText != _lastText)
                {
                    _lastText = currentText;
                    OnClipboardTextChanged(currentText);
                }

                await Task.Delay(500, cancellationToken); // Poll every 500ms
            }
        }

        protected virtual void OnClipboardTextChanged(string newText)
        {
            ClipboardTextChanged?.Invoke(newText);
            Console.WriteLine($"Clipboard changed: {newText}");
        }

        public string GetLastClipboardText()
        {
            return _lastText;
        }
    }
}
