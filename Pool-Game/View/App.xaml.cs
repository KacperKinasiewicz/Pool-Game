using System.Windows;
using ViewModel;

namespace View;
public partial class App : Application
{
    private void OnStartup(object sender, StartupEventArgs e)
    {
        ViewModelClass viewModel = new ViewModelClass(10);
        viewModel.StartSimulation();
        
        MainWindow mainWindow = new MainWindow
        {
            DataContext = viewModel
        };
        
        mainWindow.Show();
    }
}