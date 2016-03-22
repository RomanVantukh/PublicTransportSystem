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
    public interface IScheduleRepository
    {
        List<TimeTable> SelectAll();
        List<TimeTable> SearchAll(string routeNumber, string stationName, TimeSpan? from, TimeSpan? to);
        List<TimeTable> GetTimeTableByRoute(int routeId);
        List<TimeTable> GetTimeTableByBus(int busId);
        List<TimeTable> GetTimeTableByStation(int stationId);
        List<TimeTable> GetTimeTableByDriver(int driverId);
        int Insert(string busNumber, TimeSpan departureTime);
        void Delete(int scheduleId);
    }

    public class SqlScheduleRepository : SqlBaseRepository, IScheduleRepository
    {
        public SqlScheduleRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<TimeTable> SelectAll()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spSelectAllTimeTable";

                    var schedule = new List<TimeTable>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var timeTable = new TimeTable()
                            {
                                Id = (int)reader["Id"],
                                RouteNumber = (string)reader["RouteNumber"],
                                DepartureTime = (TimeSpan)reader["DepartureTime"],
                                Duration = (int)reader["Duration"],
                                ArrivalTime = (TimeSpan)reader["ArrivalTime"]
                            };

                            schedule.Add(timeTable);
                        }
                    }

                    return schedule;
                }
            }
        }

        public List<TimeTable> SearchAll(string routeNumber, string stationName, TimeSpan? from, TimeSpan? to)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetScheduleBySearch";

                    SqlParameter name = new SqlParameter();
                    name.ParameterName = "@stationName";
                    name.DbType = DbType.String;
                    name.IsNullable = true;
                    name.Value = stationName;
                    command.Parameters.Add(name);

                    SqlParameter route = new SqlParameter();
                    route.ParameterName = "@routeNumber";
                    route.DbType = DbType.String;
                    route.IsNullable = true;
                    route.Value = routeNumber;
                    command.Parameters.Add(route);

                    SqlParameter fromParameter = new SqlParameter();
                    fromParameter.ParameterName = "@from";
                    fromParameter.SqlValue = from;
                    fromParameter.IsNullable = true;
                    //fromParameter.Value = from;
                    command.Parameters.Add(fromParameter);

                    SqlParameter toParameter = new SqlParameter();
                    toParameter.ParameterName = "@to";
                    toParameter.SqlValue = to;
                    toParameter.IsNullable = true;
                    //toParameter.Value = to;
                    command.Parameters.Add(toParameter);

                    var schedule = new List<TimeTable>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var timeTable = new TimeTable();
                            timeTable.Id = (int)reader["Id"];
                            timeTable.RouteNumber = (string)reader["RouteNumber"];
                            timeTable.DepartureTime = (TimeSpan)reader["DepartureTime"];
                            timeTable.Duration = (int)reader["Duration"];
                            timeTable.ArrivalTime = (TimeSpan)reader["ArrivalTime"];

                            schedule.Add(timeTable);
                        }
                    }

                    return schedule;
                }
            }
        }

        public List<TimeTable> GetTimeTableByRoute(int routeId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetScheduleByRoute";
                    command.Parameters.AddWithValue("@routeId", routeId);

                    var schedule = new List<TimeTable>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var timeTable = new TimeTable()
                            {
                                Id = (int)reader["Id"],
                                RouteNumber = (string)reader["RouteNumber"],
                                DepartureTime = (TimeSpan)reader["DepartureTime"],
                                Duration = (int)reader["Duration"],
                                ArrivalTime = (TimeSpan)reader["ArrivalTime"]
                            };

                            schedule.Add(timeTable);
                        }
                    }
                    return schedule;
                }
            }
        }

        public List<TimeTable> GetTimeTableByBus(int busId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetScheduleByBus";
                    command.Parameters.AddWithValue("@busId", busId);

                    var schedule = new List<TimeTable>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var timeTable = new TimeTable()
                            {
                                Id = (int)reader["Id"],
                                RouteNumber = (string)reader["RouteNumber"],
                                DepartureTime = (TimeSpan)reader["DepartureTime"],
                                Duration = (int)reader["Duration"],
                                ArrivalTime = (TimeSpan)reader["ArrivalTime"]
                            };

                            schedule.Add(timeTable);
                        }
                    }
                    return schedule;
                }
            }
        }

        public List<TimeTable> GetTimeTableByStation(int stationId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetScheduleByStation";
                    command.Parameters.AddWithValue("@stationId", stationId);

                    var schedule = new List<TimeTable>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var timeTable = new TimeTable()
                            {
                                Id = (int)reader["Id"],
                                RouteNumber = (string)reader["RouteNumber"],
                                DepartureTime = (TimeSpan)reader["DepartureTime"],
                                Duration = (int)reader["Duration"],
                                ArrivalTime = (TimeSpan)reader["ArrivalTime"]
                            };

                            schedule.Add(timeTable);
                        }
                    }
                    return schedule;
                }
            }
        }

        public List<TimeTable> GetTimeTableByDriver(int driverId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetScheduleByDriver";
                    command.Parameters.AddWithValue("@driverId", driverId);

                    var schedule = new List<TimeTable>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var timeTable = new TimeTable()
                            {
                                Id = (int)reader["Id"],
                                RouteNumber = (string)reader["RouteNumber"],
                                DepartureTime = (TimeSpan)reader["DepartureTime"],
                                Duration = (int)reader["Duration"],
                                ArrivalTime = (TimeSpan)reader["ArrivalTime"]
                            };

                            schedule.Add(timeTable);
                        }
                    }
                    return schedule;
                }
            }
        }

        public int Insert(string busNumber, TimeSpan departureTime)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spInsertTimeTable";

                    command.Parameters.AddWithValue("@busNumber", busNumber);
                    command.Parameters.AddWithValue("@departureTime", departureTime);
                    var idParameter = new SqlParameter("@id", SqlDbType.Int);
                    idParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(idParameter);

                    command.ExecuteNonQuery();

                    return (int)idParameter.Value;
                }
            }
        }

        public void Delete(int scheduleId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spDeleteTimeTable";
                    command.Parameters.AddWithValue("@timeTableId", scheduleId);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}