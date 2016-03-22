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
    /// Interaction logic for BusWindow.xaml
    /// </summary>
    public partial class BusWindow : Window
    {
        private Customer _customer;

        private string _connectionString = ConfigurationManager.ConnectionStrings["PTS"].ConnectionString;

        public BusWindow(Customer customer, List<Bus> buses)
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

            dgrDataBuses.ItemsSource = buses;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addBusWindow = new AddBusWindow(_customer);
            wndBus.IsEnabled = false;
            addBusWindow.ShowDialog();

            var busRepository = new SqlBusRepository(_connectionString);
            dgrDataBuses.ItemsSource = busRepository.SelectAll();

            wndBus.IsEnabled = true;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string stationName = String.IsNullOrEmpty((string)cmbStation.SelectedItem) ? null : (string)cmbStation.SelectedItem;

            string routeNumber = String.IsNullOrEmpty((string)cmbRoute.SelectedItem) ? null : (string)cmbRoute.SelectedItem;

            var busSqlRepository = new SqlBusRepository(_connectionString);

            dgrDataBuses.ItemsSource = busSqlRepository.SearchAll(routeNumber, stationName);
        }

        private void btnTimeTable_Click(object sender, RoutedEventArgs e)
        {
            var scheduleRepository = new SqlScheduleRepository(_connectionString);

            if (dgrDataBuses.SelectedItem == null)
            {
                MessageBox.Show("Not selected bus.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int busId = ((Bus)dgrDataBuses.SelectedItem).Id;

            var scheduleWindow = new ScheduleWindow(_customer, scheduleRepository.GetTimeTableByBus(busId));
            wndBus.Close();
            scheduleWindow.ShowDialog();
        }

        private void btnStations_Click(object sender, RoutedEventArgs e)
        {
            var stationRepository = new SqlStationRepository(_connectionString);

            if (dgrDataBuses.SelectedItem == null)
            {
                MessageBox.Show("Not selected bus.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int busId = ((Bus)dgrDataBuses.SelectedItem).Id;

            var stationWindow = new StationWindow(_customer, stationRepository.GetStationByBus(busId));
            wndBus.Close();
            stationWindow.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var busRepository = new SqlBusRepository(_connectionString);

            if (dgrDataBuses.SelectedItem == null)
            {
                MessageBox.Show("Not selected bus.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int busId = ((Bus)dgrDataBuses.SelectedItem).Id;

            MessageBoxResult answer = MessageBox.Show(String.Format("You really want to delete bus {0} and all related objects?",
                            ((Bus)dgrDataBuses.SelectedItem).Number), "Deleting", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (answer == MessageBoxResult.Yes)
            {
                busRepository.Delete(busId);
                dgrDataBuses.ItemsSource = busRepository.SelectAll();
            }
        }
    }
}
