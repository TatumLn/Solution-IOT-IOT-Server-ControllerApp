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
using Microsoft.Maui.Controls.Shapes;

namespace IOT_Controller.ControllersModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        protected readonly CommunicationService _communicationService;

        private CapteurData? _capteurData;
        public CapteurData CapteurData
        {
            get { return _capteurData; }
            set { SetProperty(ref _capteurData, value); }
        }

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

        public ICommand SendDesactivationCommand { get; }

        public MainViewModel()
        {
            CapteurData = new CapteurData();
            string envFilePath = "./.env";
            DotNetEnv.Env.Load(envFilePath);
            _ipAdress = Environment.GetEnvironmentVariable("IP_ADDRESS");
            _communicationService = new CommunicationService();
            _communicationService.DataReceived += CommunicationService_DataReceived;
            PropertyChanged = delegate { }; // Initialiser l'événement PropertyChanged
            SendDesactivationCommand = new Command(async () => await _communicationService.SendDesactivationCommand());

            // WebSocket
            Task.Run(async () =>
            {
                try
                {
<<<<<<< HEAD
<<<<<<< HEAD
                    Uri uri = new Uri($"ws://{_ipAdress}:3000"); //
=======
                    Uri uri = new Uri($"ws://192.168.0.200:3000"); //
>>>>>>> TatumLn
=======
                    Uri uri = new Uri($"ws://{_ipAdress}:3000"); //
>>>>>>> TatumLn
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
                CapteurData.Temperature = responseJson.Value<double>("temperature").ToString();
                CapteurData.Humidity = responseJson.Value<double>("humidity").ToString();
                CapteurData.Luminosite = responseJson.Value<double>("luminosite").ToString();
                CapteurData.ClimActif = responseJson.Value<string>("climActif") ?? "Null";
                CapteurData.DeshumidActif = responseJson.Value<string>("deshumidActif") ?? "Null";
                CapteurData.LumiereActif = responseJson.Value<string>("lumiereActif") ?? "Null";
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
}
