using IOT_Controller.API;
using IOT_Controller.ViewsModels;

namespace IOT_Controller.Views.Mobile
{

    public partial class MobileView : BaseContentPage
    {
        public MobileView()
	    {
            InitializeComponent();
        }

        [Obsolete]
        private async void BtnConnexion(object sender, EventArgs e)
        {
            //Conexion au broker en local (par defaut)
            string clientId = "ControlAppClient";
            string brokerAddress = " <AdressBroker>";
            int port = 1883;
            //Username et Password par defaut du HiveMQ broker Community
            string username = "admin-user";
            string password = "admin-password";

            /* Chemins des certificats si avec certificat
            string caCertPath = "D:\\Projet_Licence\\IOT_Controller\\IOT_Controller\\Certificats\\hivemq-server-cert.pem"; 
            string clientCertPath = "D:\\Projet_Licence\\IOT_Controller\\IOT_Controller\\Certificats\\mqtt-client-cert.pem"; 
            string clientCertPassword = "D:\\Projet_Licence\\IOT_Controller\\IOT_Controller\\Certificats\\mqtt-client-key.pem"; */

            // si avec certificat caCertPath, clientCertPath, clientCertPassword
            await _mqttConnexion.Connect(clientId, brokerAddress, port, username, password);
            Status_Erreur.Text = _mqttConnexion.IsConnected ? _mqttConnexion.ConnectingMessage : _mqttConnexion.ErrorMessage;

            //Naviguer vers la page suivant
            _ = new MobileView_Home
            {
                BindingContext = _mqttConnexion // Passer le ViewModel existant
            };
            await Navigation.PushAsync(new MobileView_Home());

            //
           
            await _mqttConnexion.Disconnect();
        }

    }   
}