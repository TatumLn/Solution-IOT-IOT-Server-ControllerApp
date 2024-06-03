using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;
using MQTTnet.Client;
using System.Collections.ObjectModel;
using MQTTnet.Certificates;
using IOT_Controller.GetIP;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Text.Json;

namespace IOT_Controller.API
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly CommunicationService _mqttService;
        private static MainViewModel? _instance;
        public static MainViewModel Instance => _instance ??= new MainViewModel();
        public ObservableCollection<string> DataTroisIndicateur { get; set; }
        //protected readonly CertificatMqtt _certificatMqtt;
        public string[] _topicTroisIndicateur = ["iot/temperature", "iot/luminosite", "iot/humidity"];

        public MainViewModel()
        {
            _mqttService = CommunicationService.Instance;
            Commands = new CommandeModels(_mqttService);
            //_certificatMqtt = new CertificatMqtt();
            DataTroisIndicateur =
            [
                "N/A", //Temperature
                "N/A", //Luminosite
                "N/A"  //Humidite
            ];
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
            int index = Array.IndexOf(_topicTroisIndicateur, topic);
            if (index >= 0)
            {
                try
                {
                    var jsonData = JsonDocument.Parse(payload);
                    if (jsonData.RootElement.TryGetProperty("value", out var valueElement))
                    {
                        DataTroisIndicateur[index] = valueElement.ToString();
                    }
                }
                catch (Exception ex)
                {
                    DataTroisIndicateur[index] = $"{ex.Message}";
                }
            }
        }

        public async Task<string> DataTopicStateAsync(string topic)
        {
            var payload = await _mqttService.GetMqttMessageAsync(topic);
            return payload ?? "0";
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
            IsLoading = true;
            // si avec certificat  caCertPath, clientCertPath, clientCertPassword
            var options = CreateMqttClientOptions(clientId, brokerAddress, port, username, password);
            LoadingMessage = "Initialisation de la connexion au broker MQTT...";
            await Task.Delay(500);
            await _mqttService.ConnectMqtt(options);

            //
            if (_mqttService.IsConnected)
            {
                LoadingMessage = "Connexion reussie. Abonnement aux topics ...";
                foreach (var topic in _topicTroisIndicateur)
                {
                    await _mqttService.SubscribeAsync(topic);
                }
                LoadingMessage = "Abonnement reussie. Pret.";
                await Task.Delay(1000);
            }
            else
            {
                LoadingMessage = " Echec de la connexion au broker MQTT...";
                await Task.Delay(2000);
            }
            IsLoading = false;
        }

        public async Task Disconnect()
        {
            await _mqttService.DisconnectMqtt();
        }

        //Recuperation de l'ip local du reseau sans fil
        public static string GetBrokerAdress()
        {
            return DependencyService.Get<IPAdressService>().GetLocalIPAdress();
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        public CommandeModels Commands { get; }
        public bool IsConnected => _mqttService.IsConnected;
        public string? ConnectingMessage => _mqttService.ConnectingMessage;
        public string? ErrorMessage => _mqttService.ErrorMessage;
        private string? _loadingMessage;
        private bool? _isLoading;

        public string? LoadingMessage
        {
            get { return _loadingMessage; }
            set 
            { 
                _loadingMessage = value;
                OnPropertyChanged();
            }
        }

        public bool? IsLoading
        {
            get { return _isLoading; }
            set 
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
