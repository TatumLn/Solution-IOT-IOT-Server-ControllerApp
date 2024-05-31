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
        private readonly object _lock = new object();
        public bool _IsConnectingOrDisconnecting;
        public bool _IsConnected { get; private set; }
        private string? _errorMessage;
        private string? _connectingMessage;
        public event Action<string, string>? MqttTopicRecu;

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
                _IsConnected = true;
                ConnectingMessage = "Connexion au broker MQTT reussie!";
                await Task.CompletedTask;
            };

            _mqttClient.DisconnectedAsync += async e =>
            {
                lock (_lock)
                {
                    _IsConnectingOrDisconnecting |= false;
                }
                _IsConnected = false;
                ConnectingMessage = "Deconnection au brokerMQTT reussie!";
                await Task.CompletedTask;
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
                _IsConnected = false;
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
                _IsConnected = false;
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

        public async Task SubscribeAsync (string topic)
        {
            if (_mqttClient != null && _mqttClient.IsConnected)
            {
                await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
            }
        }
    }
}
