﻿using IOT_Controller.API;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private readonly CommunicationService _communicationService;
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
            _communicationService = new CommunicationService();
            _communicationService.DataReceived += CommunicationService_DataReceived;
            _humidity = "";
            _temperature = "";
            PropertyChanged = delegate { }; // Initialiser l'événement PropertyChanged

            // Connecter le WebSocket
            Task.Run(async () =>
            {
                try
                {
                    Uri uri = new Uri("ws://localhost:3000"); // URL du serveur WebSocket
                    await _communicationService.ConnectWebSocket(uri);
                    // Mettre à jour ConnexionStatus en fonction de l'état de la connexion
                    ConnectionStatus = _communicationService.isConnected ? "Connexion au serveur réussi!" : "Connexion au serveur échoué!";
                }
                catch (Exception ex)
                {
                    // Gérer toute exception survenue lors de la connexion au WebSocket
                    Console.WriteLine("Une erreur est survenue lors de la connexion au WebSocket : " + ex.Message);
                    // Si la connexion échoue, définir ConnectionStatus à "echoue"
                    ConnectionStatus = "Connexion au serveur échoué!";
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

                // Mettre à jour les propriétés Temperature et Humidity
                Temperature = temperature.ToString();
                Humidity = humidity.ToString();
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
            set
            {
                _temperature = value;
                OnPropertyChanged(nameof(Temperature));
            }
        }

        private string _humidity;
        public string Humidity
        {
            get { return _humidity; }
            set
            {
                _humidity = value;
                OnPropertyChanged(nameof(Humidity));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
