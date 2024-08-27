using System;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System.IO;
using System.Threading.Tasks;

namespace ClipboardUtil.BluetoothSender
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string deviceName = "ESP32_BT"; // The name of your ESP32 Bluetooth device

            BluetoothClient client = new BluetoothClient();
            BluetoothDeviceInfo device = null;

            // Discover devices
            var devices = client.DiscoverDevices();
            foreach (var d in devices)
            {
                Console.WriteLine($"Found device: {d.DeviceName}");
                if (d.DeviceName == deviceName)
                {
                    device = d;
                    break;
                }
            }

            if (device == null)
            {
                Console.WriteLine("Device not found.");
                return;
            }

            // Connect to the device
            client.Connect(device.DeviceAddress, BluetoothService.SerialPort);

            // Send data to the device 10 times with a 1-second delay between each message
            using (var stream = client.GetStream())
            using (var writer = new StreamWriter(stream, System.Text.Encoding.ASCII))
            {
                writer.AutoFlush = true;

                for (int i = 0; i < 1000000; i++)
                {
                    string message = $"Hello {i + 1}";
                    await writer.WriteLineAsync(message);
                    Console.WriteLine($"Sent: {message}");

                    await Task.Delay(1000); // 1-second delay
                }
            }

            client.Close();
        }
    }
}
