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
    /// Interaction logic for ModifyRouteWindow.xaml
    /// </summary>
    public partial class ModifyRouteWindow : Window
    {
        private Customer _customer;

        private string _connectionString = ConfigurationManager.ConnectionStrings["PTS"].ConnectionString;

        private Route _route;

        private List<BusStation> _stations;

        public ModifyRouteWindow(Customer customer, Route route)
        {
            InitializeComponent();

            _customer = customer;
            _route = route;

            txtNumber.Text = _route.Number;
            txtDistance.Text = _route.Distance.ToString();
            txtDuration.Text = _route.Duration.ToString();
            txtPrice.Text = _route.Price.ToString();

            var stationRepository = new SqlStationRepository(_connectionString);
            _stations = stationRepository.GetStationByRoute(_route.Id);
            List<string> allStationName = stationRepository.GetAllNames();

            List<string> addedStations = new List<string>();
            foreach (BusStation station in _stations)
            {
                addedStations.Add(station.Name);
            }

            List<string> notAddedStations = new List<string>();

            for (var i = 0; i < allStationName.Count; i++)
            {
                if (!addedStations.Contains(allStationName[i]))
                {
                    notAddedStations.Add(allStationName[i]);
                }
            }

            dgrAddedStation.ItemsSource = addedStations;
            dgrStation.ItemsSource = notAddedStations;

            txtNumber.Focus();
        }

        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            if (dgrStation.SelectedItem == null)
            {
                MessageBox.Show("Not selected station.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string stationName = (string)dgrStation.SelectedItem;

            List<string> stations = (List<string>)dgrStation.ItemsSource;
            stations.Remove(stationName);

            List<string> addedStations = (List<string>)dgrAddedStation.ItemsSource;
            addedStations.Add(stationName);

            dgrAddedStation.ItemsSource = new List<string>(addedStations);
            dgrStation.ItemsSource = new List<string>(stations);
        }

        private void btnDeleteStation_Click(object sender, RoutedEventArgs e)
        {
            if (dgrAddedStation.SelectedItem == null)
            {
                MessageBox.Show("Not selected station.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string stationName = (string)dgrAddedStation.SelectedItem;

            List<string> addedStations = (List<string>)dgrAddedStation.ItemsSource;
            addedStations.Remove(stationName);

            List<string> stations = (List<string>)dgrStation.ItemsSource;
            stations.Add(stationName);

            dgrAddedStation.ItemsSource = new List<string>(addedStations);
            dgrStation.ItemsSource = new List<string>(stations);
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtNumber.Text))
            {
                MessageBox.Show("No name", "Empty field", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string number = txtNumber.Text;

            if (String.IsNullOrEmpty(txtDistance.Text))
            {
                MessageBox.Show("No distance", "Empty field", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int distance = 0;
            if (!int.TryParse(txtDistance.Text, out distance))
            {
                MessageBox.Show("Distance is not integer", "Cast error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (distance <= 0)
            {
                MessageBox.Show("Distance <= 0", "Cast error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int duration = 0;
            if (!int.TryParse(txtDuration.Text, out duration))
            {
                MessageBox.Show("Duration is not integer", "Cast error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (duration <= 0)
            {
                MessageBox.Show("Duration <= 0", "Cast error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int price = 0;
            if (!int.TryParse(txtPrice.Text, out price))
            {
                MessageBox.Show("Price is not integer", "Cast error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (price <= 0)
            {
                MessageBox.Show("Price <= 0", "Cast error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<string> addedStations = (List<string>)dgrAddedStation.ItemsSource;

            if (addedStations.Count < 2)
            {
                MessageBox.Show("Route must have at least 2 stations", "Stations error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var routeRepository = new SqlRouteRepository(_connectionString);

            number = number == _route.Number ? null : number;

            int? distanceRoute = null;
            if (distance != _route.Distance)
            {
                distanceRoute = distance;
            }

            int? durationRoute = null;
            if (duration != _route.Duration)
            {
                durationRoute = duration;
            }

            int? priceRoute = null;
            if (price != _route.Price)
            {
                priceRoute = price;
            }

            try
            {
                routeRepository.Update(_customer.Id, _route.Id, number, durationRoute, distanceRoute, priceRoute);
                for (var i = 0; i < _stations.Count; i++)
                {
                    routeRepository.DeleteStationToRoute(_stations[i].Name, _route.Id);
                }

                for (var i = 0; i < addedStations.Count; i++)
                {
                    routeRepository.AddStationToRoute(_route.Number, addedStations[i], i + 1);
                }

                wndModifyRoute.Close();
                if (number != null || durationRoute != null || distanceRoute != null || priceRoute != null)
                {
                    MessageBox.Show(String.Format("Route {0} updated.", number), "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Number error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}