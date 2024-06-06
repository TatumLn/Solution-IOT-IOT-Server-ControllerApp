using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if ANDROID
using Android.App;
using Android.Widget;
using Android.OS;
using CommunityToolkit.Maui;
#endif
#if IOS
using UIKit;
#endif
#if WINDOWS
using System.Windows;
using Windows.UI.Popups;
#endif

namespace IOT_Controller.GetipGetnotification
{
    public interface INotificationServices
    {
        public void ShowNotification(string message);
        async Task ShowLoading(string message)
        {
           await Task.FromResult(0);
        }
    }


}
