using System.Configuration;
using System.Data;
using System.Windows;
using BuildDeployWpf.Database;
using BuildDeployWpf.Models;
using BuildDeployWpf.Utils;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BuildDeployWpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static CommandLineArgs Args = new();
    public App()
    {
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
            "MzIzMTI0NEAzMjM1MmUzMDJlMzBYQWVaeGx3Tlg2dHlHc1R2Rjc3SEhiZHhnTWx5NGhRYkFsZ1N3U1hZakc4PQ==");
        Args = CommandLineArgsBuilder.Build();
    }


    protected override void OnStartup(StartupEventArgs e)
    {
        var facade = new DatabaseFacade(DatabaseContext.Instance);
        facade.EnsureCreated();
    }
}