using BuildDeploy.Database;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
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


        builder.ConfigureLifecycleEvents(x =>
        {
            x.AddWindows(cd =>
            {
                cd.OnWindowCreated(w =>
                {
                    if (w is not MauiWinUIWindow window)
                        return;

                    window.ExtendsContentIntoTitleBar = false;
                    var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                    var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                    switch (appWindow.Presenter)
                    {
                        case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                            overlappedPresenter.IsResizable = false;
                            overlappedPresenter.SetBorderAndTitleBar(false, false);
                            break;
                    }
                });
            });
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}