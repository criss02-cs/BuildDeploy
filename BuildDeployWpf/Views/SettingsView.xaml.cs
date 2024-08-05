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
using BuildDeploy.Business.Database;
using Material.Icons;
using Material.Icons.WPF;
using ColorConverter = System.Windows.Media.ColorConverter;
using Language = BuildDeploy.Business.Entity.Language;

namespace BuildDeployWpf.Views;
/// <summary>
/// Logica di interazione per SettingsView.xaml
/// </summary>
public partial class SettingsView : WinUiWindow
{

    private readonly LanguagesManager _languagesManager = new();
    private List<Language> _languagesList = new();
    public SettingsView()
    {
        InitializeComponent();
        LoadLanguages();
    }

    private async Task LoadLanguages()
    {
        _languagesList = await _languagesManager.GetAllLanguages();
        foreach (var language in _languagesList)
        {
            var textBlock = new TextBlock
            {
                FontSize = 14,
                Cursor = Cursors.Hand,
                Margin = new Thickness(40,0,0,0),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Visibility = Visibility.Collapsed,
                Text = language.Name
            };
            this.ListView.Items.Add(textBlock);
                //< TextBlock FontSize = "14" Cursor = "Hand" Margin = "40,0,0,0" HorizontalAlignment = "Stretch" Visibility = "Collapsed" > C#</TextBlock>
        }
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
            if(_languagesList.Select(x => x.Name).ToList().Contains(textBlock.Text))
                textBlock.Visibility = textBlock.Visibility == Visibility.Visible
                    ? Visibility.Collapsed
                    : Visibility.Visible;
        }
    }

    private async void ListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
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
        stackPanel.Children.Add(title);
        stackPanel.Children.Add(subTitle);
        foreach (var linguaggio in _languagesList)
        {
            var textBlock = new TextBlock
            {
                Text = linguaggio.Name,
                FontSize = 14,
                Cursor = Cursors.Hand,
                Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#6b9bfa")),
                TextDecorations = new TextDecorationCollection
                {
                    TextDecorations.Underline
                },
                Margin = new Thickness(15, 10, 0, 0)
            };
            stackPanel.Children.Add(textBlock);
        }
        return stackPanel;
    }
}