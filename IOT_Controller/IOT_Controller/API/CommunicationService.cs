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
        private MqttClientOptions? _mqttOptions;
        public bool IsConnected { get; private set; }
        private string? _errorMessage;
        private string? _connectingMessage;

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
                IsConnected = true;
                ConnectingMessage = "Connexion au broker MQTT reussie!";
                await Task.CompletedTask;
            };

            _mqttClient.DisconnectedAsync += async e =>
            {
                IsConnected = false;
                ConnectingMessage = "Deconnection au brokerMQTT reussie!";
                await Task.CompletedTask;
            };
        }

        public async Task ConnectMqtt(MqttClientOptions options)
        {
            try
            {
                await _mqttClient.ConnectAsync(options, CancellationToken.None);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Erreur de connexion au broker MQTT : " + ex.Message;
                IsConnected = false;
            }
        }

        public async Task DisconnectMqtt()
        {
            try
            {
                await _mqttClient.DisconnectAsync();
                IsConnected = false;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Erreur de déconnexion du broker MQTT : " + ex.Message;
            }
        }
    }
}
