using IOT_Controller.ViewModels;

namespace IOT_Controller.Views.Mobile
{ 

    public partial class MobileView : ContentPage
    {
        private MainViewModel _viewModel;
        public MobileView()
	    {
		    InitializeComponent();
            _viewModel = new MainViewModel();
            BindingContext = _viewModel;
        }

        async void OnButtonClicked(object sender, EventArgs e)
        {
            var PageAccueil = new MobileView_Home();
                PageAccueil.BindingContext = _viewModel; // Passer le ViewModel existant
            await Navigation.PushAsync(new MobileView_Home());
        }
    }   
}