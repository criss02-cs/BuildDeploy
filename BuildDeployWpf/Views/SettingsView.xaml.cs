using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BuildDeploy.WinUI;
using BuildDeployWpf.Utils;
using Windows.Globalization;
using Material.Icons;
using Material.Icons.WPF;

namespace BuildDeployWpf.Views;
/// <summary>
/// Logica di interazione per SettingsView.xaml
/// </summary>
public partial class SettingsView : WinUiWindow
{
    public ObservableCollection<Language> Languages =
    [
        new Language("Linguaggi")
        {
            Languages =
            [
                new Language("C#"),
                new Language("Angular")
            ]
        }
    ];
    public SettingsView()
    {
        InitializeComponent();
    }
    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        WindowDropShadow.DropShadowToWindow(this);
    }

    private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not MaterialIcon icon) return;
        icon.Kind = icon.Kind == MaterialIconKind.MenuRight
            ? MaterialIconKind.MenuDown
            : MaterialIconKind.MenuRight;
        foreach (var item in this.ListView.Items)
        {
            if(item is not TextBlock textBlock) continue;
            if(textBlock.Text is "C#" or "Angular")
                textBlock.Visibility = textBlock.Visibility == Visibility.Visible
                    ? Visibility.Collapsed
                    : Visibility.Visible;
        }
    }

    private void ListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListView listView) return;
        if(listView.SelectedItem is not TextBlock textBlock) return;
        if (textBlock.Text is "Impostazioni FTP")
        {
            CurrentSetting.Child = new FtpSettingsView();
        }
        //if (listView.SelectedItem is not Language language) return;
        //if (language.Languages.Count is 0) return;
        //if (language.Languages[0].Header is "C#")
        //{
        //    MessageBox.Show("C#");
        //}
        //else if (language.Languages[0].Header is "Angular")
        //{
        //    MessageBox.Show("Angular");
        //}
    }
}

public class Language(string header)
{
    public string Header { get; set; } = header;
    public List<Language> Languages { get; set; } = [];
}
