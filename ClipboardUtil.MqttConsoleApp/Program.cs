using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using Microsoft.Extensions.Configuration;

namespace ClipboardUtil.MqttConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Build configuration
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var mqttSettings = config.GetSection("MqttSettings").Get<MqttSettings>();

            var mqttFactory = new MqttFactory();
            var mqttClient = mqttFactory.CreateMqttClient();

            // Define MQTT client options
            var mqttOptions = new MqttClientOptionsBuilder()
                .WithClientId("ConsoleAppClient")
                .WithTcpServer(mqttSettings.Server, mqttSettings.Port)
                .WithCredentials(mqttSettings.Username, mqttSettings.Password)
                .WithCleanSession()
                .Build();

            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine($"Received message: {payload} on topic: {e.ApplicationMessage.Topic}");
                return Task.CompletedTask;
            };

            mqttClient.ConnectedAsync += async e =>
            {
                Console.WriteLine("Connected successfully with MQTT Broker.");

                // Subscribe to a topic
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(mqttSettings.Topic).Build());
                Console.WriteLine($"Subscribed to topic '{mqttSettings.Topic}'.");

                var random = new Random();

                for (int i = 0; i < mqttSettings.MessageCount; i++)
                {
                    var messageValue = random.Next(1, 1000); // Generate a random message
                    var message = new MqttApplicationMessageBuilder()
                        .WithTopic(mqttSettings.Topic)
                        .WithPayload($"Random message: {messageValue}")
                        .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                        .WithRetainFlag(false)
                        .Build();

                    await mqttClient.PublishAsync(message);
                    Console.WriteLine($"Published message '{messageValue}' to topic '{mqttSettings.Topic}'.");

                    await Task.Delay(1000); // Wait for 1 second between messages
                }
            };

            mqttClient.DisconnectedAsync += e =>
            {
                Console.WriteLine("Disconnected from MQTT Broker.");
                return Task.CompletedTask;
            };

            // Connect to the MQTT broker
            await mqttClient.ConnectAsync(mqttOptions);

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            // Disconnect the MQTT client
            await mqttClient.DisconnectAsync();
        }
    }

    public class MqttSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Topic { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int MessageCount { get; set; }
    }
}
