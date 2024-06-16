using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

#if ANDROID
using Android.Content;
using Android.Net.Wifi;
using Android.App;
#endif

namespace IOT_Controller.GetipGetnotification
{
    public interface IPAdressService
    {
        string GetLocalIPAdress();
    }

#if ANDROID

            public class AndroidIPAdressService : IPAdressService
            {
                public string GetLocalIPAdress()
                {
                    WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);

                    var wifiInfo = wifiManager.ConnectionInfo;
                    int ipAddress = wifiInfo.IpAddress;

                    return string.Format("{0}.{1}.{2}.{3}",
                        (ipAddress & 0xff),
                        (ipAddress >> 8 & 0xff),
                        (ipAddress >> 16 & 0xff),
                        (ipAddress >> 24 & 0xff));
                }
            }
#endif

#if WINDOWS

                public class WindowsIPAdressService : IPAdressService
                {
                    public string GetLocalIPAdress()
                    {
                                    string ipAddress = "";

                        // Recherche de l'interface réseau sans fil
                        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                        foreach (NetworkInterface adapter in interfaces)
                        {
                            if (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && adapter.OperationalStatus == OperationalStatus.Up)
                            {
                                // Obtention des informations de configuration IP
                                IPInterfaceProperties properties = adapter.GetIPProperties();
                                foreach (UnicastIPAddressInformation ip in properties.UnicastAddresses)
                                {
                                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                    {
                                        // Adresse IP trouvée
                                        ipAddress = ip.Address.ToString();
                                        break;
                                    }
                                }
                          
                             }
                         }
                        return ipAddress;
                    }
                }

#endif

#if IOS
                public class IOSIPAdressService : IPAdressService
                {
                    //Recuperation de l'ip local sur IOS
                    public string GetLocalIPAdress()
                    {
                        foreach (NetworkInterface _networkInterface in NetworkInterface.GetAllNetworkInterfaces())
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
    #endif

}
