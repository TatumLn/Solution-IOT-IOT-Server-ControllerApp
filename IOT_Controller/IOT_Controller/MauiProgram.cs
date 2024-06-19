using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using UraniumUI;
namespace IOT_Controller
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .UseMauiCommunityToolkit()
                .ConfigureMauiHandlers(handlers =>
                 {
                    handlers.AddHandler<SkiaSharp.Views.Maui.Controls.SKCanvasView, SkiaSharp.Views.Maui.Handlers.SKCanvasViewHandler>();
                 })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Italiana-Regular.ttf", "Italiana");
                    fonts.AddFont("Inter-Regular.ttf", "Inter");
                    fonts.AddMaterialIconFonts();
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
