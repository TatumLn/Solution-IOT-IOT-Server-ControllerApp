using Acr.UserDialogs;
using Foundation;
using UIKit;

namespace IOT_Controller
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            UserDialogs.Init();
            return base.FinishedLaunching(app, options);
        }
    }
}
