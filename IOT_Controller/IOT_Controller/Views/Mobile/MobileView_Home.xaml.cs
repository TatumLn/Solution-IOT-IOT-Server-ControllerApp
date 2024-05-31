
using IOT_Controller.ControllersModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Shapes;
using IOT_Controller.ViewsModels;
using IOT_Controller.DesignView;
using System.ComponentModel;
using CommunityToolkit.Maui.Views;

namespace IOT_Controller.Views.Mobile;

public partial class MobileView_Home : ContentPage
{
    private CarrouselModels _popup;

    public MobileView_Home()
	{
        InitializeComponent();
        _popup = (CarrouselModels)BindingContext;
        popup.Fermer += FermeturePopup;
    }

    private void AfficherPopup(object sender, EventArgs e)
    {
        // Logique pour afficher le popup
        var viewModel = BindingContext as CarrouselModels;
        if (viewModel != null)
        {
            viewModel.IsPopupVisible = true;
        }
    }

    private void FermeturePopup(object? sender, EventArgs e)
    {
        _popup.IsPopupVisible = false;
    }

}