using IOT_Controller.API;
using IOT_Controller.ViewsModels;
using IOT_Controller.GetipGetnotification;

namespace IOT_Controller.Views.Mobile
{

    public partial class MobileView : BaseContentPage
    {
        private readonly IPAdressService ip;
        private readonly INotificationServices notification;


        public MobileView()
	    {           
            InitializeComponent();
            ip = DependencyService.Get<IPAdressService>();
            notification = DependencyService.Get<INotificationServices>();
            _mqttConnexion.LoadingMessageChanged += OnLoadingMessageChanged;
            Status_Erreur.Text = ip.GetLocalIPAdress().ToString();
        }

        private void OnLoadingMessageChanged(object sender, EventArgs e)
        {
            notification.ShowLoading(_mqttConnexion.LoadingMessage?? "No message");
        }

        [Obsolete]
        private async void BtnConnexion(object sender, EventArgs e)
        {
            await notification.ShowLoading(_mqttConnexion.LoadingMessage??"Connexion");
            //Conexion au broker en local (par defaut)
            string clientId = "ControlAppClient";
<<<<<<< HEAD
            string brokerAddress = " <AdressBroker>";
=======
            string brokerAddress = ip.GetLocalIPAdress();
>>>>>>> TatumLn
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

            notification.HideLoading();

            if (_mqttConnexion.IsConnected)
            {
                notification.ShowNotification("Connexion réussie");
                await Navigation.PushAsync(new MobileView_Home());
            }
            else
            {
                notification.ShowNotification("Échec de la connexion");
                await Navigation.PushAsync(new MobileView_Home());
            }

            //
            await _mqttConnexion.Disconnect();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _mqttConnexion.LoadingMessageChanged -= OnLoadingMessageChanged;
        }

    }   
}