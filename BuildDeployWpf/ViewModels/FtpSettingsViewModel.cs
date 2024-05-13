using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildDeploy.Business.Database;
using BuildDeploy.Business.Entity;
using BuildDeploy.Business.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BuildDeployWpf.ViewModels;
public partial class FtpSettingsViewModel : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<FtpProfile> _ftpProfiles = [];
    private readonly FtpProfilesManager _ftpProfilesManager = new();

    [RelayCommand]
    private async Task LoadAllProfiles()
    {
        var profiles = await _ftpProfilesManager.GetAllProfiles();
        FtpProfiles.AddRange(profiles);
    }
}
