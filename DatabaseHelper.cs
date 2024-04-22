using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace EmailApp
{
    internal class DatabaseHelper
    {
        private string connectionString = @"Data Source=LAB108PC01\SQLEXPRESS;Initial Catalog=EmailApp;Integrated Security=True";

        public DataTable GetAllUsers()
        {
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Users", connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        public void InsertUser(string email, string hashedPassword, string dateCreated)
        {
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (Email, HashedPassword, DateCreated) VALUES (@Email, @HashedPassword, @DateCreated)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@HashedPassword", hashedPassword);
                command.Parameters.AddWithValue("@DateCreated", dateCreated);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateUser(int userId, string email)
        {
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                string query = "UPDATE Users SET Email = @Email WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);
                command.Parameters.AddWithValue("@Email", email);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteUser(int userId)
        {
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                string query = "DELETE FROM Users WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public DataTable GetUserByEmail(string email)
        {
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Users WHERE Email = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        public bool Login(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT DateCreated, HashedPassword FROM Users WHERE Email = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string dateCreated = reader.GetString(0);
                        string storedHashedPassword = reader.GetString(1);
                        string computedHash = HashPasswordWithDate(password, dateCreated);

                        return storedHashedPassword.Equals(computedHash);
                    }
                }
            }
            return false;
        }

        public bool Register(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
                string hashedPassword = HashPasswordWithDate(password, todayDate);

                string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection);
                checkUserCommand.Parameters.AddWithValue("@Email", email);

                int userExists = Convert.ToInt32(checkUserCommand.ExecuteScalar());
                if (userExists == 0)
                {
                    string insertQuery = "INSERT INTO Users (Email, HashedPassword, DateCreated) VALUES (@Email, @HashedPassword, @DateCreated)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@Email", email);
                    insertCommand.Parameters.AddWithValue("@HashedPassword", hashedPassword);
                    insertCommand.Parameters.AddWithValue("@DateCreated", todayDate);

                    insertCommand.ExecuteNonQuery();
                    return true;
                }
            }
            return false;
        }

        private string HashPasswordWithDate(string password, string date)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string combined = password + date;
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(combined));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
