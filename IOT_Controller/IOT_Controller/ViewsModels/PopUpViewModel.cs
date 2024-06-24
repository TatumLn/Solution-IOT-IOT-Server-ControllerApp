using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using IOT_Controller.API;

namespace IOT_Controller.ViewsModels
{
    public class PopUpViewModel : BaseViewModel
    {
        private MainViewModel _mqttConnexion;
        public ObservableCollection<BoutonViewModel> ButtonList { get; set; }
        public ICommand AddButtonCommand { get; }
        public string _topicVirtuel = "iot/ValeurControler";
        public int Index { get; set; }

        private int _selectedButtonIndex;
        public int SelectedButtonIndex
        {
            get { return _selectedButtonIndex; }
            set
            {
                _selectedButtonIndex = value;
                OnPropertyChanged(nameof(SelectedButtonIndex));
            }
        }

        //Compteur 
        public int NbrBtn => ButtonList.Count(b => b.ButtonText != "Ajout");
        public int NbrBtnActive => GetNbrBtnActive();

        public PopUpViewModel()
        {
            _mqttConnexion = MainViewModel.Instance;
            _mqttConnexion.MqttService.MqttTopicRecu += OnMqttTopicRecu;
            ButtonList = [];
            AddButtonCommand = new Command(AddButton);
            Index = 1;
            // Initialisation des boutons
            Task.Run(() => InitializeButtons());
            Task.Run(async () => await SubscribeToTopics());
        }

        public async Task InitializeButtons()
        {
            ButtonList.Add(new BoutonViewModel { ButtonText = "Climatiseur", ButtonIndex = Index++, ButtonImage = "air_conditioner.png", MqttTopic = "iot/climActif", ButtonState = "inactif" });
            ButtonList.Add(new BoutonViewModel { ButtonText = "humidificateur", ButtonIndex = Index++, ButtonImage = "humidifier.png", MqttTopic = "iot/deshumidActif", ButtonState = "inactif" });
            ButtonList.Add(new BoutonViewModel { ButtonText = "Lumiere", ButtonIndex = Index++, ButtonImage = "light.png", MqttTopic = "iot/lumiereActif", ButtonState = "inactif" });
            ButtonList.Add(new BoutonViewModel { ButtonText = "Arrosoir", ButtonIndex = Index++, ButtonImage = "plant_pot.png", MqttTopic = "iot/arrosoirActif", ButtonState = "inactif" });
            ButtonList.Add(new BoutonViewModel { ButtonText = "Ajout", ButtonIndex = Index++, ButtonImage = "add.png" });

            OnPropertyChanged(nameof(NbrBtn));
            OnPropertyChanged(nameof(NbrBtnActive));
            await Task.CompletedTask;
        }

        private async Task SubscribeToTopics()
        {
            foreach (var topic in GetSubscriptionTopics())
            {
                await _mqttConnexion.MqttService.SubscribeAsync(topic??"");
            }
        }

        private List<string?> GetSubscriptionTopics()
        {
            return ButtonList.Select(b => b.MqttTopic).Where(t => !string.IsNullOrEmpty(t)).ToList();
        }


        public int GetNbrBtnActive()
        {
            return ButtonList.Count(button => button.ButtonState == "actif");
        }

        private void AddButton()
        {
            // Logique pour ajouter un nouveau bouton
            OnPropertyChanged(nameof(NbrBtn));
        }

        private void OnMqttTopicRecu(string topic, string payload)
        {
            try
            {
                // Parse le payload JSON
                var jsonDoc = JsonDocument.Parse(payload);
                var root = jsonDoc.RootElement;
                if (root.TryGetProperty("valeur", out var valeurElement))
                {
                    var valeur = valeurElement.GetString();
                    var button = ButtonList.FirstOrDefault(b => b.MqttTopic == topic);
                    if (button != null)
                    {
                        button.UpdateButtonState(valeur??"0");
                        OnPropertyChanged(nameof(NbrBtnActive));
                    }
                }
            }
            catch (JsonException ex)
            {
                // Gérer l'exception si le JSON est mal formé
                Console.WriteLine($"Erreur de parsing JSON: {ex.Message}");
            }
        }
    }

  /*#######################################################################################################################################################################
   *  Classe Bouton 
   *  ####################################################################################################################################################################*/

    public class BoutonViewModel : BaseViewModel
    {
        public string? ButtonText { get; set; }
        public ICommand? ButtonCommand { get; set; }
        public String? ButtonImage { get; set; }
        public int ButtonIndex { get; set; }
        public string? MqttTopic { get; set; }
        public string? _buttonState;
        public string? ButtonState
        {
            get => _buttonState;
            set
            {
                _buttonState = value;
                OnPropertyChanged(nameof(ButtonState));
            }
        }


        private string? _borderBackgroundColor;
        public string? BorderBackgroundColor
        {
            get { return _borderBackgroundColor; }
            set
            {
                _borderBackgroundColor = value;
                OnPropertyChanged(nameof(BorderBackgroundColor));
            }
        }

        public void UpdateButtonState(string state)
        {
            ButtonState = state == "1" ? "actif" : "inactif";
        }

    }
}
