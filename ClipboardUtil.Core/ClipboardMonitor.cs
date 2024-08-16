using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using TextCopy;

namespace ClipboardUtil.Core
{
    public class ClipboardMqttPublisher
    {
        private readonly string _broker;
        private readonly int _port;
        private readonly string _clientId;
        private readonly string _topic;
        private readonly string _username;
        private readonly string _password;
        private readonly string _certificatePath;
        private readonly IMqttClient _mqttClient;
        private readonly X509Certificate2Collection _caChain;
        private readonly ClipboardMonitor _clipboardMonitor;

        public ClipboardMqttPublisher(IConfiguration configuration)
        {
            var mqttSettings = configuration.GetSection("MqttSettings").Get<MqttSettings>();

            _broker = mqttSettings.Broker;
            _port = mqttSettings.Port;
            _clientId = mqttSettings.ClientId ?? Guid.NewGuid().ToString();
            _topic = mqttSettings.Topic;
            _username = mqttSettings.Username;
            _password = mqttSettings.Password;
            _certificatePath = mqttSettings.CertificatePath;

            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            _caChain = new X509Certificate2Collection();
            string cert = File.ReadAllText(_certificatePath);
            _caChain.ImportFromPem(cert);

            _clipboardMonitor = new ClipboardMonitor();
            _clipboardMonitor.ClipboardTextChanged += async (newText) => await PublishClipboardTextAsync(newText);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(_broker, _port)
                .WithCredentials(_username, _password)
                .WithClientId(_clientId)
                .WithCleanSession()
                .WithTlsOptions(o =>
                {
                    o.WithCertificateValidationHandler(_ => true);
                    o.WithSslProtocols(SslProtocols.Tls12);
                    o.WithTrustChain(_caChain);
                })
                .Build();

            var connectResult = await _mqttClient.ConnectAsync(options, cancellationToken);

            if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
            {
                Console.WriteLine("Connected to MQTT broker successfully.");
                await _clipboardMonitor.StartMonitoringAsync(cancellationToken);
            }
            else
            {
                Console.WriteLine($"Failed to connect to MQTT broker: {connectResult.ResultCode}");
            }
        }

        private async Task PublishClipboardTextAsync(string newText)
        {
            var messageModel = new MqttMessage
            {
                Id = DateTime.UtcNow.Ticks,
                Content = newText,
                Timestamp = DateTime.UtcNow
            };

            var payload = JsonSerializer.Serialize(messageModel);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(_topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .WithRetainFlag()
                .Build();

            await _mqttClient.PublishAsync(message);
            Console.WriteLine($"Published clipboard content to MQTT: {newText}");
        }
    }

    public class MqttSettings
    {
        public string Broker { get; set; }
        public int Port { get; set; }
        public string ClientId { get; set; }
        public string Topic { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string CertificatePath { get; set; }
    }

    public class MqttMessage
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }

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
