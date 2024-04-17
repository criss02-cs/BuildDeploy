using System.Collections.ObjectModel;
using BuildDeploy.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileInfo = System.IO.FileInfo;

namespace BuildDeploy.ViewModels;
public partial class MainPageViewModel : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<FileInfo> _projects = [];
    [ObservableProperty] private ObservableCollection<Models.FileInfo> _projectFiles = [];
    [ObservableProperty] private string _projectPath = "";
    [ObservableProperty] private bool _showDirectories = true;

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
                new Models.FileInfo(x.FullName, x.Name, FormatBytes(x.Length), x.LastWriteTime, false, Tipo.FILE))
                .ToList();
            if (ShowDirectories)
            {
                var dirs = d.GetDirectories();
                p.AddRange(dirs.Select(x =>
                    new Models.FileInfo(x.FullName, x.Name, "", x.LastWriteTime, false, Tipo.CARTELLA)));
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
}
