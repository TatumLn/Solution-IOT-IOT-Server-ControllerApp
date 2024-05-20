using System.Collections.ObjectModel;
using System.ComponentModel;
using IOT_Controller.ControllersModels;

namespace IOT_Controller.DesignView;

public partial class TroisCercleIndicateur : ContentView
{
    public ObservableCollection<string> TxtTroisIndicateur { get; set; }

    public TroisCercleIndicateur() 
    {
        InitializeComponent();
        TxtTroisIndicateur = new ObservableCollection<string>
            {
                "Temperature",
                "Luminosité",
                "Humidité"
            };

        BindingContext = this;
    }

}