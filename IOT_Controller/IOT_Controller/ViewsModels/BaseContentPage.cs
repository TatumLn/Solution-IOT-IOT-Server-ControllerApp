using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOT_Controller.API;
using IOT_Controller.GetipGetnotification;

namespace IOT_Controller.ViewsModels
{
    public class BaseContentPage : ContentPage
    {
        protected MainViewModel _mqttConnexion;
        protected IPAdressService ip;
        protected INotificationServices notification;

        public BaseContentPage()
        {
            _mqttConnexion = MainViewModel.Instance;
            ip = DependencyService.Get<IPAdressService>();
            notification = DependencyService.Get<INotificationServices>();
            BindingContext = _mqttConnexion;
        }


    }
}
