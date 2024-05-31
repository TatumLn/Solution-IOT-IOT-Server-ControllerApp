using IOT_Controller.ControllersModels;

namespace IOT_Controller.Views.Mobile
{ 

    public partial class MobileView : ContentPage
    {
        private readonly MainViewModel _mqttConnexion;
        public MobileView()
	    {
		    InitializeComponent();
            _mqttConnexion = new MainViewModel();
            BindingContext = _mqttConnexion;
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            //Conexion au broker en local (par defaut)
            string clientId = "ControlAppClient";
            string brokerAddress = "192.168.0.126";
            int port = 1883;
            string username = "Prosper";
            string password = "pass1";
            await _mqttConnexion.Connect(clientId, brokerAddress, port, username, password);
            Status_Erreur.Text = _mqttConnexion.IsConnected ? _mqttConnexion.ConnectingMessage : _mqttConnexion.ErrorMessage;

            //Naviguer vers la page suivant
            var PageAccueil = new MobileView_Home();
                PageAccueil.BindingContext = _mqttConnexion; // Passer le ViewModel existant
            await Navigation.PushAsync(new MobileView_Home());
        }
    }   
}