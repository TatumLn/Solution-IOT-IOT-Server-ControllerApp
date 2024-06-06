using IOT_Controller.Views.Mobile;
using IOT_Controller.Views.Desktop;
using IOT_Controller.GetipGetnotification;
using static IOT_Controller.GetipGetnotification.INotificationServices;

namespace IOT_Controller
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

#if ANDROID
            DependencyService.Register<IPAdressService, AndroidIPAdressService>();
            DependencyService.Register<INotificationServices, AndroidNotificationService>();
#elif IOS
            DependencyService.Register<IPAdressService, IOSIPAdressService>();
            DependencyService.Register<INotificationServices, iOSNotificationService>();
#else
            DependencyService.Register<IPAdressService, WindowsIPAdressService>();
            DependencyService.Register<INotificationServices, WindowsNotificationService>();
#endif


#if ANDROID || IOS
            MainPage = new NavigationPage(new MobileView());
            #else
                        MainPage = new NavigationPage(new DesktopView());
            #endif
        }


    }
}
