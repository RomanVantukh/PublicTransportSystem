using PTS.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PTS.DesktopUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            LoginWindow login = new LoginWindow();

            if (!(bool)login.ShowDialog())
            {
                Current.Shutdown();
                return;
            }

            Customer customer = login.GetCustomer();

            MainWindow mainWindow = new MainWindow(customer);
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Current.MainWindow = mainWindow;
            mainWindow.Show();
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
