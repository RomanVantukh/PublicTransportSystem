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
    /// Interaction logic for AddDriverWindow.xaml
    /// </summary>
    public partial class AddDriverWindow : Window
    {
        private Customer _customer;

        private string _connectionString = ConfigurationManager.ConnectionStrings["PTS"].ConnectionString;

        public AddDriverWindow(Customer customer)
        {
            InitializeComponent();

            _customer = customer;

            txtName.Focus();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("No driver name", "Empty field", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string driverName = txtName.Text;

            if (String.IsNullOrEmpty(txtSurname.Text))
            {
                MessageBox.Show("No driver surrname", "Empty field", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string driverSurname = txtSurname.Text;

            if (String.IsNullOrEmpty(txtBusNumber.Text))
            {
                MessageBox.Show("No bus number", "Empty field", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string busNumber = txtBusNumber.Text;

            var driverRepository = new SqlDriverRepository(_connectionString);

            var driver = new Driver()
            {
                Name = driverName, 
                Surname = driverSurname,
                BusNumber = busNumber
            };

            try
            {
                int id = driverRepository.Insert(_customer.Id, driver);

                txtName.Text = "";
                txtSurname.Text = "";
                txtBusNumber.Text = "";

                MessageBox.Show(String.Format("Driver {0} {1} added with id = {1}.", driverName, driverSurname, id), "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Number error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
