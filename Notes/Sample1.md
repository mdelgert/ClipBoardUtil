using System;
using TextCopy;

class ClipboardUtility
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: ClipboardUtility <text>");
            return;
        }

        // Combine all arguments into a single string
        string textToCopy = string.Join(" ", args);

        // Copy text to clipboard
        ClipboardService.SetText(textToCopy);
        Console.WriteLine($"Copied to clipboard: {textToCopy}");

        // Read text from clipboard
        string clipboardText = ClipboardService.GetText();
        Console.WriteLine($"Read from clipboard: {clipboardText}");
    }
}
