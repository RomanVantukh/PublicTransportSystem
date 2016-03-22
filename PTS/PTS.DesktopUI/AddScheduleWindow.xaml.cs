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
    /// Interaction logic for AddScheduleWindow.xaml
    /// </summary>
    public partial class AddScheduleWindow : Window
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["PTS"].ConnectionString;

        public AddScheduleWindow()
        {
            InitializeComponent();
            txtBusNumber.Focus();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtBusNumber.Text))
            {
                MessageBox.Show("No bus number", "Empty field", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string busNumber = txtBusNumber.Text;

            if (String.IsNullOrEmpty(txtDepartureTime.Text))
            {
                MessageBox.Show("No departure time", "Empty field", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TimeSpan departureTime = new TimeSpan();
            if (!TimeParser.ParseTime(txtDepartureTime.Text, out departureTime))
            {
                MessageBox.Show("Departure time is not time", "Cast error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var scheduleRepository = new SqlScheduleRepository(_connectionString);

            try
            {
                int id = scheduleRepository.Insert(busNumber, departureTime);

                txtBusNumber.Text = "";
                txtDepartureTime.Text = "";

                MessageBox.Show(String.Format("Schedule id = {0}.", id), "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Number error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
