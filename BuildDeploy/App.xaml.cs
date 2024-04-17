using BuildDeploy.Models;
using BuildDeploy.Utils;
using BuildDeploy.Views;

namespace BuildDeploy;

public partial class App : Application
{
    public static CommandLineArgs Args = new();
    public App()
    {
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
            "MzIyNzMxM0AzMjM1MmUzMDJlMzBYcy8zY29hK3FNTE5XbFFvWW90SWtNTU9QNElXOWJqdFI2c2dmY3JzNm8wPQ==");
        InitializeComponent();
        Args = CommandLineArgsBuilder.Build();

        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        window.Title = "Build and Deploy";
        if (window.Page is not AppShell shell) return window;
        if (shell.CurrentItem.Title != "SplashScreen") return window;
        window.Title = "Splash Screen";
        window.Width = 700;
        window.Height = 500;
        window.MaximumHeight = 500;
        window.MinimumHeight = 500;
        window.MaximumWidth = 700;
        window.MinimumWidth = 700;
        window.Y = (DeviceDisplay.MainDisplayInfo.Height - 500) / 2;
        window.X = (DeviceDisplay.MainDisplayInfo.Width - 700) / 2;
        return window;
    }
}