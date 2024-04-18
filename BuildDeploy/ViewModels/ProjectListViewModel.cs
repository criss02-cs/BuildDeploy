using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildDeploy.Database;
using BuildDeploy.Messages;
using BuildDeploy.Models;
using BuildDeploy.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BuildDeploy.ViewModels;
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
        var projects = await DbService?.GetAllProjects(x => x.LastTimeOpened, true)!;
        Projects = new ObservableCollection<Project>(projects);
    }

    public void Receive(Appearing message) => Task.Run(LoadProjects);

    [RelayCommand]
    private async Task AddNewProject()
    {
        var fileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.WinUI, [".csproj"] }
        });
        var options = new PickOptions
        {
            PickerTitle = "Select a project file",
            FileTypes = fileType
        };
        var file = await FilePicker.PickAsync(options);
        if (file is null) return;
        var project = new Project
        {
            Name = file.FileName,
            Path = file.FullPath.Replace(file.FileName, ""),
            LastTimeOpened = DateTime.Now,
            DefaultDeployPath = "",
            DefaultReleasePath = ""
        };
        var result = await DbService?.AddOrUpdateProject(project)!;
        if (!result) return;
        Projects.Add(project);
    }

    [RelayCommand]
    private async Task OpenProject(Project project)
    {
        project.LastTimeOpened = DateTime.Now;
        var result = await DbService?.AddOrUpdateProject(project)!;
        if (!result) return;
        await SecureStorage.SetAsync("ProjectId", project.Id.ToString());
        var window = new Window(new MainPage())
        {
            Y = (DeviceDisplay.MainDisplayInfo.Height - 768) / 2,
            X = (DeviceDisplay.MainDisplayInfo.Width - 1366) / 2,
            Title = "Build and Deploy"
        };
        Application.Current?.OpenWindow(window);
    }
}
