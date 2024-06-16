using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOT_Controller.API;
using IOT_Controller.GetipGetnotification;

namespace IOT_Controller.ViewsModels
{
    public class BaseContentView : ContentView
    {
        protected MainViewModel _mqttConnexion;
        protected IPAdressService ip;
        protected INotificationServices notification;

        public BaseContentView()
        {
            _mqttConnexion = MainViewModel.Instance;
            _mqttConnexion.MqttService.MqttTopicRecu += OnMqttTopicRecu;
            ip = DependencyService.Get<IPAdressService>();
            notification = DependencyService.Get<INotificationServices>();
            BindingContext = _mqttConnexion;
        }

        protected virtual void OnMqttTopicRecu(string topic, string payload)
        {
            // Implémentation par défaut (si nécessaire)
        }
    }
}
