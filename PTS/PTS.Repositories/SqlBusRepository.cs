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
    public interface IBusRepository
    {
        List<Bus> SelectAll();
        List<Bus> SearchAll(string routeNumber, string stationName);
        List<string> GetAllNumbers();
        List<Bus> GetBusesByRoute(int routeId);
        List<Bus> GetBusesByStation(int stationId);
        int Insert(int customerId, Bus bus);
        void Delete(int busId);
    }

    public class SqlBusRepository : SqlBaseRepository, IBusRepository
    {
        public SqlBusRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<Bus> SelectAll()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spSelectAllBuses";

                    var buses = new List<Bus>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var bus = new Bus()
                            {
                                Id = (int)reader["Id"],
                                Number = (string)reader["BusNumber"],
                                RouteNumber = (string)reader["RouteNumber"],
                                Model = (string)reader["Model"],
                                Customer = (string)reader["Customer"]
                            };

                            buses.Add(bus);
                        }
                    }

                    return buses;
                }
            }
        }

        public List<Bus> GetBusesByRoute(int routeId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetBusesByRoute";
                    command.Parameters.AddWithValue("@routeId", routeId);

                    var buses = new List<Bus>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var route = new Bus()
                            {
                                Id = (int)reader["Id"],
                                Number = (string)reader["BusNumber"],
                                RouteNumber = (string)reader["RouteNumber"],
                                Model = (string)reader["Model"],
                                Customer = (string)reader["Customer"]
                            };

                            buses.Add(route);
                        }
                    }
                    return buses;
                }
            }
        }

        public List<Bus> SearchAll(string routeNumber, string stationName)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetBusesBySearch";

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

                    var buses = new List<Bus>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var bus = new Bus()
                            {
                                Id = (int)reader["Id"],
                                Number = (string)reader["BusNumber"],
                                RouteNumber = (string)reader["RouteNumber"],
                                Model = (string)reader["Model"],
                                Customer = (string)reader["Customer"]
                            };

                            buses.Add(bus);
                        }
                    }

                    return buses;
                }
            }
        }

        public List<string> GetAllNumbers()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetBusNumbers";

                    var numbers = new List<string>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            numbers.Add((string)reader["Number"]);
                        }
                    }

                    return numbers;
                }
            }
        }

        public List<Bus> GetBusesByStation(int stationId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetBusesByStation";
                    command.Parameters.AddWithValue("@stationId", stationId);

                    var buses = new List<Bus>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var route = new Bus()
                            {
                                Id = (int)reader["Id"],
                                Number = (string)reader["BusNumber"],
                                RouteNumber = (string)reader["RouteNumber"],
                                Model = (string)reader["Model"],
                                Customer = (string)reader["Customer"]
                            };

                            buses.Add(route);
                        }
                    }
                    return buses;
                }
            }
        }

        public int Insert(int customerId, Bus bus)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spInsertBus";

                    command.Parameters.AddWithValue("@busNumber", bus.Number);
                    command.Parameters.AddWithValue("@routeNumber", bus.RouteNumber);
                    command.Parameters.AddWithValue("@model", bus.Model);
                    command.Parameters.AddWithValue("@customerId", customerId);
                    var idParameter = new SqlParameter("@id", SqlDbType.Int);
                    idParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(idParameter);

                    command.ExecuteNonQuery();

                    return (int)idParameter.Value;
                }
            }
        }

        public void Delete(int busId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spDeleteBus";
                    command.Parameters.AddWithValue("@busId", busId);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
