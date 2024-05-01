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
public class Button : ContentControl
{

    #region Bindable Properties

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius), typeof(CornerRadius), typeof(Button), new PropertyMetadata(new CornerRadius(5)));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command), typeof(ICommand), typeof(Button), new PropertyMetadata(default(ICommand)));

    public ICommand? Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    #endregion

    /// <summary>
    /// Serve per vedere se ho settato un colore particolare, così che non venga sovrascritto da quello di sistema
    /// </summary>
    private bool CanSetBackground => Style.Setters.All(x => (x as Setter)?.Property != BackgroundProperty);
    private bool CanSetForeground => Style.Setters.All(x => (x as Setter)?.Property != ForegroundProperty);
    static Button()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(typeof(Button)));
        FontSizeProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(14.0));
        HorizontalContentAlignmentProperty.OverrideMetadata(typeof(Button),
            new FrameworkPropertyMetadata(System.Windows.HorizontalAlignment.Center));
        VerticalContentAlignmentProperty.OverrideMetadata(typeof(Button),
            new FrameworkPropertyMetadata(System.Windows.VerticalAlignment.Center));
        VerticalAlignmentProperty.OverrideMetadata(typeof(Button),
            new FrameworkPropertyMetadata(System.Windows.VerticalAlignment.Center));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        SystemParameters.StaticPropertyChanged += SystemParametersOnStaticPropertyChanged;
        if(CanSetBackground) Background = SystemParameters.WindowGlassBrush;
        SystemParametersOnStaticPropertyChanged(this, new PropertyChangedEventArgs(nameof(SystemParameters.WindowGlassBrush)));
    }

    private void SystemParametersOnStaticPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(SystemParameters.WindowGlassBrush)) return;
        if (CanSetBackground)
        {
            Background = SystemParameters.WindowGlassBrush;
        }

        if (!CanSetForeground) return;
        if (Background is not SolidColorBrush solidColorBrush) return;
        var c = solidColorBrush.Color;
        var br = 0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B;
        Foreground = br >= 128 ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White);
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        Click?.Invoke(this, EventArgs.Empty);
        if (Command is null) return;
        if(Command.CanExecute(null))
            Command.Execute(null);
    }

    public event EventHandler? Click;
}
