using MQTTnet.Client;
using MQTTnet.Protocol;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ClipboardUtil.MqttSimple
{
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

                // Create an instance of the model
                var model = new MessageModel 
                { 
                    Msg = "helloX" 
                };

                // Convert the model object to JSON
                var payload = JsonSerializer.Serialize(model);

                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    //.WithPayload("{\"msg\": \"hello\"}")
                    .WithPayload(payload)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build();

                await mqttClient.PublishAsync(message, CancellationToken.None);

                Console.WriteLine($"Message sent to topic: {topic}");
            }
        }

        //public static async Task SendSimpleMessage(string server, int port, string username, string password, string topic)
        //{
        //    var mqttFactory = new MqttFactory();

        //    using (var mqttClient = mqttFactory.CreateMqttClient())
        //    {
        //        var mqttClientOptions = new MqttClientOptionsBuilder()
        //            .WithTcpServer(server, port)
        //            .WithCredentials(username, password)
        //            .Build();

        //        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        //        var message = new MqttApplicationMessageBuilder()
        //            .WithTopic(topic)
        //            .WithPayload("{\"msg\": \"helloX\"}")
        //            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
        //            .Build();

        //        await mqttClient.PublishAsync(message, CancellationToken.None);

        //        Console.WriteLine($"Message sent to topic: {topic}");
        //    }
        //}

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
