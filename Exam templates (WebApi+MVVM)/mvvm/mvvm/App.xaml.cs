using mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace mvvm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            MainWindow view = new MainWindow();
            DataContext context = new DataContext();
            MainWindowViewModel viewModel = new MainWindowViewModel(context); 
            view.DataContext = viewModel;  
            view.Show();
        }
    }
}
