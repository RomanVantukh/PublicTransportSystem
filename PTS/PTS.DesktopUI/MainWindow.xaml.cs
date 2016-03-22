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
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using PTS.Entities;
using PTS.Repositories;


namespace PTS.DesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Customer _customer;

        private string _connectionString = ConfigurationManager.ConnectionStrings["PTS"].ConnectionString;

        public MainWindow(Customer customer)
        {
            InitializeComponent();
            _customer = customer;
            lblCustomer.Content = String.Format("{0} : {1} {2}", _customer.Mode.ToString(), _customer.Name, _customer.Surname);
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();

            wndMainWindow.IsEnabled = false;
            if (!(bool)login.ShowDialog())
            {
                login.Close();
            }
            else
            {
                _customer = login.GetCustomer();
                lblCustomer.Content = String.Format("{0} : {1} {2}", _customer.Mode.ToString(), _customer.Name, _customer.Surname);
            }
            wndMainWindow.IsEnabled = true;
        }

        private void btnRoutes_Click(object sender, RoutedEventArgs e)
        {
            var routeRepository = new SqlRouteRepository(_connectionString);
            var routeWindow = new RouteWindow(_customer, routeRepository.SelectAll());
            wndMainWindow.IsEnabled = false;
            routeWindow.ShowDialog();
            wndMainWindow.IsEnabled = true;
        }

        private void btnBuses_Click(object sender, RoutedEventArgs e)
        {
            var busRepository = new SqlBusRepository(_connectionString);
            var busWindow = new BusWindow(_customer, busRepository.SelectAll());
            wndMainWindow.IsEnabled = false;
            busWindow.ShowDialog();
            wndMainWindow.IsEnabled = true;
        }

        private void btnDrivers_Click(object sender, RoutedEventArgs e)
        {
            var driverRepository = new SqlDriverRepository(_connectionString);
            var driverWindow = new DriverWindow(_customer, driverRepository.SelectAll());
            wndMainWindow.IsEnabled = false;
            driverWindow.ShowDialog();
            wndMainWindow.IsEnabled = true;
        }

        private void btnStation_Click(object sender, RoutedEventArgs e)
        {
            var stationRepositoty = new SqlStationRepository(_connectionString);
            var stationWindow = new StationWindow(_customer, stationRepositoty.SelectAll());
            wndMainWindow.IsEnabled = false;
            stationWindow.ShowDialog();
            wndMainWindow.IsEnabled = true;
        }

        private void btnTimetable_Click(object sender, RoutedEventArgs e)
        {
            var scheduleRepository = new SqlScheduleRepository(_connectionString);
            var timeTableWindow = new ScheduleWindow(_customer, scheduleRepository.SelectAll());
            wndMainWindow.IsEnabled = false;
            timeTableWindow.ShowDialog();
            wndMainWindow.IsEnabled = true;
        }
    }
}
