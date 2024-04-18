using System.Collections.ObjectModel;
using BuildDeploy.Messages;
using BuildDeploy.Models;
using BuildDeploy.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileInfo = System.IO.FileInfo;

namespace BuildDeploy.ViewModels;
public partial class MainPageViewModel : BaseViewModel, IRecipient<Appearing>
{
    [ObservableProperty] private ObservableCollection<FileInfo> _projects = [];
    [ObservableProperty] private ObservableCollection<Models.FileInfo> _projectFiles = [];
    [ObservableProperty] private string _projectPath = "";
    [ObservableProperty] private bool _showDirectories = true;

    public MainPageViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }

    [RelayCommand]
    private void LoadProjectsFile(string path)
    {
        var d = new DirectoryInfo(path);
        var files = d.GetFiles("*.csproj", SearchOption.AllDirectories);
        Projects = new ObservableCollection<FileInfo>(files);
    }

    [RelayCommand]
    private void OpenProject(FileInfo project)
    {
        ProjectPath = @$"{project.Directory?.FullName}\bin\Release\net8.0\";
        LoadProjectFiles(ProjectPath);
    }

    [RelayCommand]
    private void LoadProjectFiles(string path)
    {
        try
        {
            var d = new DirectoryInfo(path);
            var files = d.GetFiles("*.*");
            var p = files.Select(x =>
                new Models.FileInfo(x.FullName, x.Name, FormatBytes(x.Length), x.LastWriteTime, false, Tipo.File))
                .ToList();
            if (ShowDirectories)
            {
                var dirs = d.GetDirectories();
                p.AddRange(dirs.Select(x =>
                    new Models.FileInfo(x.FullName, x.Name, "", x.LastWriteTime, false, Tipo.Folder)));
            }
            ProjectFiles = new ObservableCollection<Models.FileInfo>(p);
        }
        catch (Exception e)
        {

        }
    }


    private static string FormatBytes(long bytes)
    {
        string[] sizes = ["B", "KB", "MB", "GB", "TB"];
        var order = 0;
        while (bytes >= 1024 && order < sizes.Length - 1)
        {
            order++;
            bytes /= 1024;
        }

        return $"{bytes:0.##} {sizes[order]}";
    }

    public async void Receive(Appearing message)
    {
        var splashWindow =
            Application.Current?.Windows.FirstOrDefault(x =>
                (x.Page as AppShell)?.CurrentPage is ProjectListView);
        if (splashWindow is not null)
        {
            if (MainThread.IsMainThread)
            {
                Application.Current?.CloseWindow(splashWindow);
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() => Application.Current?.CloseWindow(splashWindow));
            }
        }
        var id = await SecureStorage.GetAsync("ProjectId");
        if (id is null) return;
        var project = await DbService?.GetProjectById(Convert.ToInt32(id))!;
        if (project.Path != null) LoadProjectFiles(@$"{project.Path}\bin");
    }
}
