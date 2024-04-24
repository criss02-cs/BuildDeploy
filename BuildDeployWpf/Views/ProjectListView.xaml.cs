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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BuildDeployWpf.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Syncfusion.UI.Xaml.Grid;

namespace BuildDeployWpf.Views;
/// <summary>
/// Logica di interazione per ProjectListView.xaml
/// </summary>
public partial class ProjectListView : Window
{
    public ProjectListView()
    {
        InitializeComponent();
        DataGrid.AutoScroller.AutoScrolling = AutoScrollOrientation.Both;
        DataGrid.AutoScroller.IsEnabled = true;
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
}
