
using System.Security.Cryptography.X509Certificates;
using MQTTnet.Client;
using System.Collections.ObjectModel;
using MQTTnet.Certificates;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Text.Json;
using IOT_Controller.ViewsModels;
using System.Windows.Input;

namespace IOT_Controller.API
{
    public class MainViewModel : BaseViewModel
    {
        private readonly CommunicationService _mqttService;
        private static readonly MainViewModel _instance = new();
        public static MainViewModel Instance => _instance;
        public CommunicationService MqttService => _mqttService;
        private readonly List<BaseContentView> _contentViews;
        private CancellationTokenSource _cancellationTokenSource = new();

        //Chart
        private bool _isChartVisible;
        public bool IsChartVisible
        {
            get => _isChartVisible;
            set
            {
                if (_isChartVisible != value)
                {
                    _isChartVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainViewModel()
        {
            _mqttService = CommunicationService.Instance;
            Commands = new CommandeModels(_mqttService);
            //_certificatMqtt = new CertificatMqtt();
            _mqttService.MqttTopicRecu += OnMqttTopicRecu;
            _contentViews = new List<BaseContentView>();
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
            //A Override
            foreach (var view in _contentViews)
            {
                view.OnMqttTopicRecu(topic, payload);
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
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;

            try
            {
                // si avec certificat  caCertPath, clientCertPath, clientCertPassword
                var options = CreateMqttClientOptions(clientId, brokerAddress, port, username, password);
                LoadingMessage = "Initialisation de la connexion au broker MQTT...";
                await Task.Delay(500, cancellationToken);
                await _mqttService.ConnectMqtt(options);

                //
                if (_mqttService.IsConnected)
                {
                    LoadingMessage = "Connexion reussie...";
                    await Task.Delay(1000, cancellationToken);
                    LoadingMessage = "Chargement de la page home...";
                    // Subscribe to topics for all content views
                    foreach (var view in _contentViews)
                    {
                        await view.SubscribeToTopics();
                    }
                }
                else
                {
                    LoadingMessage = " Echec de la connexion au broker MQTT...";
                    await Task.Delay(1000, cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                LoadingMessage = "Connexion annulée.";
            }
            catch (Exception ex)
            {
                LoadingMessage = $"Erreur: {ex.Message}";
            }
        }

        public async Task Disconnect()
        {
            _cancellationTokenSource.Cancel();
            await _mqttService.DisconnectMqtt();
        }


        public CommandeModels Commands { get; }
        public bool IsConnected => _mqttService.IsConnected;
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
        public void RegisterContentView(BaseContentView view)
        {
            _contentViews.Add(view);
        }

        public void UnregisterContentView(BaseContentView view)
        {
            _contentViews.Remove(view);
        }
    }
}
