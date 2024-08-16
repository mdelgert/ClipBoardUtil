using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClipboardUtil.MqttSimple
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

            Console.WriteLine("Simple demo mqtt");

            //await ClientSubscribeSamples.SubscribeTopic(server, port, username, password, topic1);

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(i);
                await ClientSubscribeSamples.SendSimpleMessage(server, port, username, password, topic1);
                System.Threading.Thread.Sleep(1000);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
