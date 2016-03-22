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
using System.Windows.Shapes;
using System.Configuration;
using PTS.Entities;
using PTS.Repositories;

namespace PTS.DesktopUI
{
    /// <summary>
    /// Interaction logic for DriverWindow.xaml
    /// </summary>
    public partial class DriverWindow : Window
    {
        private Customer _customer;

        private string _connectionString = ConfigurationManager.ConnectionStrings["PTS"].ConnectionString;

        public DriverWindow(Customer customer, List<Driver> drivers)
        {
            InitializeComponent();

            _customer = customer;
            if (_customer.Mode == Mode.User)
            {
                btnAdd.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
            }

            var stationRepository = new SqlStationRepository(_connectionString);

            List<string> stations = stationRepository.GetAllNames();
            stations.Insert(0, "");

            cmbStation.ItemsSource = stations;
            cmbStation.SelectedItem = "";

            var routeRepository = new SqlRouteRepository(_connectionString);

            List<string> routes = routeRepository.GetAllNumbers();
            routes.Insert(0, "");

            cmbRoute.ItemsSource = routes;
            cmbRoute.SelectedItem = "";

            dgrDataDrivers.ItemsSource = drivers;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addDriverWindow = new AddDriverWindow(_customer);
            wndDriver.IsEnabled = false;
            addDriverWindow.ShowDialog();

            var driverRepository = new SqlDriverRepository(_connectionString);
            dgrDataDrivers.ItemsSource = driverRepository.SelectAll();

            wndDriver.IsEnabled = true;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string stationName = String.IsNullOrEmpty((string)cmbStation.SelectedItem) ? null : (string)cmbStation.SelectedItem;

            string routeNumber = String.IsNullOrEmpty((string)cmbRoute.SelectedItem) ? null : (string)cmbRoute.SelectedItem;

            var driverSqlRepository = new SqlDriverRepository(_connectionString);

            dgrDataDrivers.ItemsSource = driverSqlRepository.SearchAll(routeNumber, stationName);
        }

        private void btnTimeTable_Click(object sender, RoutedEventArgs e)
        {
            var scheduleRepository = new SqlScheduleRepository(_connectionString);

            if (dgrDataDrivers.SelectedItem == null)
            {
                MessageBox.Show("Not selected driver.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int driverId = ((Driver)dgrDataDrivers.SelectedItem).Id;

            var scheduleWindow = new ScheduleWindow(_customer, scheduleRepository.GetTimeTableByDriver(driverId));
            wndDriver.Close();
            scheduleWindow.ShowDialog();
        }

        private void btnStations_Click(object sender, RoutedEventArgs e)
        {
            var stationRepository = new SqlStationRepository(_connectionString);

            if (dgrDataDrivers.SelectedItem == null)
            {
                MessageBox.Show("Not selected driver.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int driverId = ((Driver)dgrDataDrivers.SelectedItem).Id;

            var stationWindow = new StationWindow(_customer, stationRepository.GetStationByDriver(driverId));
            wndDriver.Close();
            stationWindow.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var driverRepository = new SqlDriverRepository(_connectionString);

            if (dgrDataDrivers.SelectedItem == null)
            {
                MessageBox.Show("Not selected driver.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Driver driver = (Driver)dgrDataDrivers.SelectedItem;

            MessageBoxResult answer = MessageBox.Show(String.Format("You really want to delete driver {0}?",
                            String.Format("{0} {1}", driver.Name, driver.Surname)), "Deleting", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (answer == MessageBoxResult.Yes)
            {
                driverRepository.Delete(driver.Id);
                dgrDataDrivers.ItemsSource = driverRepository.SelectAll();
            }
        }
    }
}