using IOT_Controller.API;
using IOT_Controller.ViewsModels;

namespace IOT_Controller.Views.Desktop;

public partial class DesktopView : BaseContentPage
{
    public DesktopView()
	{
		InitializeComponent();
    }

    async void BtnConnexion(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DesktopView_Home());
    }

}