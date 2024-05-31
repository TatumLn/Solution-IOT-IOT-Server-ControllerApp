using IOT_Controller.API;
using Newtonsoft.Json.Linq;
using MQTTnet.Client;

namespace IOT_Controller.ControllersModels
{
    public class MainViewModel
    {

        protected readonly CommunicationService _communicationService;

        public MainViewModel()
        {
            _communicationService = new CommunicationService();
        }
        public MqttClientOptions CreateMqttClientOptions(string clientId, string brokerAddress, int port, string? username = null, string? password = null)
        {
            var optionsBuilder = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithTcpServer(brokerAddress, port)
                .WithCleanSession();

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                optionsBuilder.WithCredentials(username, password);
            }

            return optionsBuilder.Build();
        }

        public async Task Connect(string clientId, string brokerAddress, int port, string? username = null, string? password = null)
        {
            var options = CreateMqttClientOptions(clientId, brokerAddress, port, username, password);
            await _communicationService.ConnectMqtt(options);
        }

        public async Task Disconnect()
        {
            await _communicationService.DisconnectMqtt();
        }

        public bool IsConnected => _communicationService.IsConnected;
        public string? ConnectingMessage => _communicationService.ConnectingMessage;
        public string? ErrorMessage => _communicationService.ErrorMessage;
    }
}
