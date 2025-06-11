using System.Windows;
using ViewModel;

namespace View;
public partial class App : Application
{
    private async void OnStartup(object sender, StartupEventArgs e)
    {
        ViewModelClass viewModel = new ViewModelClass(2);
        await viewModel.StartSimulation();
        
        MainWindow mainWindow = new MainWindow
        {
            DataContext = viewModel
        };
        
        mainWindow.Show();
    }
}