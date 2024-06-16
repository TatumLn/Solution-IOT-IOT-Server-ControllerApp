using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IOT_Controller.API
{
    public class CommandeModels
    {
        private readonly CommunicationService _mqttService;
        public ICommand Augmenter { get; }
        public ICommand Diminuer {  get; }
        public ObservableCollection<string> NomTopic { get; }

        public CommandeModels(CommunicationService mqttService)
        {
            _mqttService = mqttService;
            NomTopic = ["iot/command/temperature", "iot/command/luminosite", "iot/command/humidite"];
            Augmenter = new Command<string>(async (topic) => await EnvoyerCommandAsync( topic, "Augmenter"));
            Diminuer = new Command<string>(async (topic) => await EnvoyerCommandAsync( topic, "Diminuer"));
        }

        private async Task EnvoyerCommandAsync(string topic, string command)
        {
            await _mqttService.PublishAsync(topic, command);
        }
    }
}
