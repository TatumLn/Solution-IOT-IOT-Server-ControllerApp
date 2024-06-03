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
        private readonly MqttClientOptions? _mqttOptions;
        private readonly object _lock = new();
        public bool _IsConnectingOrDisconnecting;
        public bool IsConnected { get; private set; }
        private string? _errorMessage;
        private string? _connectingMessage;
        public event Action<string, string>? MqttTopicRecu;
        //Singleton
        private static CommunicationService? _instance;
        public static CommunicationService Instance => _instance ??= new CommunicationService();

        public CommunicationService()
        {
            InitializeMqttClient();
        }

        public string? ConnectingMessage
        {
            get { return _connectingMessage; }
            private set
            {
                _connectingMessage = value;
            }
        }

        public string? ErrorMessage
        {
            get { return _errorMessage; }
            private set
            {
                _errorMessage = value;
            }
        }

        private void InitializeMqttClient()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            _mqttClient.ConnectedAsync += async e =>
            {
                lock (_lock)
                {
                    _IsConnectingOrDisconnecting = false;
                }
                IsConnected = true;
                ConnectingMessage = "Connexion au broker MQTT reussie!";
                await Task.CompletedTask;
            };

            _mqttClient.DisconnectedAsync += async e =>
            {
                lock (_lock)
                {
                    _IsConnectingOrDisconnecting |= false;
                }
                IsConnected = false;
                ConnectingMessage = "Deconnection au brokerMQTT reussie!";
                await Task.CompletedTask;

                //Tentative de reconnexion si l'utilisateur ne s'est pas deconnecte volontairement
                await ReconnectAsync();
            };

            _mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                MqttTopicRecu?.Invoke(e.ApplicationMessage.Topic,
                    e.ApplicationMessage.ConvertPayloadToString());
                return Task.CompletedTask;
            };
        }

        public async Task ConnectMqtt(MqttClientOptions options)
        {
            lock ( _lock) 
            {
                if (_IsConnectingOrDisconnecting)
                {
                    ErrorMessage = "Une Operation de connexion ou de deconnexion est deja en cours";
                    return;
                }
                _IsConnectingOrDisconnecting = true;
            }
            try
            {
                await _mqttClient.ConnectAsync(options, CancellationToken.None);
            }
            catch (Exception ex)
            {
                lock (_lock) 
                {
                    _IsConnectingOrDisconnecting = false;
                }
                ErrorMessage = "Erreur de connexion au broker MQTT : " + ex.Message;
                IsConnected = false;
            }
        }

        public async Task DisconnectMqtt()
        {
            lock (_lock) 
            {
                if (!_IsConnectingOrDisconnecting) 
                {
                    ErrorMessage = "Une Operation de connexion ou de deconnexion est deja en cours.";
                    return;
                }
                _IsConnectingOrDisconnecting = true;
            }
            try
            {
                await _mqttClient.DisconnectAsync();
                IsConnected = false;
            }
            catch (Exception ex)
            {
                lock (_lock)
                {
                    _IsConnectingOrDisconnecting = false;
                }
                ErrorMessage = "Erreur de déconnexion du broker MQTT : " + ex.Message;
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
