using System.Diagnostics;
using BuildDeploy.Messages;
using BuildDeploy.Models;
using BuildDeploy.ViewModels;
using BuildDeploy.Views;
using CommunityToolkit.Mvvm.Messaging;
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
        WeakReferenceMessenger.Default.Send(new Appearing());
    }

    private void InputView_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (BindingContext is not MainPageViewModel vm) return;
        vm.LoadProjectFilesCommand.Execute(e.NewTextValue);
    }

    private bool FilterDataGrid(object record)
    {
        if (BindingContext is not MainPageViewModel vm) return false;
        if (record is FileInfo item && !vm.ShowDirectories && item.Tipo is Tipo.Folder) return false;
        return true;
    }

    private void CheckBox_OnCheckChanged(object? sender, EventArgs e)
    {
        sfPopup.Show();
        //if (projectFiles.View == null) return;
        //projectFiles.View.Filter = FilterDataGrid;
        //projectFiles.View?.RefreshFilter();
    }
}