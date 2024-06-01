using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOT_Controller.API;

namespace IOT_Controller.ViewsModels
{
    public class BaseContentView : ContentView
    {
        protected MainViewModel _mqttConnexion;

        public BaseContentView()
        {
            _mqttConnexion = MainViewModel.Instance;
            BindingContext = _mqttConnexion;
        }
    }
}
