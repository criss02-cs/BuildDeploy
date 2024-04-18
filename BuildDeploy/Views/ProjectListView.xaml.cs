using BuildDeploy.Messages;
using BuildDeploy.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace BuildDeploy.Views;

public partial class ProjectListView : ContentPage
{
	public ProjectListView()
	{
		InitializeComponent();
		BindingContext = new ProjectListViewModel();
	}

    private void ProjectListView_OnAppearing(object? sender, EventArgs e) =>
        WeakReferenceMessenger.Default.Send(new Appearing());
}