using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOT_Controller.API;

namespace IOT_Controller.ViewsModels
{
    public class BaseContentPage : ContentPage
    {
        protected MainViewModel _mqttConnexion;

        public BaseContentPage()
        {
            _mqttConnexion = MainViewModel.Instance;
            BindingContext = _mqttConnexion;
        }
    }
}
