using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using IOT_Controller.ViewsModels;

namespace IOT_Controller.DesignView;

public partial class PopUp : BaseContentView
{
   

    public PopUp()
    {
        InitializeComponent();
        InitializeGesture();
    }

    private void InitializeGesture()
    {
        var _taperHorsPopUp = new TapGestureRecognizer();
        _taperHorsPopUp.Tapped += (s, e) => { IsVisible = false; };
        this.GestureRecognizers.Add(_taperHorsPopUp);
        //
        var _swipeVersleBas = new SwipeGestureRecognizer { Direction = SwipeDirection.Down };
        _swipeVersleBas.Swiped += (s, e) => { IsVisible = false; };
        this.GestureRecognizers.Add(_swipeVersleBas);
    }

    private void SwitcherVersHomeassistant(object sender, EventArgs e)
    {
        var _uri = "homeassistant://navigate";
        try
        {
            Launcher.OpenAsync(new Uri(_uri));
        }
        catch (Exception ex) 
        {
            //si l'application n;est pas encore installer
            notification.ShowNotification($"Home assistant n'est pas installer sur votre appareil veuillez l'installer {ex.Message}");
        }
    }
}