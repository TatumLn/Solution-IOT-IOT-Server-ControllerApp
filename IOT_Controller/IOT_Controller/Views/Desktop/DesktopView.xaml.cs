using IOT_Controller.API;

namespace IOT_Controller.Views.Desktop;

public partial class DesktopView : ContentPage
{
    private MainViewModel _viewModel;
    public DesktopView()
	{
		InitializeComponent();
        _viewModel = new MainViewModel();
        BindingContext = _viewModel;
    }

    async void OnButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DesktopView_Home());
    }

}