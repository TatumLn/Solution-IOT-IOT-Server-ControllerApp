using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
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
        Task ShowLoading(string message);
        void HideLoading();

#if ANDROID
        public class AndroidNotificationService : INotificationServices
        {
            public void ShowNotification(string message) 
            {
                Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
            }

            public Task ShowLoading(string message)
            {
                 UserDialogs.Instance.ShowLoading(message);
                return Task.CompletedTask;
            }

            public void HideLoading() 
            {
                UserDialogs.Instance.HideLoading();
            }
        }
#endif

#if IOS
    public class iOSNotificationService : INotificationServices
    {
        public void ShowNotification(string message)
        {
            var alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            vc.PresentViewController(alert, true, null);
        }

        public Task ShowLoading(string message)
        {
            UserDialogs.Instance.ShowLoading(message);
            return Task.CompletedTask;
        }

        public void HideLoading()
        {
            UserDialogs.Instance.HideLoading();
        }
    }
#endif

    }
}
