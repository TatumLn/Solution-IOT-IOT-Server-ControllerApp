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

namespace IOT_Controller.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private readonly CommunicationService _communicationService;
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

            // Connecter le WebSocket
            Task.Run(async () =>
            {
                try
                {
                    Uri uri = new Uri($"ws://{_ipAdress}:3000"); // URL du serveur WebSocket
                    await _communicationService.ConnectWebSocket(uri);
                    // Mettre à jour ConnexionStatus en fonction de l'état de la connexion
                    ConnectionStatus = _communicationService.IsConnected ? _communicationService.ConnectingMessage : _communicationService.ErrorMessage;
                }
                catch (Exception)
                {
                    // Gérer toute exception survenue lors de la connexion au WebSocket
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

        private string _temperature;
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
    }
}
