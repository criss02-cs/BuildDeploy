using System.Diagnostics;
using BuildDeploy.Models;
using BuildDeploy.ViewModels;
using BuildDeploy.Views;
using Syncfusion.Maui.Buttons;
using FileInfo = BuildDeploy.Models.FileInfo;

namespace BuildDeploy;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainPageViewModel();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {

    }

    private void MainPage_OnAppearing(object? sender, EventArgs e)
    {
        Task.Run(() =>
        {
            var splashWindow =
                Application.Current?.Windows.FirstOrDefault(x =>
                    (x.Page as AppShell)?.CurrentPage is SplashScreenView);
            if (splashWindow is not null)
            {
                MainThread.BeginInvokeOnMainThread(() => Application.Current?.CloseWindow(splashWindow));
            }
        });
        if(BindingContext is not MainPageViewModel vm) return;
        vm.LoadProjectsFileCommand.Execute(@"F:\Source\C#\criss02-cs\Tesi");
    }

    private void InputView_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (BindingContext is not MainPageViewModel vm) return;
        vm.LoadProjectFilesCommand.Execute(e.NewTextValue);
    }

    private bool FilterDataGrid(object record)
    {
        if (BindingContext is not MainPageViewModel vm) return false;
        if (record is FileInfo item && !vm.ShowDirectories && item.Tipo is Tipo.CARTELLA) return false;
        return true;
    }

    private void ToggleButton_OnStateChanged(object? sender, StateChangedEventArgs e)
    {
        this.projectFiles.View?.RefreshFilter();
    }

    private void CheckBox_OnCheckChanged(object? sender, EventArgs e)
    {
        if (projectFiles.View == null) return;
        projectFiles.View.Filter = FilterDataGrid;
        projectFiles.View?.RefreshFilter();
    }
}