﻿using System;
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
using Syncfusion.Data.Extensions;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.TreeView;
using Syncfusion.Windows.Tools.Controls;
using ComboBox = System.Windows.Controls.ComboBox;
using SelectionChangedEventArgs = System.Windows.Controls.SelectionChangedEventArgs;

namespace BuildDeployWpf.Views;
/// <summary>
/// Logica di interazione per ProjectListView.xaml
/// </summary>
public partial class ProjectListView : WinUiWindow
{
    private const double AnimationDuration = 0.2;
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

    private async void ListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(DataContext is not ProjectListViewModel vm) return;
        if (sender is not System.Windows.Controls.ListView listView) return;
        await LoadProjectAndStartAnimation(vm, listView);
    }

    private async Task LoadProjectAndStartAnimation(ProjectListViewModel vm, ListView listView)
    {
        await HideProjects();
        await vm.OpenProjectCommand.ExecuteAsync(listView.SelectedItem);
    }

    private void ShowProjects()
    {
        var storyBoard = new Storyboard();

        var column = Grid.ColumnDefinitions[0]; // Sostituisci con l'indice della colonna che vuoi animare
        // animazione per la larghezza della colonna
        AnimationManager.CreateGridLengthAnimation(column.Width, new GridLength(3, GridUnitType.Star), storyBoard, column,
            new PropertyPath(ColumnDefinition.WidthProperty));

        // Animazione per l'opacità della listview
        AnimationManager.CreateDoubleAnimation(0, 1, storyBoard, ListView, new PropertyPath(OpacityProperty));

        // Animazione per l'opacità del border
        AnimationManager.CreateDoubleAnimation(0, 1, storyBoard, NewProjectBorder, new PropertyPath(OpacityProperty));

        storyBoard.Completed += (s, e) =>
        {
            //ShowArrow();
            foreach (var item in ProjectStackPanel.Children)
            {
                switch (item)
                {
                    case TextBlock textBlock:
                        textBlock.Visibility = textBlock.Name switch
                        {
                            "Back" => Visibility.Visible,
                            "Forward" => Visibility.Collapsed,
                            _ => textBlock.Visibility
                        };
                        break;
                }
            }
        };

        storyBoard.Begin(this, true);
    }

    private Task<bool> HideProjects()
    {
        var taskCompleted = new TaskCompletionSource<bool>();
        var storyBoard = new Storyboard();

        var column = Grid.ColumnDefinitions[0]; // Sostituisci con l'indice della colonna che vuoi animare

        // animazione per la larghezza della colonna
        AnimationManager.CreateGridLengthAnimation(column.Width, new GridLength(0.3, GridUnitType.Star), storyBoard, column,
            new PropertyPath(ColumnDefinition.WidthProperty));

        // Animazione per l'opacità della listview
        AnimationManager.CreateDoubleAnimation(1, 0, storyBoard, ListView, new PropertyPath(OpacityProperty));

        // Animazione per l'opacità del border
        AnimationManager.CreateDoubleAnimation(1, 0, storyBoard, NewProjectBorder, new PropertyPath(OpacityProperty));


        storyBoard.Completed += (s, e) =>
        {
            //ShowArrow();
            foreach(var item in ProjectStackPanel.Children)
            {
                switch (item)
                {
                    case TextBlock textBlock:
                        textBlock.Visibility = textBlock.Name switch
                        {
                            "Back" => Visibility.Collapsed,
                            "Forward" => Visibility.Visible,
                            _ => textBlock.Visibility
                        };
                        break;
                }
            }

            taskCompleted.TrySetResult(true);
        };

        storyBoard.Begin(this,true);
        return taskCompleted.Task;
    }
    
    private void Forward_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if(sender is not TextBlock textBlock) return;
        if (textBlock.Name == "Back")
        {
            HideProjects();
        }
        else
        {
            ShowProjects();
        }
    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }

    private void Settings_OnClick(object? sender, EventArgs e)
    {
        var settings = new SettingsView();
        settings.ShowDialog();
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