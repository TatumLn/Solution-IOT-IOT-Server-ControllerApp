using System;
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
        public int NbrBtn => ((ButtonList.Count) - 1);
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

        public async Task InitializeButtons()
        {
            ButtonList.Add(new BoutonViewModel { ButtonText = "Climatiseur", ButtonIndex = Index++, ButtonImage = "" });
            ButtonList.Add(new BoutonViewModel { ButtonText = "Deshumidificateur", ButtonIndex = Index++, ButtonImage = "" });
            ButtonList.Add(new BoutonViewModel { ButtonText = "Lumiere", ButtonIndex = Index++, ButtonImage = "" });
            ButtonList.Add(new BoutonViewModel { ButtonText = "Add", ButtonIndex = Index++, ButtonImage = "" });

            foreach (var button in ButtonList)
            {
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

  /*#######################################################################################################################################################################
   *  Classe Bouton 
   *  ####################################################################################################################################################################*/

    public class BoutonViewModel : BaseViewModel
    {
        public string? ButtonText { get; set; }
        public ICommand? ButtonCommand { get; set; }
        public String? ButtonImage { get; set; }
        public int ButtonIndex { get; set; }
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

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
                UpdateBackgroundColor();
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

        public void UpdateBackgroundColor()
        {
            // Mettre à jour l'arrière-plan
            BorderBackgroundColor = IsSelected ? "#EAFD0E" : "#90EE90";
        }

        public void UpdateButtonState(string state)
        {
            ButtonState = state == "1" ? "actif" : "inactif";
        }

    }
}
