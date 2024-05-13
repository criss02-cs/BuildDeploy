using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BuildDeployWpf.ViewModels;

namespace BuildDeployWpf.Views;
/// <summary>
/// Logica di interazione per FtpSettingsView.xaml
/// </summary>
public partial class FtpSettingsView : UserControl
{
    public FtpSettingsView()
    {
        InitializeComponent();
        LoadFtpProfiles();
    }

    private void LoadFtpProfiles()
    {
        if(DataContext is not FtpSettingsViewModel vm) return;
        vm.LoadAllProfilesCommand.ExecuteAsync(null);
    }
}
