using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BuildDeploy.ViewModels;
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
            await Shell.Current.DisplayAlert("Errore",
                ".NET non è presente nella variabile d'ambiente PATH, si prega di installarla", "Ok");
            Application.Current?.Quit();
        }

        Status = "Ottenendo la versione di .NET";
        await Task.Delay(500);
        var output = await GetDotNetVersion();
        try
        {
            var version = new Version(output.Trim());
            await SecureStorage.SetAsync("DotNetVersion", version.ToString());
            GoToMainPage();
        }
        catch (ArgumentException)
        {
            await Shell.Current.DisplayAlert("Errore",
                "Non è stato possibile eseguire il comando dotnet --version, verificare l'installazione", "Ok");
            Application.Current?.Quit();
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

    private static void GoToMainPage()
    {
        var window = new Window(new MainPage())
        {
            Y = (DeviceDisplay.MainDisplayInfo.Height - 768) / 2,
            X = (DeviceDisplay.MainDisplayInfo.Width - 1366) / 2,
            Title = "Build and Deploy"
        };
        Application.Current?.OpenWindow(window);
    }

    private static bool CheckDotNetInPath()
    {
        var path = Environment.GetEnvironmentVariable("PATH");
        if (path is null) return false;
        var items = path.Split(';').ToList();
        var dotnetPath = items.FirstOrDefault(x => x.Contains("\\dotnet\\"));
        return !string.IsNullOrEmpty(dotnetPath);
    }
}
