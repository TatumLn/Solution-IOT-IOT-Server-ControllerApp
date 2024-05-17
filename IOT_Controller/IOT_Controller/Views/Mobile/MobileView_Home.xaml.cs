
using IOT_Controller.ControllersModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Shapes;
using IOT_Controller.ViewsModels;

namespace IOT_Controller.Views.Mobile;

public partial class MobileView_Home : ContentPage
{
    public double MaxY { get; set; }
    // Propriété pour l'IndicatorTemplate
    public MobileView_Home()
	{
        InitializeComponent();
        // Calculez la valeur maximale de Y
        MaxY = absoluteLayout.Height - PopUpFrame.Height;
    }

    private void AfficherPlus(object sender, EventArgs e)
    {
        PopUpFrame.IsVisible = true;
    }

    private void AfficherMoins(object sender, EventArgs e)
    {
        PopUpFrame.IsVisible = false;
    }

}