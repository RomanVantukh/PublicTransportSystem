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
    /// Interaction logic for Routes.xaml
    /// </summary>
    public partial class RouteWindow : Window
    {
        private Customer _customer;

        private string _connectionString = ConfigurationManager.ConnectionStrings["PTS"].ConnectionString;

        public RouteWindow(Customer customer, List<Route> routes)
        {
            InitializeComponent();

            _customer = customer;
            if (_customer.Mode == Mode.User)
            {
                btnAdd.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
                btnModify.Visibility = Visibility.Hidden;
            }

            var stationRepository = new SqlStationRepository(_connectionString);

            List<string> stations = stationRepository.GetAllNames();
            stations.Insert(0, "");

            cmbStation.ItemsSource = stations;
            cmbStation.SelectedItem = "";

            dgrDataRoutes.ItemsSource = routes;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addRouteWindow = new AddRouteWindow(_customer);
            wndRoute.IsEnabled = false;
            addRouteWindow.ShowDialog();

            var routeRepository = new SqlRouteRepository(_connectionString);
            dgrDataRoutes.ItemsSource = routeRepository.SelectAll();

            wndRoute.IsEnabled = true;
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            if (dgrDataRoutes.SelectedItem == null)
            {
                MessageBox.Show("Not selected route.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Route route = (Route)dgrDataRoutes.SelectedItem;

            var modifyRouteWindow = new ModifyRouteWindow(_customer, route);
            modifyRouteWindow.ShowDialog();

            var routeRepository = new SqlRouteRepository(_connectionString);
            dgrDataRoutes.ItemsSource = routeRepository.SelectAll();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string stationName = String.IsNullOrEmpty((string)cmbStation.SelectedItem) ? null : (string)cmbStation.SelectedItem;

            int? maxPrice = null;
            if (!String.IsNullOrEmpty(txtMaxPrice.Text))
            {
                int max;
                if (int.TryParse(txtMaxPrice.Text, out max))
                {
                    maxPrice = max;
                }
                else
                {
                    MessageBox.Show("Price is not integer.", "Cast error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            int? maxDuration = null;
            if (!String.IsNullOrEmpty(txtMaxDuration.Text))
            {
                int max;
                if (int.TryParse(txtMaxDuration.Text, out max))
                {
                    maxDuration = max;
                }
                else
                {
                    MessageBox.Show("Duration is not integer.", "Cast error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            var routeSqlRepository = new SqlRouteRepository(_connectionString);

            dgrDataRoutes.ItemsSource = routeSqlRepository.SearchAll(stationName, maxPrice, maxDuration);
        }

        private void btnBuses_Click(object sender, RoutedEventArgs e)
        {
            var busRepository = new SqlBusRepository(_connectionString);

            if (dgrDataRoutes.SelectedItem == null)
            {
                MessageBox.Show("Not selected route.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int routeId = ((Route)dgrDataRoutes.SelectedItem).Id;

            var busWindow = new BusWindow(_customer, busRepository.GetBusesByRoute(routeId));
            wndRoute.Close();
            busWindow.ShowDialog();
        }

        private void btnStations_Click(object sender, RoutedEventArgs e)
        {
            var stationRepository = new SqlStationRepository(_connectionString);

            if (dgrDataRoutes.SelectedItem == null)
            {
                MessageBox.Show("Not selected route.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int routeId = ((Route)dgrDataRoutes.SelectedItem).Id;

            var stationWindow = new StationWindow(_customer, stationRepository.GetStationByRoute(routeId));
            wndRoute.Close();
            stationWindow.ShowDialog();
        }

        private void btnTimeTable_Click(object sender, RoutedEventArgs e)
        {
            var scheduleRepository = new SqlScheduleRepository(_connectionString);

            if (dgrDataRoutes.SelectedItem == null)
            {
                MessageBox.Show("Not selected route.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int routeId = ((Route)dgrDataRoutes.SelectedItem).Id;

            var scheduleWindow = new ScheduleWindow(_customer, scheduleRepository.GetTimeTableByRoute(routeId));
            wndRoute.Close();
            scheduleWindow.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var routeRepository = new SqlRouteRepository(_connectionString);

            if (dgrDataRoutes.SelectedItem == null)
            {
                MessageBox.Show("Not selected route.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int routeId = ((Route)dgrDataRoutes.SelectedItem).Id;

            MessageBoxResult answer = MessageBox.Show(String.Format("You really want to delete route {0} and all related objects?", 
                            ((Route)dgrDataRoutes.SelectedItem).Number), "Deleting", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (answer == MessageBoxResult.Yes)
            {
                routeRepository.Delete(routeId);
                dgrDataRoutes.ItemsSource = routeRepository.SelectAll();
            }
        }
    }
}
