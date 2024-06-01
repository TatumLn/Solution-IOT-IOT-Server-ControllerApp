using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

[assembly: Dependency(typeof(IOT_Controller.GetIP.iOSIPAdressService))]

namespace IOT_Controller.GetIP
{
    public class iOSIPAdressService : IPAdressService
    {
        //Recuperation de l'ip local sur IOS
        public string GetLocalIPAdress()
        {
            foreach (NetworkInterface _networkInterface  in NetworkInterface.GetAllNetworkInterfaces()) 
            {
                if (_networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || _networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet) 
                {
                    foreach (UnicastIPAddressInformation addressInfo in _networkInterface.GetIPProperties().UnicastAddresses) 
                    {
                        if (addressInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            addressInfo.Address.ToString();
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
