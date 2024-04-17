using BuildDeploy.ViewModels;

namespace BuildDeploy.Views;

public partial class SplashScreenView : ContentPage
{
    public SplashScreenView()
    {
        InitializeComponent();
        BindingContext = new SplashScreenViewModel();
    }

    private async Task CheckCommandLineArgs()
    {
        if (App.Args.SplashScreen is true)
        {
            if (BindingContext is not SplashScreenViewModel vm) return;
            await vm.CheckDotNetVersionCommand.ExecuteAsync(null);
        }
        else
        {
            GoToMainPage();
        }
    }

    private void GoToMainPage()
    {
        var window = new Window(new MainPage());
        Application.Current?.OpenWindow(window);
    }

    private async void SplashScreenView_OnAppearing(object? sender, EventArgs e)
    {
        await CheckCommandLineArgs();
    }
}