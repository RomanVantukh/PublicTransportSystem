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
    /// Interaction logic for ScheduleWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["PTS"].ConnectionString;

        private Customer _customer;

        public ScheduleWindow(Customer customer, List<TimeTable> schedule)
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

            dgrDataSchedule.ItemsSource = schedule;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addScheduleWindow = new AddScheduleWindow();
            wndSchedule.IsEnabled = false;
            addScheduleWindow.ShowDialog();

            var scheduleRepository = new SqlScheduleRepository(_connectionString);
            dgrDataSchedule.ItemsSource = scheduleRepository.SelectAll();

            wndSchedule.IsEnabled = true;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string stationName = String.IsNullOrEmpty((string)cmbStation.SelectedItem) ? null : (string)cmbStation.SelectedItem;

            string routeNumber = String.IsNullOrEmpty((string)cmbRoute.SelectedItem) ? null : (string)cmbRoute.SelectedItem;

            TimeSpan? from = null;
            if (!String.IsNullOrEmpty(txtFrom.Text))
            {
                TimeSpan fromTime;
                if (TimeParser.ParseTime(txtFrom.Text, out fromTime))
                {
                    from = fromTime;
                }
                else
                {
                    MessageBox.Show("From is not time.", "Cast error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            TimeSpan? to = null;
            if (!String.IsNullOrEmpty(txtTo.Text))
            {
                TimeSpan toTime;
                if (TimeParser.ParseTime(txtTo.Text, out toTime))
                {
                    to = toTime;
                }
                else
                {
                    MessageBox.Show("To is not time.", "Cast error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            var scheduleSqlRepository = new SqlScheduleRepository(_connectionString);

            dgrDataSchedule.ItemsSource = scheduleSqlRepository.SearchAll(routeNumber, stationName, from, to);
        }

        private void btnStation_Click(object sender, RoutedEventArgs e)
        {
            var stationRepository = new SqlStationRepository(_connectionString);

            if (dgrDataSchedule.SelectedItem == null)
            {
                MessageBox.Show("Not selected schedule.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int scheduleId = ((TimeTable)dgrDataSchedule.SelectedItem).Id;

            var stationWindow = new StationWindow(_customer, stationRepository.GetStationBySchedule(scheduleId));
            wndSchedule.Close();
            stationWindow.ShowDialog();
        }

        private void btnDriver_Click(object sender, RoutedEventArgs e)
        {
            var driverRepository = new SqlDriverRepository(_connectionString);

            if (dgrDataSchedule.SelectedItem == null)
            {
                MessageBox.Show("Not selected schedule.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int scheduleId = ((TimeTable)dgrDataSchedule.SelectedItem).Id;

            var driverWindow = new DriverWindow(_customer, driverRepository.GetDriversBySchedule(scheduleId));
            wndSchedule.Close();
            driverWindow.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var scheduleRepository = new SqlScheduleRepository(_connectionString);

            if (dgrDataSchedule.SelectedItem == null)
            {
                MessageBox.Show("Not selected schedule.", "Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int scheduleId = ((TimeTable)dgrDataSchedule.SelectedItem).Id;

            MessageBoxResult answer = MessageBox.Show(String.Format("You really want to delete bus #{0}?",
                            ((TimeTable)dgrDataSchedule.SelectedItem).Id), "Deleting", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (answer == MessageBoxResult.Yes)
            {
                scheduleRepository.Delete(scheduleId);
                dgrDataSchedule.ItemsSource = scheduleRepository.SelectAll();
            }
        }
    }
}