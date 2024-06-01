using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using System.Net;

[assembly : Dependency(typeof(IOT_Controller.GetIP.AndroidIPAdressService))]

namespace IOT_Controller.GetIP
{
    public class AndroidIPAdressService : IPAdressService
    {
        //Recuperation de l'ip local sur android
        public string GetLocalIPAdress()
        {
            _wifiManager wifiManager = (_wifiManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.WifiService);
            int _ipAdress = wifiManager.ConnectionInfo.IpAdress;
            return new InetSocketAddress(_ipAdress, 0).Adress.HostAdress;
        }
    }
}
