using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using IOT_Controller.ViewsModels;

namespace IOT_Controller.ControllersModels
{
    public class ContenuPopUp : INotifyPropertyChanged
    {
        public ObservableCollection<BoutonData> ButtonList { get; set; }
        public ICommand AddButtonCommand { get; }
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
        public int nbrBtn => (ButtonList.Count)-1;
        public int nbrBtnActive => GetNbrBtnActive();

        private MainViewModel _mainViewModel;

        public ContenuPopUp ()
        {
            _mainViewModel = new MainViewModel();
            ButtonList = new ObservableCollection<BoutonData>();
            AddButtonCommand = new Command(AddButton);
            Index = 1;
            // Initialisation des boutons
            Task.Run(() => InitializeButtons());
        }

        private void InitializeButtons()
        {
            ButtonList.Add(new BoutonData { ButtonText = "Climatiseur", ButtonIndex = Index++, ButtonImage = "climatiseur_icon.svg"});
                ButtonList.Add(new BoutonData { ButtonText = "Deshumidificateur", ButtonIndex = Index++, ButtonImage = "deshumidificateur_icon.svg"});
                ButtonList.Add(new BoutonData { ButtonText = "Lumiere", ButtonIndex = Index++, ButtonImage = "led_icon.svg"});
                ButtonList.Add(new BoutonData { ButtonText = "Add", ButtonIndex = Index++ });

                foreach (var button in ButtonList)
                {
                    button.ButtonCommand = new Command(() => UpdateSelectedContent(button.ButtonIndex));
                    button.UpdateBackgroundColor();
                }

                SelectedContent = $"{_mainViewModel.CapteurData.}";
            OnPropertyChanged(nameof(nbrBtn));
            OnPropertyChanged(nameof(nbrBtnActive));
        }

        private void UpdateSelectedContent(int buttonIndex) // Modification du type de paramètre
        {
            foreach (var button in ButtonList)
            {
                button.IsSelected = button.ButtonIndex == buttonIndex;
            }
            // Met à jour le contenu en fonction de l'index du bouton sélectionné
            switch (buttonIndex)
            {
                case 1:
                    SelectedContent = $"{_mainViewModel.CapteurData.Temperature}";
                    break;
                case 2:
                    SelectedContent = "Deshum";
                    break;
                case 3:
                    SelectedContent = "Lum";
                    break;
                default:
                    SelectedContent = "Nouveau";
                    break;
            }
        }

        public int GetNbrBtnActive()
        {
            var BtnActive = ButtonList.Where(button => button.ButtonState == "1");
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
