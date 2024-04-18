using BuildDeploy.Database;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using UraniumUI;

namespace BuildDeploy;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseUraniumUI()
            .UseUraniumUIMaterial()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });


        builder.Services.AddSingleton<DbService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}