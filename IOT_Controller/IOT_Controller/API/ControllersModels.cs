
using System.Security.Cryptography.X509Certificates;
using MQTTnet.Client;
using System.Collections.ObjectModel;
using MQTTnet.Certificates;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Text.Json;
using IOT_Controller.ViewsModels;

namespace IOT_Controller.API
{
    public class MainViewModel : BaseViewModel
    {
        private readonly CommunicationService _mqttService;
        private static MainViewModel? _instance;
        public static MainViewModel Instance => _instance ??= new MainViewModel();
        public CommunicationService MqttService => _mqttService;

        public MainViewModel()
        {
            _mqttService = CommunicationService.Instance;
            Commands = new CommandeModels(_mqttService);
            //_certificatMqtt = new CertificatMqtt();
            _mqttService.MqttTopicRecu += OnMqttTopicRecu;
        }

        [Obsolete]
        // si avec certificat string? caCertPath = null, string? clientCertPath = null, string? clientCertPassword = null
        public MqttClientOptions CreateMqttClientOptions
            (string clientId,
            string brokerAddress,
            int port,
            string? username = null,
            string? password = null)
        {
            var optionsBuilder = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithTcpServer(brokerAddress, port)
                .WithCleanSession();

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                optionsBuilder.WithCredentials(username, password);
            }

            /*  if (!string.IsNullOrEmpty(caCertPath) || !string.IsNullOrEmpty(clientCertPath))
              {
                  var tlsOptions = new MqttClientOptionsBuilderTlsParameters
                  {
                      UseTls = true,
                      CertificatesProvider = new CertificatMqtt(caCertPath, clientCertPath, clientCertPassword),
                      AllowUntrustedCertificates = false,
                      IgnoreCertificateChainErrors = false,
                      IgnoreCertificateRevocationErrors = false,
                      SslProtocol = System.Security.Authentication.SslProtocols.Tls12

                  };
                  optionsBuilder.WithTls(tlsOptions);
              } */

            return optionsBuilder.Build();
        }

        private void OnMqttTopicRecu(string topic, string payload)
        {
            //Override
        }

        [Obsolete]
        // si avec certificat string? caCertPath = null, string? clientCertPath = null, string? clientCertPassword = null
        public async Task Connect
            (string clientId,
            string brokerAddress,
            int port,
            string? username = null,
            string? password = null)
        {
            // si avec certificat  caCertPath, clientCertPath, clientCertPassword
            var options = CreateMqttClientOptions(clientId, brokerAddress, port, username, password);
            LoadingMessage = "Initialisation de la connexion au broker MQTT...";
            await Task.Delay(500);
            await _mqttService.ConnectMqtt(options);

            //
            if (_mqttService.IsConnected)
            {
                LoadingMessage = "Connexion reussie...";
                await Task.Delay(1000);
                LoadingMessage = "Chargement de la page home...";
            }
            else
            {
                LoadingMessage = " Echec de la connexion au broker MQTT...";
                await Task.Delay(1000);
            }
        }

        public async Task Disconnect()
        {
            await _mqttService.DisconnectMqtt();
        }


        public CommandeModels Commands { get; }
        public bool IsConnected => _mqttService.IsConnected;
        public string? ConnectingMessage => _mqttService.ConnectingMessage;
        public string? ErrorMessage => _mqttService.ErrorMessage;

        private string? _loadingMessage;
        public string? LoadingMessage
        {
            get { return _loadingMessage; }
            set 
            { 
                _loadingMessage = value;
                OnPropertyChanged();
                LoadingMessageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? LoadingMessageChanged;
    }
}
