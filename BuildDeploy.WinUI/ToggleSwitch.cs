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
///     <MyNamespace:ToggleSwitch/>
///
/// </summary>
public class ToggleSwitch : Control
{

    #region Bindable Properties

    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
        nameof(IsChecked), typeof(bool), typeof(ToggleSwitch), new PropertyMetadata(default(bool)));

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public static readonly DependencyProperty CheckedChangedCommandProperty = DependencyProperty.Register(
        nameof(CheckedChangedCommand), typeof(ICommand), typeof(ToggleSwitch), new PropertyMetadata(default(ICommand)));

    public ICommand CheckedChangedCommand
    {
        get => (ICommand)GetValue(CheckedChangedCommandProperty);
        set => SetValue(CheckedChangedCommandProperty, value);
    }

    

    #endregion
    static ToggleSwitch()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleSwitch), new FrameworkPropertyMetadata(typeof(ToggleSwitch)));
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        IsChecked = !IsChecked;
        CheckedChanged?.Invoke(this, IsChecked);
        CheckedChangedCommand?.Execute(IsChecked);
    }

    public event EventHandler<bool>? CheckedChanged;
}
