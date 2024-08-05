using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Navigation;
using BuildDeploy.Business.Database;
using BuildDeployWpf.Properties;
using BuildDeployWpf.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Language = BuildDeploy.Business.Entity.Language;

namespace BuildDeployWpf.ViewModels;
public partial class SplashScreenViewModel : BaseViewModel
{
    [ObservableProperty] private string _status = "Inizio controllo .NET";
    private readonly LanguagesManager _languageManager = new();

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
            
        }
        catch (ArgumentException)
        {
            MessageBox.Show("Non è stato possibile eseguire il comando dotnet --version, verificare l'installazione",
                "Errore",
                MessageBoxButton.OK);
            Application.Current.Shutdown();
        }
        Status = "Inizializzando il database";
        await TryInitializeDatabase();

        GoToMainPage();
    }

    private async Task TryInitializeDatabase()
    {
        var json = Resources.languages;
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var defaultLanguages = JsonSerializer.Deserialize<List<Language>>(json, options);
        if (defaultLanguages != null) await _languageManager.InitDatabase(defaultLanguages);
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
        var projectWindow = new ProjectListView();
        projectWindow.Show();
        Application.Current.MainWindow = projectWindow;
        window.Close();
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
