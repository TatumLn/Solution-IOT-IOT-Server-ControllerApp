
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
    private readonly CarrouselModels _popup;

    public MobileView_Home()
	{
        InitializeComponent();
        _popup = (CarrouselModels)BindingContext;
        popup.Fermer += FermeturePopup;
    }

    private void AfficherPopup(object sender, EventArgs e)
    {
        // Logique pour afficher le popup
        if (BindingContext is CarrouselModels viewModel)
        {
            viewModel.IsPopupVisible = true;
        }
    }

    private void FermeturePopup(object? sender, EventArgs e)
    {
        _popup.IsPopupVisible = false;
    }

    private async void BtnDeconnexion(object sender, EventArgs e)
    {
        await _mqttConnexion.Disconnect();
        await Navigation.PopAsync();
    }

}