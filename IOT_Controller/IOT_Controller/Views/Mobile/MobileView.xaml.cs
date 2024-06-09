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
        }

        private void OnLoadingMessageChanged(object? sender, EventArgs e)
        {
            notification.ShowLoading(_mqttConnexion.LoadingMessage?? "...");
        }

        [Obsolete]
        private async void BtnConnexion(object sender, EventArgs e)
        {
            
            //Conexion au broker en local (par defaut)
            string brokerAddress = "192.168.0.126";//ip.GetLocalIPAdress();
            int port = 1883;
            //Username et Password par defaut du HiveMQ broker Community
            string username = "admin-user";
            string password = "admin-password";
            string clientId = $"{username}";

            /* Chemins des certificats si avec certificat
            string caCertPath = "D:\\Projet_Licence\\IOT_Controller\\IOT_Controller\\Certificats\\hivemq-server-cert.pem"; 
            string clientCertPath = "D:\\Projet_Licence\\IOT_Controller\\IOT_Controller\\Certificats\\mqtt-client-cert.pem"; 
            string clientCertPassword = "D:\\Projet_Licence\\IOT_Controller\\IOT_Controller\\Certificats\\mqtt-client-key.pem"; */

            // si avec certificat caCertPath, clientCertPath, clientCertPassword
            await _mqttConnexion.Connect(clientId, brokerAddress, port, username, password);

            if (_mqttConnexion.IsConnected)
            {
                notification.ShowNotification("Connexion r�ussie");
                await Navigation.PushAsync(new MobileView_Home());
            }
            else
            {
                notification.ShowNotification("�chec de la connexion");
                await Navigation.PushAsync(new MobileView_Home());
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