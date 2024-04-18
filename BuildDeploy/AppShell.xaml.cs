using BuildDeploy.Views;

namespace BuildDeploy;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(ProjectListView), typeof(ProjectListView));
    }

}