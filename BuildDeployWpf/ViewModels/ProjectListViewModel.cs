using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using BuildDeploy.Business.Database;
using BuildDeploy.Business.Entity;
using BuildDeploy.Business.Models;
using BuildDeploy.Business.Utils;
using BuildDeploy.WinUI;
using BuildDeployWpf.Extensions;
using BuildDeployWpf.Messages;
using BuildDeployWpf.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileInfo = BuildDeploy.Business.Models.FileInfo;

namespace BuildDeployWpf.ViewModels;
public partial class ProjectListViewModel : BaseViewModel, IRecipient<Appearing>
{
    [ObservableProperty] private ObservableCollection<Project> _projects = [];
    [ObservableProperty] private ObservableCollection<Folder> _folders = [];
    [ObservableProperty, NotifyPropertyChangedFor(nameof(ShowDataGrid))] private ObservableCollection<FileInfo> _projectFiles = [];
    [ObservableProperty] private Project _selectedProject = new();
    [ObservableProperty] private Folder _selectedFolder = new();
    [ObservableProperty] private bool _showDirectories = true;
    public bool ShowDataGrid => ProjectFiles.Count > 0;


    public ProjectListViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }


    [RelayCommand]
    private async Task LoadProjects()
    {
        var projects = await DbService.Instance.GetAllProjects(x => x.LastTimeOpened, true)!;
        Projects.AddRange(projects);
    }

    public void Receive(Appearing message)
    {
        Task.Run(LoadProjects);
    }

    [RelayCommand]
    private async Task AddNewProject()
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "Project files (*.csproj)|*.csproj",
        };
        var result = dialog.ShowDialog();
        if (result is not true) return;
        var project = new Project
        {
            Name = dialog.SafeFileName,
            Path = dialog.FileName.Replace(dialog.SafeFileName, ""),
            LastTimeOpened = DateTime.Now,
            DefaultDeployPath = "",
            DefaultReleasePath = ""
        };
        result = await DbService.Instance.AddOrUpdateProject(project);
        if (result is not true) return;
        Application.Current.Dispatcher.Invoke(() => Projects.Add(project));
        SelectedProject = project;
        LoadFolders();
    }


    private void LoadFolders()
    {
        if (SelectedProject.Path == null) return;
        if (SelectedProject.DefaultReleasePath.IsNullOrEmpty())
        {
            var folder = Utils.FindFolderAndSubFolders(SelectedProject.Path);
            Folders.AddRange([folder]);
        }
        else
        {
            SelectedFolder = new Folder
            {
                Path = SelectedProject.DefaultReleasePath
            };
            SelectedFolder.Name = Path.GetDirectoryName(SelectedFolder.Path);
            LoadProjectFiles();
        }
    }

    [RelayCommand]
    private Task OpenProject(object param)
    {
        return Task.Run(async () =>
        {
            if (param is not Project project) return;
            project.LastTimeOpened = DateTime.Now;
            var result = await DbService.Instance?.AddOrUpdateProject(project)!;
            if (!result) return;
            SelectedProject = project;
            LoadFolders();
        });
    }

    [RelayCommand]
    private async Task OpenFolder(object param)
    {
        if (param is not Folder folder) return;
        SelectedFolder = folder;
        if (SelectedFolder.Path == null) return;
        if (SelectedProject.DefaultReleasePath.IsNullOrEmpty())
        {
            var currentWindow = Application.Current.MainWindow;
            if (currentWindow is not WinUiWindow win) return;
            var resp = await win.ShowConfirmAsync("Cartella di default",
                "Vuoi rendere questa cartella la cartella standard?", "Sì");
            if (resp)
            {
                SelectedProject.DefaultReleasePath = SelectedFolder.Path;
                await DbService.Instance.AddOrUpdateProject(SelectedProject);
            }
        }
       
    }

    private void LoadProjectFiles()
    {
        if (SelectedFolder.Path == null) return;
        var d = new DirectoryInfo(SelectedFolder.Path);
        var files = d.GetFiles("*.*");
        var p = files.Select(x =>
                new FileInfo(x.FullName, x.Name, Utils.FormatBytes(x.Length), x.LastWriteTime, Tipo.File))
            .ToList();
        if (ShowDirectories)
        {
            var dirs = d.GetDirectories();
            p.AddRange(dirs.Select(x =>
                new FileInfo(x.FullName, x.Name, "", x.LastWriteTime, Tipo.Folder)));
        }
        ProjectFiles.AddRange(p);
        OnPropertyChanged(nameof(ShowDataGrid));
    }
}
