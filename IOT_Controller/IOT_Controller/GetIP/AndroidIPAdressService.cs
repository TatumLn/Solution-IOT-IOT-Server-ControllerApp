#if ANDROID
using Android.Content;
using Android.Net.Wifi;
using Android.App;

namespace IOT_Controller.GetIP
{
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
}
#endif
