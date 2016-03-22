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
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["PTS"].ConnectionString;

        private Customer _customer;

        public StationWindow(Customer customer, List<BusStation> stations)
        {
            InitializeComponent();

            _customer = customer;
            if (_customer.Mode == Mode.User)
            {
                btnAdd.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
            }

            var routeRepository = new SqlRouteRepository(_connectionString);

            List<string> routes = routeRepository.GetAllNumbers();
            routes.Insert(0, "");

            cmbRoute.ItemsSource = routes;
            cmbRoute.SelectedItem = "";

            var busRepository = new SqlBusRepository(_connectionString);

            List<string> buses = busRepository.GetAllNumbers();
            buses.Insert(0, "");

            cmbBus.ItemsSource = buses;
            cmbBus.SelectedItem = "";

            dgrDataStations.ItemsSource = stations;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addStationWindow = new AddStationWindow();
            wndStation.IsEnabled = false;
            addStationWindow.ShowDialog();

            var stationRepository = new SqlStationRepository(_connectionString);
            dgrDataStations.ItemsSource = stationRepository.SelectAll();

            wndStation.IsEnabled = true;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string routeNumber = String.IsNullOrEmpty((string)cmbRoute.SelectedItem) ? null : (string)cmbRoute.SelectedItem;

            string busNumber = String.IsNullOrEmpty((string)cmbBus.SelectedItem) ? null : (string)cmbBus.SelectedItem;

            var stationSqlRepository = new SqlStationRepository(_connectionString);

            dgrDataStations.ItemsSource = stationSqlRepository.SearchAll(routeNumber, busNumber);
        }

        private void btnTimeTable_Click(object sender, RoutedEventArgs e)
        {
            var scheduleRepository = new SqlScheduleRepository(_connectionString);

            if (dgrDataStations.SelectedItem == null)
            {
                MessageBox.Show("Not selected station.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int stationId = ((BusStation)dgrDataStations.SelectedItem).Id;

            var scheduleWindow = new ScheduleWindow(_customer, scheduleRepository.GetTimeTableByStation(stationId));
            wndStation.Close();
            scheduleWindow.ShowDialog();
        }

        private void btnBus_Click(object sender, RoutedEventArgs e)
        {
            var busRepository = new SqlBusRepository(_connectionString);

            if (dgrDataStations.SelectedItem == null)
            {
                MessageBox.Show("Not selected station.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int stationId = ((BusStation)dgrDataStations.SelectedItem).Id;

            var busWindow = new BusWindow(_customer, busRepository.GetBusesByStation(stationId));
            wndStation.Close();
            busWindow.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var stationRepository = new SqlStationRepository(_connectionString);

            if (dgrDataStations.SelectedItem == null)
            {
                MessageBox.Show("Not selected station.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int stationId = ((BusStation)dgrDataStations.SelectedItem).Id;

            MessageBoxResult answer = MessageBox.Show(String.Format("You really want to delete bus {0}?",
                            ((BusStation)dgrDataStations.SelectedItem).Name), "Deleting", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (answer == MessageBoxResult.Yes)
            {
                stationRepository.Delete(stationId);
                dgrDataStations.ItemsSource = stationRepository.SelectAll();
            }
        }
    }
}
