using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using BuildDeployWpf.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BuildDeployWpf.ViewModels;
public partial class SplashScreenViewModel : BaseViewModel
{
    [ObservableProperty] private string _status = "Inizio controllo .NET";


    [RelayCommand]
    private async Task CheckDotNetVersion()
    {
        Status = "Controllo .NET nella variabile d'ambiente PATH";
        await Task.Delay(500);
        var result = await Task.Run(CheckDotNetInPath);
        if (!result)
        {
            MessageBox.Show(".NET non è presente nella variabile d'ambiente PATH, si prega di installarla", "Errore",
                MessageBoxButton.OK);
            Application.Current.Shutdown();
        }

        Status = "Ottenendo la versione di .NET";
        await Task.Delay(500);
        var output = await GetDotNetVersion();
        try
        {
            var version = new Version(output.Trim());
            GoToMainPage();
        }
        catch (ArgumentException)
        {
            MessageBox.Show("Non è stato possibile eseguire il comando dotnet --version, verificare l'installazione",
                "Errore",
                MessageBoxButton.OK);
            Application.Current.Shutdown();
        }
    }

    private static async Task<string> GetDotNetVersion()
    {
        var process = new Process();
        var startInfo = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = "dotnet --version",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        process.StartInfo = startInfo;
        process.Start();
        var output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();
        return output;
    }

    private void GoToMainPage()
    {
        var window = Application.Current.MainWindow;
        if (window == null) return;
        //window.Height = 675;
        //window.MaxHeight = 675;
        //window.Width = 1018;
        //window.MaxWidth = 1018;
        //window.Content = new ProjectListView();
        //window.DataContext = new ProjectListViewModel();
        //window.WindowStyle = WindowStyle.SingleBorderWindow;

        Application.Current.Dispatcher.Invoke(() =>
        {
            var projectWindow = new ProjectListView();
            projectWindow.Show();
            window.Close();
        });
    }

    private static bool CheckDotNetInPath()
    {
        var path = Environment.GetEnvironmentVariable("PATH");
        if (path is null) return false;
        var items = path.Split(';').ToList();
        var dotnetPath = items.FirstOrDefault(x => x.Contains(@"\dotnet\"));
        return !string.IsNullOrEmpty(dotnetPath);
    }
}
