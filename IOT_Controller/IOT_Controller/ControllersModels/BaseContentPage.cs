using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.ControllersModels
{
    public class BaseContentPage : ContentPage
    {
        protected MainViewModel _mqttConnexion;

        public BaseContentPage()
        {
            _mqttConnexion = new MainViewModel.Instance;
            BindingContext = _mqttConnexion;
        }
    }
}
