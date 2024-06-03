
using IOT_Controller.ViewsModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Shapes;
using IOT_Controller.DesignView;
using System.ComponentModel;
using CommunityToolkit.Maui.Views;

namespace IOT_Controller.Views.Mobile;

public partial class MobileView_Home : BaseContentPage
{

    public MobileView_Home()
	{
        InitializeComponent();
    }

    private void AfficherPopup(object sender, EventArgs e)
    {
        popup.IsVisible = true;
    }

    private void FermeturePopup(object? sender, EventArgs e)
    {
       popup.IsVisible = false;
    }

    private async void BtnDeconnexion(object sender, EventArgs e)
    {
        await _mqttConnexion.Disconnect();
        await Navigation.PopAsync();
    }

}