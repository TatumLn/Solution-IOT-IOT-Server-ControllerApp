using IOT_Controller.API;
using IOT_Controller.ViewsModels;
using IOT_Controller.GetipGetnotification;

namespace IOT_Controller.Views.Desktop;

public partial class DesktopView : BaseContentPage
{
    private readonly INotificationServices notification;
    public DesktopView()
	{
		InitializeComponent();
        notification = DependencyService.Get<INotificationServices>();
        _mqttConnexion.LoadingMessageChanged += OnLoadingMessageChanged;
    }

    private void OnLoadingMessageChanged(object? sender, EventArgs e)
    {
        notification.ShowLoading(_mqttConnexion.LoadingMessage ?? "...");
    }

    async void BtnConnexion(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new DesktopView_Home());
    }

}