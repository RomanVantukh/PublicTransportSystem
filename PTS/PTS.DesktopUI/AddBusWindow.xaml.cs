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
    /// Interaction logic for AddBusWindow.xaml
    /// </summary>
    public partial class AddBusWindow : Window
    {
        private Customer _customer;

        private string _connectionString = ConfigurationManager.ConnectionStrings["PTS"].ConnectionString;

        public AddBusWindow(Customer customer)
        {
            InitializeComponent();

            _customer = customer;

            txtNumber.Focus();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtNumber.Text))
            {
                MessageBox.Show("No bus number", "Empty field", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string busNumber = txtNumber.Text;

            if (String.IsNullOrEmpty(txtRouteNumner.Text))
            {
                MessageBox.Show("No route number", "Empty field", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string routeNumber = txtRouteNumner.Text;

            if (String.IsNullOrEmpty(txtModel.Text))
            {
                MessageBox.Show("No bus model", "Empty field", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string busModel = txtModel.Text;

            var busRepository = new SqlBusRepository(_connectionString);

            var bus = new Bus()
            {
                Number = busNumber,
                RouteNumber = routeNumber,
                Model = busModel
            };

            try
            {
                int id = busRepository.Insert(_customer.Id, bus);

                txtNumber.Text = "";
                txtRouteNumner.Text = "";
                txtModel.Text = "";

                MessageBox.Show(String.Format("Bus {0} added with id = {1}.", busNumber, id), "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message, "Number error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
