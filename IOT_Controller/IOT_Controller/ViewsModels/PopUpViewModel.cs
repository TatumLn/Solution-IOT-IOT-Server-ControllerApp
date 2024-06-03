using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using IOT_Controller.ViewsModels;
using IOT_Controller.API;


namespace IOT_Controller.ViewsModels
{
    public class PopUpViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<BoutonViewModel> ButtonList { get; set; }
        public ICommand AddButtonCommand { get; }
        private readonly CommandeModels _commandModels;
        private readonly MainViewModel _mainViewModel;
        public string[] _topicStateAppareil = ["iot/climActif", "iot/deshumidActif", "iot/lumiereActif"];
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
        public int NbrBtn => (ButtonList.Count)-1;
        public int NbrBtnActive => GetNbrBtnActive();

        public PopUpViewModel ()
        {
            _commandModels = new CommandeModels(new CommunicationService());
            _mainViewModel = new MainViewModel();
            ButtonList = [];
            AddButtonCommand = new Command(AddButton);
            Index = 1;
            // Initialisation des boutons
            Task.Run(() => InitializeButtons());
        }

        private async void InitializeButtons()
        {
            ButtonList.Add(new BoutonViewModel { ButtonText = "Climatiseur", ButtonIndex = Index++, ButtonImage = "icon_climatisation.png"});
            ButtonList.Add(new BoutonViewModel { ButtonText = "Deshumidificateur", ButtonIndex = Index++, ButtonImage = "icon_deshumidificateur.png"});
            ButtonList.Add(new BoutonViewModel { ButtonText = "Lumiere", ButtonIndex = Index++, ButtonImage = "icon_lampe.png"});
            ButtonList.Add(new BoutonViewModel { ButtonText = "Add", ButtonIndex = Index++, ButtonImage = "icon_ajouter.png" });

                foreach (var button in ButtonList)
                {
                    button.ButtonCommand = new Command(async () => await UpdateSelectedContentAsync(button.ButtonIndex));
                    if (button.ButtonIndex == _topicStateAppareil.Length)
                        {
                    var state = await _mainViewModel.DataTopicStateAsync(_topicStateAppareil[button.ButtonIndex - 1]);
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
               // SelectedContent = await _mainViewModel.DataTopicStateAsync();
            }
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
