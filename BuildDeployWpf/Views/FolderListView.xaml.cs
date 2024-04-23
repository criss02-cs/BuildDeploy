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
using System.Windows.Shapes;
using BuildDeployWpf.Messages;
using CommunityToolkit.Mvvm.Messaging;

namespace BuildDeployWpf.Views;

/// <summary>
/// Logica di interazione per FolderListView.xaml
/// </summary>
public partial class FolderListView : Window
{
    private string? _pathToFolder;
    private readonly string? _rootPath;
    public FolderListView(string? rootPath)
    {
        InitializeComponent();
        _rootPath = rootPath;
    }

    public static string? Show(string? rootPath)
    {
        var s = new FolderListView(rootPath);
        s.ShowDialog();
        return s._pathToFolder;
    }

    private void FolderListView_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (_rootPath != null) WeakReferenceMessenger.Default.Send(new Appearing(_rootPath));
    }
}