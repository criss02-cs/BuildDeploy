using System.Text;
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
using BuildDeployWpf.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace BuildDeployWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task CheckCommandLineArgs()
        {
            if (App.Args.SplashScreen is true)
            {
                if (DataContext is not SplashScreenViewModel vm) return;
                await vm.CheckDotNetVersionCommand.ExecuteAsync(null);
            }
            else
            {
                GoToMainPage();
            }
        }

        private void GoToMainPage()
        {
            //var window = new Window(new MainPage());
            //Application.Current?.OpenWindow(window);
        }

        private async void MainWindow_OnActivated(object? sender, EventArgs e)
        {
            await CheckCommandLineArgs();
        }

        private void MainWindow_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            WeakReferenceMessenger.Default.Send(new Appearing());
        }
    }
}