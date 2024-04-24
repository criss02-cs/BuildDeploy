using System.Configuration;
using System.Data;
using System.Windows;
using BuildDeploy.Business.Database;
using BuildDeploy.Business.Models;
using BuildDeploy.Business.Utils;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Syncfusion.SfSkinManager;

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
        SfSkinManager.ApplyStylesOnApplication = true;
    }


    protected override void OnStartup(StartupEventArgs e)
    {
        var facade = new DatabaseFacade(DatabaseContext.Instance);
        facade.EnsureCreated();
    }
}