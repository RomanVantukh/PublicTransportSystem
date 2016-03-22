using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using PTS.Entities;
using System.Security.Cryptography;

namespace PTS.Repositories
{
    public interface ICustomerRepository
    {
        Customer LogIn(string login, string password);
    }

    public class SqlCustomerRepository : SqlBaseRepository, ICustomerRepository
    {
        public SqlCustomerRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public Customer LogIn(string login, string password)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    string hashPassword = Hashing(password);

                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spLogIn";

                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", hashPassword);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return null;
                        }

                        return new Customer()
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            Surname = (string)reader["Surname"],
                            Mode = (Mode)reader["Mode"]
                        };
                    }
                }
            }
        }

        private string Hashing(string password)
        {
            StringBuilder result = new StringBuilder();
            SHA256 hash = SHA256.Create();

            byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int j = 0; j < bytes.Length; j++)
            {
                result.Append(bytes[j].ToString("x2"));
            }

            return result.ToString();
        }
    }
}
