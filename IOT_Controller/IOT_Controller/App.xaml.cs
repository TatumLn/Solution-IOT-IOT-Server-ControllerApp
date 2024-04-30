using IOT_Controller.Views.Mobile;
using IOT_Controller.Views.Desktop;

namespace IOT_Controller
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            #if ANDROID || IOS
                        MainPage = new NavigationPage(new MobileView());
            #else
                        MainPage = new NavigationPage(new DesktopView());
            #endif
        }


    }
}
