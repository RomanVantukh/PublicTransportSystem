using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using PTS.Entities;

namespace PTS.Repositories
{
    public interface IDriverRepository
    {
        List<Driver> SelectAll();
        List<Driver> SearchAll(string routeNumber, string stationName);
        List<Driver> GetDriversBySchedule(int scheduleId);
        int Insert(int customerId, Driver driver);
        void Delete(int driverId);
    }

    public class SqlDriverRepository : SqlBaseRepository, IDriverRepository
    {
        public SqlDriverRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<Driver> SelectAll()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spSelectAllDrivers";

                    var drivers = new List<Driver>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var driver = new Driver()
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                Surname = (string)reader["Surname"],
                                BusNumber = (string)reader["BusNumber"],
                                RouteNumber = (string)reader["RouteNumber"],
                                Customer = (string)reader["Customer"]
                            };

                            drivers.Add(driver);
                        }
                    }

                    return drivers;
                }
            }
        }

        public List<Driver> SearchAll(string routeNumber, string stationName)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetDriversBySearch";

                    SqlParameter name = new SqlParameter();
                    name.ParameterName = "@stationName";
                    name.DbType = DbType.String;
                    name.IsNullable = true;
                    name.Value = stationName;
                    command.Parameters.Add(name);

                    SqlParameter number = new SqlParameter();
                    number.ParameterName = "@routeNumber";
                    number.DbType = DbType.String;
                    number.IsNullable = true;
                    number.Value = routeNumber;
                    command.Parameters.Add(number);

                    var drivers = new List<Driver>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var driver = new Driver()
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                Surname = (string)reader["Surname"],
                                RouteNumber = (string)reader["RouteNumber"],
                                BusNumber = (string)reader["BusNumber"],
                                Customer = (string)reader["Customer"]
                            };

                            drivers.Add(driver);
                        }
                    }

                    return drivers;
                }
            }
        }

        public List<Driver> GetDriversBySchedule(int scheduleId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetDriversBySchedule";
                    command.Parameters.AddWithValue("@scheduleId", scheduleId);

                    var drivers = new List<Driver>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var driver = new Driver()
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                Surname = (string)reader["Surname"],
                                RouteNumber = (string)reader["RouteNumber"],
                                BusNumber = (string)reader["BusNumber"],
                                Customer = (string)reader["Customer"]
                            };

                            drivers.Add(driver);
                        }
                    }

                    return drivers;
                }
            }
        }

        public int Insert(int customerId, Driver driver)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spInsertDriver";

                    command.Parameters.AddWithValue("@busNumber", driver.BusNumber);
                    command.Parameters.AddWithValue("@name", driver.Name);
                    command.Parameters.AddWithValue("@surname", driver.Surname);
                    command.Parameters.AddWithValue("@customerId", customerId);
                    var idParameter = new SqlParameter("@id", SqlDbType.Int);
                    idParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(idParameter);

                    command.ExecuteNonQuery();

                    return (int)idParameter.Value;
                }
            }
        }

        public void Delete(int driverId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spDeleteDriver";
                    command.Parameters.AddWithValue("@driverId", driverId);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
