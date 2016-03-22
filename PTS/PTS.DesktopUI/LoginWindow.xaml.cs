using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PTS.Repositories;
using PTS.Entities;
using System.Configuration;

namespace PTS.DesktopUI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private Customer _customer;

        public LoginWindow()
        {
            InitializeComponent();
            txtLogin.Focus();
        }

        public Customer GetCustomer()
        {
            return _customer;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtLogin.Text))
            {
                MessageBox.Show("Please input login.");
                return;
            }

            string login = txtLogin.Text;

            if (String.IsNullOrEmpty(pasPassword.Password))
            {
                MessageBox.Show("Please input password.");
                return;
            }

            string password = pasPassword.Password;

            string connectionString = ConfigurationManager.ConnectionStrings["PTS"].ConnectionString;

            SqlCustomerRepository customerRepository = new SqlCustomerRepository(connectionString);

            _customer = customerRepository.LogIn(login, password) ?? _customer;

            if (_customer == null)
            {
                MessageBox.Show("Customer with this login and password doesn't exist.", "Wrong login or password", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                DialogResult = true;
            }
        }
    }
}
