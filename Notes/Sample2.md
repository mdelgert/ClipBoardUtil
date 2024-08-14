using System;
using System.Threading;
using TextCopy;

class ClipboardPoller
{
    static void Main()
    {
        string lastText = ClipboardService.GetText();

        while (true)
        {
            string currentText = ClipboardService.GetText();
            if (currentText != lastText)
            {
                Console.WriteLine("Clipboard updated: " + currentText);
                lastText = currentText;
            }

            // Sleep for a short interval before checking again
            Thread.Sleep(500);
        }
    }
}
