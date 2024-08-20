using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MQTTConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // MQTT connection details
            string server = "localhost";
            int port = 1883;
            string username = "test";
            string password = "test";

            // Topics for subscription
            string topic1 = "mqttnet/samples/topic/1";
            string topic2 = "mqttnet/samples/topic/2";
            string topic3 = "mqttnet/samples/topic/3";

            // Choose the function to run based on the user's input.
            Console.WriteLine("Choose a sample to run:");
            Console.WriteLine("1. Handle Received Application Message");
            Console.WriteLine("2. Send Responses");
            Console.WriteLine("3. Subscribe to Multiple Topics");
            Console.WriteLine("4. Subscribe to a Topic");
            Console.WriteLine("5. Send simple message");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ClientSubscribeSamples.HandleReceivedApplicationMessage(server, port, username, password, topic2);
                    break;
                case "2":
                    await ClientSubscribeSamples.SendResponses(server, port, username, password, topic1);
                    break;
                case "3":
                    await ClientSubscribeSamples.SubscribeMultipleTopics(server, port, username, password, topic1, topic2, topic3);
                    break;
                case "4":
                    await ClientSubscribeSamples.SubscribeTopic(server, port, username, password, topic1);
                    break;
                case "5":
                    await ClientSubscribeSamples.SendSimpleMessage(server, port, username, password, topic1);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    public static class ClientSubscribeSamples
    {
        public static async Task SendSimpleMessage(string server, int port, string username, string password, string topic)
        {
            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(server, port)
                    .WithCredentials(username, password)
                    .Build();

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload("{\"msg\": \"helloX\"}")
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build();

                await mqttClient.PublishAsync(message, CancellationToken.None);

                Console.WriteLine($"Message sent to topic: {topic}");
            }
        }

        public static async Task HandleReceivedApplicationMessage(string server, int port, string username, string password, string topic)
        {
            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(server, port)
                    .WithCredentials(username, password)
                    .Build();

                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    Console.WriteLine("Received application message.");
                    e.DumpToConsole();

                    return Task.CompletedTask;
                };

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(f => f.WithTopic(topic))
                    .Build();

                await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                Console.WriteLine($"MQTT client subscribed to topic: {topic}");
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
            }
        }

        public static async Task SendResponses(string server, int port, string username, string password, string topic)
        {
            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                mqttClient.ApplicationMessageReceivedAsync += args =>
                {
                    // Do some work with the message...
                    args.ReasonCode = MqttApplicationMessageReceivedReasonCode.ImplementationSpecificError;
                    args.ResponseReasonString = "That did not work!";
                    args.ResponseUserProperties.Add(new MqttUserProperty("My", "Data"));

                    return Task.CompletedTask;
                };

                var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(server, port)
                    .WithCredentials(username, password)
                    .Build();

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(f => f.WithTopic(topic))
                    .Build();

                var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                Console.WriteLine($"MQTT client subscribed to topic: {topic}");
                response.DumpToConsole();
            }
        }

        public static async Task SubscribeMultipleTopics(string server, int port, string username, string password, string topic1, string topic2, string topic3)
        {
            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(server, port)
                    .WithCredentials(username, password)
                    .Build();

                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    Console.WriteLine($"Received message on topic {e.ApplicationMessage.Topic}: {System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                    return Task.CompletedTask;
                };

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(f => f.WithTopic(topic1))
                    .WithTopicFilter(f => f.WithTopic(topic2))
                    .WithTopicFilter(f => f.WithTopic(topic3))
                    //.WithTopicFilter(f => f.WithTopic(topic2).WithNoLocal())
                    //.WithTopicFilter(f => f.WithTopic(topic3).WithRetainHandling(MqttRetainHandling.SendAtSubscribe))
                    .Build();

                var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                Console.WriteLine("MQTT client subscribed to multiple topics:");
                Console.WriteLine($"- {topic1}");
                Console.WriteLine($"- {topic2}");
                Console.WriteLine($"- {topic3}");
                response.DumpToConsole();

                Console.WriteLine("Waiting for messages on subscribed topics. Press any key to exit...");

                Console.ReadKey();  // This line ensures the application stays open to receive messages.
            }
        }

        public static async Task SubscribeTopic(string server, int port, string username, string password, string topic)
        {
            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(server, port)
                    .WithCredentials(username, password)
                    .Build();

                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    Console.WriteLine($"Received message on topic {e.ApplicationMessage.Topic}: {System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                    return Task.CompletedTask;
                };

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(f => f.WithTopic(topic))
                    .Build();

                await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                Console.WriteLine($"MQTT client subscribed to topic: {topic}");
                Console.WriteLine("Waiting for messages. Press any key to exit...");

                Console.ReadKey();  // This line ensures the application stays open to receive messages.
            }
        }

    }
}
