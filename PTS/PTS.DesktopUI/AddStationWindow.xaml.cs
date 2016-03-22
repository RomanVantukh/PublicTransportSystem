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
    /// Interaction logic for AddStationWindow.xaml
    /// </summary>
    public partial class AddStationWindow : Window
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["PTS"].ConnectionString;

        public AddStationWindow()
        {
            InitializeComponent();
            txtName.Focus();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("No station name", "Empty field", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string stationName = txtName.Text;

            var stationRepository = new SqlStationRepository(_connectionString);

            try
            {
                int id = stationRepository.Insert(stationName);

                txtName.Text = "";

                MessageBox.Show(String.Format("Station {0} added with id = {1}.", stationName, id), "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Number error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
