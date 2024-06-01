﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.ControllersModels
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
