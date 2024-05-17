using IOT_Controller.API;
using Newtonsoft.Json.Linq;
using DotNetEnv;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace IOT_Controller.ControllersModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        protected readonly CommunicationService _communicationService;
      
        private string? _ipAdress;
        private string? _connexionStatus;
        public string? ConnectionStatus
        {
            get { return _connexionStatus; }
            set
            {
                _connexionStatus = value;
                OnPropertyChanged(nameof(ConnectionStatus));
            }
        }

        public MainViewModel()
        {
            string envFilePath = "./.env";
            DotNetEnv.Env.Load(envFilePath);
            _ipAdress = Environment.GetEnvironmentVariable("IP_ADDRESS");
            _communicationService = new CommunicationService();
            _communicationService.DataReceived += CommunicationService_DataReceived;
            _humidity = "";
            _temperature = "";
            _luminosite = "";
            _climActif = "";
            _deshumidActif = "";
            _lumiereActif = "";
            PropertyChanged = delegate { }; // Initialiser l'événement PropertyChanged

            // WebSocket
            Task.Run(async () =>
            {
                try
                {
                    Uri uri = new Uri($"ws:// 192.168.0.200:3000"); //
                    await _communicationService.ConnectWebSocket(uri);
                    // Etat de la connexion
                    ConnectionStatus = _communicationService.IsConnected ? _communicationService.ConnectingMessage : _communicationService.ErrorMessage;
                }
                catch (Exception)
                {
                    // Exception au WebSocket
                    ConnectionStatus = _communicationService.ErrorMessage;
            }
            });
        }

        private void CommunicationService_DataReceived(object? sender, IOT_Controller.API.DataReceivedEventArgs e)
        {
            try
            {
                // Désérialiser les données JSON
                JObject responseJson = JObject.Parse(e.Data);

                // Extraire la température et l'humidité du JSON
                double temperature = responseJson.Value<double>("temperature");
                double humidity = responseJson.Value<double>("humidity");
                double luminosite = responseJson.Value<double>("luminosite");
                string? climActif = responseJson.Value<string>("climActif");
                string? deshumidActif = responseJson.Value<string>("deshumidActif");
                string? lumiereActif = responseJson.Value<string>("lumiereActif");

                // Mettre à jour les propriétés Temperature et Humidity
                Temperature = temperature.ToString();
                Humidity = humidity.ToString();
                Luminosite = luminosite.ToString();
                ClimActif = climActif??("Null");
                DeshumidActif = deshumidActif??("Null");
                LumiereActif = lumiereActif ?? ("Null");    

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'analyse des données JSON : " + ex.Message);
            }
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected string _temperature;
        public string Temperature
        {
            get { return _temperature; }
            set { SetProperty(ref _temperature, value); }
        }

        private string _humidity;
        public string Humidity
        {
            get { return _humidity; }
            set { SetProperty(ref _humidity, value); }
        }

        private string _luminosite;
        public string Luminosite
        {
            get { return _luminosite; }
            set { SetProperty(ref _luminosite, value); }
        }

        private string _climActif;
        public string ClimActif
        {
            get => _climActif;
            set { SetProperty(ref _climActif, value); }
        }

        private string _deshumidActif;
        public string DeshumidActif
        {
            get => _deshumidActif;
            set { SetProperty(ref _deshumidActif, value); }
        }

        private string _lumiereActif;
        public string LumiereActif
        {
            get => _lumiereActif;
            set { SetProperty(ref _lumiereActif, value); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

     

        private async Task DisactivationDevice()
        {
            try
            {
                await _communicationService.SendDesactivationCommand();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la désactivation de l'appareil : " + ex.Message);
            }
        }
    }

    public class ButtonData : INotifyPropertyChanged
    {
        public string? ButtonText { get; set; }
        public ICommand? ButtonCommand { get; set; }
        public Color? BackgroundColor { get; set; }
        public String? ButtonImage { get; set; }
        public int ButtonIndex { get; set; }
        public string? ButtonState { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public void UpdateBackgroundColor()
        {
            if (ButtonState == "1")
            {
                // Mettre à jour l'arrière-plan en noir si ButtonState est vrai
                BackgroundColor = Color.FromArgb("000000");
            }else if(ButtonState == "")
                {
                ButtonState = "Vrai";
            }
            // Ajoutez d'autres conditions pour d'autres états si nécessaire
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ComportementModels : MainViewModel
    {
        public ObservableCollection<ButtonData> ButtonList { get; set; }
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

        public void InitializeButtons()
        {
           
        }

        public ComportementModels() : base ()
        {
            ButtonList = new ObservableCollection<ButtonData>();
            AddButtonCommand = new Command(AddButton);
            Index = 1;
            // InitializeButtons est appelé après la réception des données du WebSocket
            _communicationService.DataReceived += (sender, args) => InitializeButtons();
            ButtonList.Add(new ButtonData { ButtonText = "Climatiseur", ButtonIndex = Index++, ButtonState = Temperature, ButtonImage = "climatiseur_icon.svg"});
            ButtonList.Add(new ButtonData { ButtonText = "Deshumidificateur", ButtonIndex = Index++, ButtonState = DeshumidActif, ButtonImage = "deshumidificateur_icon.svg" });
            ButtonList.Add(new ButtonData { ButtonText = "Lumiere", ButtonIndex = Index++, ButtonState = LumiereActif, ButtonImage = "led_icon.svg" });
            ButtonList.Add(new ButtonData { ButtonText = "Add", ButtonIndex = Index++ });
            // Associer la méthode OnButtonClick à chaque bouton de la liste
            foreach (var button in ButtonList)
            {
                button.ButtonCommand = new Command(() => UpdateSelectedContent(button.ButtonIndex));
                button.UpdateBackgroundColor();
            }
            SelectedContent = "Sélectionnez un bouton pour afficher le contenu de réglage";
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
                    SelectedContent = "Contenu spécifique du Climatisseur";
                    break;
                case 2:
                    SelectedContent = "Contenu spécifique du Deshumidificateur";
                    break;
                case 3:
                    SelectedContent = "Contenu spécifique de la Lumiere";
                    break;
                default:
                    SelectedContent = "Sélectionnez un bouton pour afficher le contenu de réglage";
                    break;
            }
        }

        private void AddButton()
        {
            // Logique pour ajouter un nouveau bouton
        }
    }
}
