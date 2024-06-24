
using IOT_Controller.ViewsModels;
using IOT_Controller.DesignView;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Shapes;
using System.ComponentModel;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Layouts;

namespace IOT_Controller.Views.Mobile;

public partial class MobileView_Home : BaseContentPage
{
    public MobileView_Home()
	{
        InitializeComponent();
        popup.Overlay = DarkOverlay;
        popup.PopupClosed += (sender, e) =>
        {
            DarkOverlay.IsVisible = false;
        };
        popup.PropertyChanged += OnPopupPropertyChanged;
    }

    private async void AfficherPopup(object sender, EventArgs e)
    {
        DarkOverlay.IsVisible = true;
        await DarkOverlay.FadeTo(0.5, 250); 
        await popup.ShowPopUpAsync();
    }

    private void OnPopupPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(popup.IsVisible))
        {
            if (popup.IsVisible)
            {
                // Le popup est visible, ajustez les propriétés des éléments lorsque le popup est actif
                // Masquez la deuxième rangée
                //monGrid.RowDefinitions[0].Height = new GridLength(0);
                //monGrid.RowDefinitions[1].Height = new GridLength(0);
                monGrid.RowDefinitions[0].Height = new GridLength(monGrid.RowDefinitions[0].Height.Value / 2, GridUnitType.Star);
                monGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Absolute);
                // Autres ajustements d'éléments si nécessaire...
            }
            else
            {
                // Restaurer les hauteurs d'origine lorsque le popup est caché
                //monGrid.RowDefinitions[0].Height = GridLength.Auto;
                //monGrid.RowDefinitions[1].Height = GridLength.Auto;
                monGrid.RowDefinitions[0].Height = GridLength.Star;
                monGrid.RowDefinitions[1].Height = GridLength.Star;
            }
        }
    }

        private async void BtnDeconnexion(object sender, EventArgs e)
    {
        await _mqttConnexion.Disconnect();
        await Navigation.PopAsync();
    }

}