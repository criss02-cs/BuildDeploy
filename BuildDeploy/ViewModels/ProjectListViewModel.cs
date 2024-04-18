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
    private readonly DbService? _dbService = ServiceManager.GetService<DbService>();


    public ProjectListViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }


    [RelayCommand]
    private async Task LoadProjects()
    {
        var projects = await _dbService?.GetAllProjects(x => x.LastTimeOpened, true)!;
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
        var result = await _dbService?.AddOrUpdateProject(project)!;
        if (!result) return;
        Projects.Add(project);
    }
}
