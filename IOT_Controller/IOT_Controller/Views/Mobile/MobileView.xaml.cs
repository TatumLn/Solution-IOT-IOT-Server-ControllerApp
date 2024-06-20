using IOT_Controller.API;
using IOT_Controller.ViewsModels;
using IOT_Controller.GetipGetnotification;
//using static UIKit.UIGestureRecognizer;

namespace IOT_Controller.Views.Mobile
{

    public partial class MobileView : BaseContentPage
    {
        public MobileView()
	    {           
            InitializeComponent();
            _mqttConnexion.LoadingMessageChanged += OnLoadingMessageChanged;
        }

        private void OnLoadingMessageChanged(object? sender, EventArgs e)
        {
            notification.ShowLoading(_mqttConnexion.LoadingMessage ?? "...");
        }

        [Obsolete]
        private async void BtnConnexion(object sender, EventArgs e)
        {
            
            //Conexion au broker en local (par defaut)
            string brokerAddress = "192.168.3.178";//ip.GetLocalIPAdress();
            int port = 1883;
            //Username et Password par defaut du HiveMQ broker Community
            string username = "admin-user";
            string password = "admin-password";
            string clientId = $"AppMAUI : {username}";

            /* Chemins des certificats si avec certificat
            string caCertPath = "D:\\Projet_Licence\\IOT_Controller\\IOT_Controller\\Certificats\\hivemq-server-cert.pem"; 
            string clientCertPath = "D:\\Projet_Licence\\IOT_Controller\\IOT_Controller\\Certificats\\mqtt-client-cert.pem"; 
            string clientCertPassword = "D:\\Projet_Licence\\IOT_Controller\\IOT_Controller\\Certificats\\mqtt-client-key.pem"; */

            // si avec certificat caCertPath, clientCertPath, clientCertPassword
            await _mqttConnexion.Connect(clientId, brokerAddress, port, username, password);

            if (_mqttConnexion.IsConnected)
            {
                //Authentification réussie, génération et envoi du token à Home Assistant
                        await Navigation.PushAsync(new MobileView_Home());
              
            }
            else 
            {
                notification.ShowNotification($"Serveur indisponible");
            }
            //
            notification.HideLoading();
            await _mqttConnexion.Disconnect();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _mqttConnexion.LoadingMessageChanged -= OnLoadingMessageChanged;
        }

    }   
}