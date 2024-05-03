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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BuildDeploy.WinUI;
using BuildDeployWpf.Messages;
using BuildDeployWpf.Utils;
using BuildDeployWpf.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.TreeView;
using Syncfusion.Windows.Tools.Controls;
using SelectionChangedEventArgs = System.Windows.Controls.SelectionChangedEventArgs;

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

    private void ListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(DataContext is not ProjectListViewModel vm) return;
        if (sender is not System.Windows.Controls.ListView listView) return;
        vm.OpenProjectCommand.ExecuteAsync(listView.SelectedItem);
        HideProjects();
    }

    private void HideProjects()
    {
        var arrow = new TextBlock();
        arrow.Text = "&#xE72A;";
        arrow.Opacity = 0;
        arrow.FontFamily = new FontFamily("Segoe MDL2 Assets");

        var storyBoard = new Storyboard();

        var column = Grid.ColumnDefinitions[0]; // Sostituisci con l'indice della colonna che vuoi animare

        var animation = new GridLengthAnimation
        {
            From = column.Width,
            To = new GridLength(0.3, GridUnitType.Star), // Sostituisci con la nuova larghezza desiderata
            Duration = TimeSpan.FromSeconds(0.5) // Sostituisci con la durata desiderata
        };
        storyBoard.Children.Add(animation);
        Storyboard.SetTarget(animation, column);
        Storyboard.SetTargetProperty(animation, new PropertyPath(ColumnDefinition.WidthProperty));

        var listViewAnimation = new DoubleAnimation
        {
            From = 1,
            To = 0,
            Duration = TimeSpan.FromSeconds(0.5)
        };
        storyBoard.Children.Add(listViewAnimation);
        Storyboard.SetTarget(listViewAnimation, ListView);
        Storyboard.SetTargetProperty(listViewAnimation, new PropertyPath(OpacityProperty));

        var borderAnimation = new DoubleAnimation
        {
            From = 1,
            To = 0,
            Duration = TimeSpan.FromSeconds(0.5)
        };
        storyBoard.Children.Add(borderAnimation);
        Storyboard.SetTarget(borderAnimation, NewProjectBorder);
        Storyboard.SetTargetProperty(borderAnimation, new PropertyPath(OpacityProperty));

        storyBoard.Completed += (s, e) =>
        {
            ShowArrow();
        };

        storyBoard.Begin(this,true);
        
    }

    private void ShowArrow()
    {
        var storyBoard = new Storyboard();
        foreach (var item in ProjectStackPanel.Children)
        {
            switch (item)
            {
                case ListView listView:
                    listView.Visibility = Visibility.Collapsed;
                    break;
                case TextBlock text:
                {
                    text.Visibility = Visibility.Visible;
                    var listViewAnimation = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(0.5)
                    };
                    storyBoard.Children.Add(listViewAnimation);
                    Storyboard.SetTarget(listViewAnimation, text);
                    Storyboard.SetTargetProperty(listViewAnimation, new PropertyPath(OpacityProperty));
                    break;
                }
            }
        }

        ProjectStackPanel.VerticalAlignment = VerticalAlignment.Center;
        storyBoard.Begin(this);
    }
}
public class GridLengthAnimation : AnimationTimeline
{
    public override Type TargetPropertyType => typeof(GridLength);

    protected override Freezable CreateInstanceCore() => new GridLengthAnimation();

    public GridLength From
    {
        get => (GridLength)GetValue(FromProperty);
        set => SetValue(FromProperty, value);
    }

    public static readonly DependencyProperty FromProperty =
        DependencyProperty.Register("From", typeof(GridLength), typeof(GridLengthAnimation));

    public GridLength To
    {
        get => (GridLength)GetValue(ToProperty);
        set => SetValue(ToProperty, value);
    }

    public static readonly DependencyProperty ToProperty =
        DependencyProperty.Register("To", typeof(GridLength), typeof(GridLengthAnimation));

    public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
    {
        double fromVal = From.Value;
        double toVal = To.Value;

        if (fromVal > toVal)
        {
            return new GridLength((1 - animationClock.CurrentProgress.Value) * (fromVal - toVal) + toVal, GridUnitType.Star);
        }
        else
        {
            return new GridLength(animationClock.CurrentProgress.Value * (toVal - fromVal) + fromVal, GridUnitType.Star);
        }
    }
}