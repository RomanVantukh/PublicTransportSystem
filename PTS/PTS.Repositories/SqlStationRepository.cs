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
    public interface IStationRepository
    {
        List<BusStation> SelectAll();
        List<BusStation> SearchAll(string routeNumber, string busNumber);
        List<string> GetAllNames();
        List<BusStation> GetStationByRoute(int routeId);
        List<BusStation> GetStationByBus(int busId);
        List<BusStation> GetStationBySchedule(int scheduleId);
        List<BusStation> GetStationByDriver(int driverId);
        int Insert(string stationName);
        void Delete(int stationId);
    }

    public class SqlStationRepository : SqlBaseRepository, IStationRepository
    {
        public SqlStationRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<string> GetAllNames()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetStationName";

                    var routes = new List<string>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            routes.Add((string)reader["Name"]);
                        }
                    }

                    return routes;
                }
            }
        }

        public List<BusStation> SelectAll()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spSelectAllStations";

                    var stations = new List<BusStation>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var station = new BusStation()
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"]
                            };

                            stations.Add(station);
                        }
                    }

                    return stations;
                }
            }
        }

        public List<BusStation> SearchAll(string routeNumber, string busNumber)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetStationsBySearch";

                    SqlParameter busNumberParameter = new SqlParameter();
                    busNumberParameter.ParameterName = "@busNumber";
                    busNumberParameter.DbType = DbType.String;
                    busNumberParameter.IsNullable = true;
                    busNumberParameter.Value = busNumber;
                    command.Parameters.Add(busNumberParameter);

                    SqlParameter number = new SqlParameter();
                    number.ParameterName = "@routeNumber";
                    number.DbType = DbType.String;
                    number.IsNullable = true;
                    number.Value = routeNumber;
                    command.Parameters.Add(number);

                    var stations = new List<BusStation>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var station = new BusStation()
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"]
                            };

                            stations.Add(station);
                        }
                    }

                    return stations;
                }
            }
        }

        public List<BusStation> GetStationByRoute(int routeId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetStationsByRoute";
                    command.Parameters.AddWithValue("@routeId", routeId);

                    var stations = new List<BusStation>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var station = new BusStation()
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"]
                            };

                            stations.Add(station);
                        }
                    }
                    return stations;
                }
            }
        }

        public List<BusStation> GetStationByBus(int busId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetStationsByBus";
                    command.Parameters.AddWithValue("@busId", busId);

                    var stations = new List<BusStation>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var station = new BusStation()
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"]
                            };

                            stations.Add(station);
                        }
                    }
                    return stations;
                }
            }
        }

        public List<BusStation> GetStationBySchedule(int scheduleId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetStationsBySchedule";
                    command.Parameters.AddWithValue("@scheduleId", scheduleId);

                    var stations = new List<BusStation>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var station = new BusStation()
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"]
                            };

                            stations.Add(station);
                        }
                    }
                    return stations;
                }
            }
        }

        public List<BusStation> GetStationByDriver(int driverId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetStationsByDriver";
                    command.Parameters.AddWithValue("@driverId", driverId);

                    var stations = new List<BusStation>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var station = new BusStation()
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"]
                            };

                            stations.Add(station);
                        }
                    }
                    return stations;
                }
            }
        }

        public int Insert(string stationName)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spInsertStation";

                    command.Parameters.AddWithValue("@name", stationName);
                    var idParameter = new SqlParameter("@id", SqlDbType.Int);
                    idParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(idParameter);

                    command.ExecuteNonQuery();

                    return (int)idParameter.Value;
                }
            }
        }

        public void Delete(int stationId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spDeleteStation";
                    command.Parameters.AddWithValue("@stationId", stationId);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}