using System.Collections.ObjectModel;
using System.Windows;
using BuildDeployWpf.Database;
using BuildDeployWpf.Extensions;
using BuildDeployWpf.Messages;
using BuildDeployWpf.Models;
using BuildDeployWpf.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BuildDeployWpf.ViewModels;
public partial class ProjectListViewModel : BaseViewModel, IRecipient<Appearing>
{
    [ObservableProperty] private ObservableCollection<Project> _projects = [];


    public ProjectListViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }


    [RelayCommand]
    private async Task LoadProjects()
    {
        var projects = await DbService.Instance.GetAllProjects(x => x.LastTimeOpened, true)!;
        Application.Current.Dispatcher.Invoke(() => Projects.AddRange(projects));
    }

    public void Receive(Appearing message)
    {
        if (!message.Value.IsNullOrEmpty()) return;
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
        FolderListView.Show(project.Path);
    }

    [RelayCommand]
    private async Task OpenProject(object param)
    {
        if (param is not Project project) return;
        project.LastTimeOpened = DateTime.Now;
        var result = await DbService.Instance?.AddOrUpdateProject(project)!;
        if (!result) return;
        FolderListView.Show(project.Path);
        //await SecureStorage.SetAsync("ProjectId", project.Id.ToString());
        //var window = new Window(new MainPage())
        //{
        //    Y = (DeviceDisplay.MainDisplayInfo.Height - 768) / 2,
        //    X = (DeviceDisplay.MainDisplayInfo.Width - 1366) / 2,
        //    Title = "Build and Deploy",
        //};
        //Application.Current?.OpenWindow(window);
    }
}
