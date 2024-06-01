using IOT_Controller.API;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;
using MQTTnet.Client;
using System.Collections.ObjectModel;
using MQTTnet.Certificates;
using IOT_Controller.GetIP;

namespace IOT_Controller.ControllersModels
{
    public class MainViewModel
    {

        protected readonly CommunicationService _communicationService;
        //protected readonly CertificatMqtt _certificatMqtt;
        private readonly string[] _topicTroisIndicateur = ["iot/temperature", "iot/luminosite", "iot/humidite"];
        private readonly MqttService _mqttService;
        public ObservableCollection<string> DataTroisIndicateur { get; set; }

        public MainViewModel()
        {
            _communicationService = new CommunicationService();
            _mqttService = MqttService.Instance;
            //_certificatMqtt = new CertificatMqtt();
            DataTroisIndicateur = new ObservableCollection<string>
            {
                "N/A", //Temperature
                "N/A", //Luminosite
                "N/A"  //Humidite
            };
            _communicationService.MqttTopicRecu += OnMqttTopicRecu;
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
                DataTroisIndicateur[index] = payload;
            }
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
            await _communicationService.ConnectMqtt(options);

            //
            if (_communicationService._IsConnected)
            {
                foreach (var topic in _topicTroisIndicateur) 
                {
                    await _communicationService.SubscribeAsync(topic);
                }
            }
        }

        public async Task Disconnect()
        {
            await _communicationService.DisconnectMqtt();
        }

        //Recuperation de l'ip local du reseau sans fil
        public string GetBrokerAdress()
        {
            return DependencyService.Get<IPAdressService>().GetLocalIPAdress();
        }

        public bool IsConnected => _communicationService._IsConnected;
        public string? ConnectingMessage => _communicationService.ConnectingMessage;
        public string? ErrorMessage => _communicationService.ErrorMessage;
    }
}
