using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.GetIP
{
    public class WindowsIPAdressService : IPAdressService
    {
        public string GetLocalIPAdress()
        {
            foreach (NetworkInterface _networkInterface in NetworkInterface.GetAllNetworkInterfaces()) 
            {
                if (_networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || _networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet) 
                {
                    foreach (UnicastIPAddressInformation adressInfo in _networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (adressInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) 
                        {
                            return adressInfo.Address.ToString();
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
