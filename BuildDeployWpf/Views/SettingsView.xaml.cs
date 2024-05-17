using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
using ColorConverter = System.Windows.Media.ColorConverter;

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
        if (listView.SelectedItem is StackPanel)
        {
            CurrentSetting.Child = CreateViewForLanguages();
        }
        if(listView.SelectedItem is not TextBlock textBlock) return;
        switch (textBlock.Text)
        {
            case "Impostazioni FTP":
                CurrentSetting.Child = new FtpSettingsView();
                break;
            case "Linguaggi":
                CurrentSetting.Child = CreateViewForLanguages();
                break;
            default:
                //CurrentSetting.Child = new StackPanel();
                break;
        }
    }

    private UIElement CreateViewForLanguages()
    {
        var stackPanel = new StackPanel();
        var title = new TextBlock();
        title.Text = "Linguaggi";
        title.FontSize = 25;
        title.FontWeight = FontWeights.Bold;

        var subTitle = new TextBlock();
        subTitle.Text = "Configura le impostazione relative ai specifici linguaggi";
        subTitle.FontSize = 14;
        subTitle.Margin = new Thickness(0, 10, 0, 0);
        var language = new TextBlock();
        language.Text = "C#";
        language.Margin = new Thickness(15, 10, 0, 0);
        language.FontSize = 14;
        language.Cursor = Cursors.Hand;
        language.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#6b9bfa"));
        language.TextDecorations = new TextDecorationCollection
        {
            TextDecorations.Underline
        };
        var language1 = new TextBlock();
        language1.FontSize = 14;
        language1.Text = "Angular";
        language1.Cursor = Cursors.Hand;
        language1.TextDecorations = new TextDecorationCollection
        {
            TextDecorations.Underline
        };
        language1.Margin = new Thickness(15, 0, 0, 0);
        language1.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#6b9bfa"));
        stackPanel.Children.Add(title);
        stackPanel.Children.Add(subTitle);

        stackPanel.Children.Add(language);
        stackPanel.Children.Add(language1);
        return stackPanel;
    }
}

public class Language(string header)
{
    public string Header { get; set; } = header;
    public List<Language> Languages { get; set; } = [];
}
