using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IOT_Controller.API;
using System.Text.Json;

namespace IOT_Controller.ViewsModels
{
    public class TroisCercleIndicateurViewModel: BaseViewModel
    {
        private MainViewModel _mqttConnexion;
        public ObservableCollection<string> NomTroisIndicateur { get; set; }
        public ObservableCollection<string> DataTroisIndicateur { get; set; }
        //protected readonly CertificatMqtt _certificatMqtt;
        private string[] _topicTroisIndicateur = ["iot/temperature", "iot/luminosite", "iot/humidite"];

        private double _currentValue;
        public double CurrentValue
        {
            get => _currentValue;
            set
            {
                if (_currentValue != value)
                {
                    _currentValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public TroisCercleIndicateurViewModel() 
        {
            _mqttConnexion = MainViewModel.Instance;
            _mqttConnexion.MqttService.MqttTopicRecu += OnMqttTopicRecu;
            DataTroisIndicateur = ["N/A", "N/A", "N/A"];
            NomTroisIndicateur = ["__", "__", "__"];
            Task.Run(async () => await SubscribeToTopics());
        }

        private async Task SubscribeToTopics()
        {
            foreach (var topic in GetSubscriptionTopics())
            {
                await _mqttConnexion.MqttService.SubscribeAsync(topic);
            }
        }

        private List<string> GetSubscriptionTopics()
        {
            return [.. _topicTroisIndicateur];
        }

        private void OnMqttTopicRecu(string topic, string payload)
        {
            int index = Array.IndexOf(_topicTroisIndicateur, topic);
            if (index >= 0)
            {
                try
                {
                    var jsonData = JsonDocument.Parse(payload);
                    if (jsonData.RootElement.TryGetProperty("nom", out var nomElement) && jsonData.RootElement.TryGetProperty("valeur", out var valueElement))
                    {
                        DataTroisIndicateur[index] = valueElement.ToString();
                        NomTroisIndicateur[index] = nomElement.ToString();
                        OnPropertyChanged(nameof(DataTroisIndicateur));
                        OnPropertyChanged(nameof(NomTroisIndicateur));

                        if (double.TryParse(valueElement.ToString(), out double newValue))
                        {
                            CurrentValue = newValue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    DataTroisIndicateur[index] = $"{ex.Message}";
                    NomTroisIndicateur[index] = $"{ex.Message}";
                    OnPropertyChanged(nameof(DataTroisIndicateur));
                    OnPropertyChanged(nameof(NomTroisIndicateur));
                }
            }
        }
    }
}
