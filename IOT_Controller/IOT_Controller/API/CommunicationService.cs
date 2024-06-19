using System;
using System.Collections.Generic;
using System.Linq;
using MQTTnet;
using MQTTnet.Client;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.API
{
    public class CommunicationService
    {
        private IMqttClient? _mqttClient;
        private MqttClientOptions _mqttOptions;
        private readonly object _lock = new();
        public bool _IsConnectingOrDisconnecting;
        public bool IsConnected { get; private set; }
        public event Action<string, string>? MqttTopicRecu;
        //Singleton
        private static CommunicationService? _instance;
        public static CommunicationService Instance => _instance ??= new CommunicationService();

        public CommunicationService()
        {
            InitializeMqttClient();
        }

        private void InitializeMqttClient()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            //connexion
            _mqttClient.ConnectedAsync += async e =>
            {
                lock (_lock)
                {
                    _IsConnectingOrDisconnecting = false;
                }
                IsConnected = true;
                await Task.CompletedTask;
            };

            //Deconnection
            _mqttClient.DisconnectedAsync += async e =>
            {
                lock (_lock)
                {
                    _IsConnectingOrDisconnecting |= false;
                }
                IsConnected = false;
                await Task.CompletedTask;

                // Tentative de reconnexion si l'utilisateur ne s'est pas déconnecté volontairement
                if (!_IsConnectingOrDisconnecting)
                {
                    await ReconnectAsync();
                }
            };

            //Reception des messages
            _mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                MqttTopicRecu?.Invoke(e.ApplicationMessage.Topic,e.ApplicationMessage.ConvertPayloadToString());
                return Task.CompletedTask;
            };
        }

        public async Task ConnectMqtt(MqttClientOptions options)
        {
            lock ( _lock) 
            {
                if (_IsConnectingOrDisconnecting)
                {
                    return;
                }
                _IsConnectingOrDisconnecting = true;
            }
            _mqttOptions = options;
            try
            {
                await _mqttClient.ConnectAsync(options, CancellationToken.None);
            }
            catch (Exception)
            {
                lock (_lock) 
                {
                    _IsConnectingOrDisconnecting = false;
                }
                IsConnected = false;
            }
        }

        public async Task DisconnectMqtt()
        {
            lock (_lock) 
            {
                if (!_IsConnectingOrDisconnecting) 
                {
                    return;
                }
                _IsConnectingOrDisconnecting = true;
            }
            try
            {
                await _mqttClient.DisconnectAsync();
                IsConnected = false;
            }
            catch (Exception)
            {
                lock (_lock)
                {
                    _IsConnectingOrDisconnecting = false;
                }
            }
            finally
            {
                lock (_lock)
                {
                    _IsConnectingOrDisconnecting = false;
                }
            }
        }

        //s'abonner au broker
        public async Task SubscribeAsync (string topic)
        {
            if (_mqttClient != null && _mqttClient.IsConnected)
            {
                await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
            }
        }

        //publier dans le broker
        public async Task PublishAsync (string topic, string payload)
        {
            if(_mqttClient != null && _mqttClient.IsConnected)
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .Build();
                await _mqttClient.PublishAsync(message, CancellationToken.None);
            }
        }

        //
        private async Task ReconnectAsync()
        {
            while (!IsConnected)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    await _mqttClient.ConnectAsync(_mqttOptions, CancellationToken.None);
                }
                catch (Exception)
                {

                }
            }
        }

        public async Task<string> GetMqttMessageAsync(string topic)
        {
            await Task.Delay(100);
            return "1";
        }

    }
}
