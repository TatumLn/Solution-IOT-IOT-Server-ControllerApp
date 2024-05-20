using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace IOT_Controller.DesignView;

public partial class PopUp : ContentView
{
    public event EventHandler? Fermer;

    public PopUp()
    {
        InitializeComponent();
    }

    private void FermeturePopup(object sender, EventArgs e)
    {
        Fermer?.Invoke(this, EventArgs.Empty);
    }

}