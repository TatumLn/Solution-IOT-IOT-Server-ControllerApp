﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        public string[] _topicStateAppareil = ["iot/climActif", "iot/deshumidActif", "iot/lumiereActif"];
        public string _topicVirtuel = "iot/ValeurControler";
        public int Index { get; set; }

        private string? _selectedContent;
        public string? SelectedContent
        {
            get { return _selectedContent; }
            set
            {
                _selectedContent = value;
                OnPropertyChanged(nameof(SelectedContent));
            }
        }

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
        public int NbrBtn => (ButtonList.Count) - 1;
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
        }

        private async void InitializeButtons()
        {
            ButtonList.Add(new BoutonViewModel { ButtonText = "Climatiseur", ButtonIndex = Index++, ButtonImage = "icon_climatisation.png" });
            ButtonList.Add(new BoutonViewModel { ButtonText = "Deshumidificateur", ButtonIndex = Index++, ButtonImage = "icon_deshumidificateur.png" });
            ButtonList.Add(new BoutonViewModel { ButtonText = "Lumiere", ButtonIndex = Index++, ButtonImage = "icon_lampe.png" });
            ButtonList.Add(new BoutonViewModel { ButtonText = "Add", ButtonIndex = Index++, ButtonImage = "icon_ajouter.png" });

            foreach (var button in ButtonList)
            {
                button.ButtonCommand = new Command(async () => await UpdateSelectedContentAsync(button.ButtonIndex));
                if (button.ButtonIndex == _topicStateAppareil.Length)
                {
                    var state = await DataTopicStateAsync(_topicStateAppareil[button.ButtonIndex - 1]);
                    button.UpdateButtonState(state);
                }
                button.UpdateBackgroundColor();
            }

            OnPropertyChanged(nameof(NbrBtn));
            OnPropertyChanged(nameof(NbrBtnActive));
        }

        private async Task UpdateSelectedContentAsync(int buttonIndex)
        {
            foreach (var button in ButtonList)
            {
                button.IsSelected = button.ButtonIndex == buttonIndex;
                button.UpdateBackgroundColor();
            }
            // Met à jour le contenu en fonction de l'index du bouton sélectionné
            if (buttonIndex > 0 && buttonIndex <= 3)
            {
                SelectedContent = await DataTopicStateAsync(_topicVirtuel);
            }
        }

        public async Task<string> DataTopicStateAsync(string topic)
        {
            var payload = await _mqttConnexion.MqttService.GetMqttMessageAsync(topic);
            return payload ?? "0";
        }

        public int GetNbrBtnActive()
        {
            var BtnActive = ButtonList.Where(button => button.ButtonState == "actif");
            return BtnActive.Count();
        }

        private void AddButton()
        {
            // Logique pour ajouter un nouveau bouton
        }

        private void OnMqttTopicRecu(string topic, string payload)
        {
        }
    }
}
