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
    public interface IRouteRepository
    {
        List<Route> SelectAll();
        List<Route> SearchAll(string stationName, int? maxPrice, int? MaxDuration);
        List<string> GetAllNumbers();
        int Insert(int customerId, Route route);
        int AddStationToRoute(string routeNumber, string stationName, int orderNumber);
        void DeleteStationToRoute(string stationName, int routeId);
        void Update(int customerId, int routeId, string number, int? duration, int? distance, int? price);
        void Delete(int routeId);
    }

    public class SqlRouteRepository : SqlBaseRepository, IRouteRepository
    {
        public SqlRouteRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<Route> SelectAll()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spSelectAllRoutes";

                    var routes = new List<Route>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var route = new Route()
                            {
                                Id = (int)reader["Id"],
                                Number = (string)reader["Number"],
                                Distance = (int)reader["Distance"],
                                Price = (int)reader["Price"],
                                Duration = (int)reader["Duration"],
                                Customer = (string)reader["Customer"]
                            };

                            routes.Add(route);
                        }
                    }

                    return routes;
                }
            }
        }

        public List<Route> SearchAll(string stationName, int? maxPrice, int? MaxDuration)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spGetRoutesBySearch";

                    SqlParameter name = new SqlParameter();
                    name.ParameterName = "@stationName";
                    name.DbType = DbType.String;
                    name.IsNullable = true;
                    name.Value = stationName;
                    command.Parameters.Add(name);

                    SqlParameter price = new SqlParameter();
                    price.ParameterName = "@maxPrice";
                    price.DbType = DbType.Int16;
                    price.IsNullable = true;
                    price.Value = maxPrice;
                    command.Parameters.Add(price);

                    SqlParameter duration = new SqlParameter();
                    duration.ParameterName = "@maxDuration";
                    duration.DbType = DbType.Int16;
                    duration.IsNullable = true;
                    duration.Value = MaxDuration;
                    command.Parameters.Add(duration);

                    var routes = new List<Route>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var route = new Route()
                            {
                                Id = (int)reader["Id"],
                                Number = (string)reader["Number"],
                                Distance = (int)reader["Distance"],
                                Price = (int)reader["Price"],
                                Duration = (int)reader["Duration"],
                                Customer = (string)reader["Customer"]
                            };

                            routes.Add(route);
                        }
                    }

                    return routes;
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
                    command.CommandText = "spGetRouteNumber";

                    var routes = new List<string>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            routes.Add((string)reader["Number"]);
                        }
                    }

                    return routes;
                }
            }
        }

        public int Insert(int customerId, Route route)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spInsertRoute";

                    command.Parameters.AddWithValue("@number", route.Number);
                    command.Parameters.AddWithValue("@distance", route.Distance);
                    command.Parameters.AddWithValue("@price", route.Price);
                    command.Parameters.AddWithValue("@duration", route.Duration);
                    command.Parameters.AddWithValue("@customerId", customerId);
                    var idParameter = new SqlParameter("@id", SqlDbType.Int);
                    idParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(idParameter);

                    command.ExecuteNonQuery();

                    return (int)idParameter.Value;
                }
            }
        }

        public int AddStationToRoute(string routeNumber, string stationName, int orderNumber)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spInsertRouteStationRelation";

                    command.Parameters.AddWithValue("@routeNumber", routeNumber);
                    command.Parameters.AddWithValue("@stationName", stationName);
                    command.Parameters.AddWithValue("@orderNumber", orderNumber);
                    var idParameter = new SqlParameter("@id", SqlDbType.Int);
                    idParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(idParameter);

                    command.ExecuteNonQuery();

                    return (int)idParameter.Value;
                }
            }
        }

        public void DeleteStationToRoute(string stationName, int routeId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spDeleteRouteStationRelation";
                    command.Parameters.AddWithValue("@name", stationName);
                    command.Parameters.AddWithValue("@routeId", routeId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(int customerId, int routeId, string number, int? duration, int? distance, int? price)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spUpdateRoute";

                    command.Parameters.AddWithValue("@id", routeId);
                    command.Parameters.AddWithValue("@customerId", customerId);

                    SqlParameter numberParameter = new SqlParameter();
                    numberParameter.ParameterName = "@number";
                    numberParameter.DbType = DbType.String;
                    numberParameter.IsNullable = true;
                    numberParameter.Value = number;
                    command.Parameters.Add(numberParameter);

                    SqlParameter distanceParameter = new SqlParameter();
                    distanceParameter.ParameterName = "@distance";
                    distanceParameter.DbType = DbType.Int16;
                    distanceParameter.IsNullable = true;
                    distanceParameter.Value = distance;
                    command.Parameters.Add(distanceParameter);

                    SqlParameter priceParameter = new SqlParameter();
                    priceParameter.ParameterName = "@price";
                    priceParameter.DbType = DbType.Int16;
                    priceParameter.IsNullable = true;
                    priceParameter.Value = price;
                    command.Parameters.Add(priceParameter);

                    SqlParameter durationParameter = new SqlParameter();
                    durationParameter.ParameterName = "@duration";
                    durationParameter.DbType = DbType.Int16;
                    durationParameter.IsNullable = true;
                    durationParameter.Value = duration;
                    command.Parameters.Add(durationParameter);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int routeid)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spDeleteRoute";
                    command.Parameters.AddWithValue("@id", routeid);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}