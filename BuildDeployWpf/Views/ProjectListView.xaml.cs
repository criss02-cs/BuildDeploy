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
using BuildDeploy.WinUI;
using BuildDeployWpf.Messages;
using BuildDeployWpf.Utils;
using CommunityToolkit.Mvvm.Messaging;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.TreeView;
using Syncfusion.Windows.Tools.Controls;

namespace BuildDeployWpf.Views;
/// <summary>
/// Logica di interazione per ProjectListView.xaml
/// </summary>
public partial class ProjectListView : WinUiWindow
{
    public ProjectListView()
    {
        InitializeComponent();
        DataGrid.AutoScroller.AutoScrolling = AutoScrollOrientation.Both;
        DataGrid.AutoScroller.IsEnabled = true;
        SfSkinManager.SetTheme(this, new Theme("Windows11Dark", [nameof(SfTreeView)]));
    }

    private void ProjectListView_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        WeakReferenceMessenger.Default.Send(new Appearing());
    }

    private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (sender is not ScrollViewer scv) return;
        scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
        e.Handled = true;
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        WindowDropShadow.DropShadowToWindow(this);
    }
}
