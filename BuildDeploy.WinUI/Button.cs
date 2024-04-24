using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace BuildDeploy.WinUI;
/// <summary>
/// Per utilizzare questo controllo personalizzato in un file XAML, eseguire i passaggi 1a o 1b e 2.
///
/// Passaggio 1a) Utilizzo di questo controllo personalizzato in un file XAML esistente nel progetto corrente.
/// Aggiungere questo attributo XmlNamespace all'elemento radice del file di markup dove 
/// deve essere utilizzato:
///
///     xmlns:MyNamespace="clr-namespace:BuildDeploy.WinUI"
///
///
/// Passaggio 1b) Utilizzo di questo controllo personalizzato in un file XAML esistente in un progetto diverso.
/// Aggiungere questo attributo XmlNamespace all'elemento radice del file di markup dove 
/// deve essere utilizzato:
///
///     xmlns:MyNamespace="clr-namespace:BuildDeploy.WinUI;assembly=BuildDeploy.WinUI"
///
/// Sarà inoltre necessario aggiungere nel progetto in cui si trova il file XAML
/// un riferimento a questo progetto, quindi ricompilare per evitare errori di compilazione:
///
///     In Esplora soluzioni, fare clic con il pulsante destro del mouse sul progetto di destinazione, quindi scegliere
///     "Aggiungi riferimento"->"Progetti"->[Individuare e selezionare questo progetto]
///
///
/// Passaggio 2)
/// Utilizzare il controllo nel file XAML.
///
///     <MyNamespace:Button/>
///
/// </summary>
public class Button : ContentControl, INotifyPropertyChanged
{

    #region Bindable Properties

    //public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
    //    nameof(Text), typeof(string), typeof(Button), new PropertyMetadata(default(string)));

    //public string Text
    //{
    //    get => (string)GetValue(TextProperty);
    //    set => SetValue(TextProperty, value);
    //}

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius), typeof(CornerRadius), typeof(Button), new PropertyMetadata(default(CornerRadius)));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command), typeof(ICommand), typeof(Button), new PropertyMetadata(default(ICommand)));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    #endregion
    static Button()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(typeof(Button)));

    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        SystemParameters.StaticPropertyChanged += SystemParametersOnStaticPropertyChanged;
        Background = SystemParameters.WindowGlassBrush;
    }

    private void SystemParametersOnStaticPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(SystemParameters.WindowGlassBrush)) return;
        Background = SystemParameters.WindowGlassBrush;
        if (Background is not SolidColorBrush solidColorBrush) return;
        var c = solidColorBrush.Color;
        var brightness = (c.R * 299 + c.G * 587 + c.B * 114) / 1000;
        var br = 0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B;
        Foreground = br >= 128 ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White);
        OnPropertyChanged(nameof(Foreground));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
