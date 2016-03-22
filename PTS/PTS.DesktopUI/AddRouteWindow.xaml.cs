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
    /// Interaction logic for AddRouteWindow.xaml
    /// </summary>
    public partial class AddRouteWindow : Window
    {
        private Customer _customer;

        private string _connectionString = ConfigurationManager.ConnectionStrings["PTS"].ConnectionString;

        public AddRouteWindow(Customer customer)
        {
            InitializeComponent();

            var stationRepository = new SqlStationRepository(_connectionString);

            List<string> stations = stationRepository.GetAllNames();

            dgrStation.ItemsSource = stations;
            dgrAddedStation.ItemsSource = new List<string>();

            _customer = customer;

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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
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
            var newRoute = new Route()
            {
                Number = number,
                Distance = distance,
                Duration = duration,
                Price = price
            };

            try
            {
                int id = routeRepository.Insert(_customer.Id, newRoute);
                for (var i = 0; i < addedStations.Count; i++)
                {
                    routeRepository.AddStationToRoute(number, addedStations[i], i + 1);
                }

                txtNumber.Text = "";
                txtDuration.Text = "";
                txtDistance.Text = "";
                txtPrice.Text = "";

                var stationRepository = new SqlStationRepository(_connectionString);
                dgrStation.ItemsSource = stationRepository.GetAllNames();

                dgrAddedStation.ItemsSource = new List<string>();

                MessageBox.Show(String.Format("Route {0} added with id = {1}.", number, id), "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Number error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
