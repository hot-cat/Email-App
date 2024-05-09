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
        //private string connectionString = @"Data Source=LAB108PC01\SQLEXPRESS;Initial Catalog=EmailApp;Integrated Security=True";
        private string connectionString = @"Data Source=M2MDEVYH-2T15\SQLEXPRESS;Initial Catalog=EmailApp;Integrated Security=True";

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

        public int Login(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT UserID, DateCreated, HashedPassword FROM Users WHERE Email = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int userId = reader.GetInt32(0);
                        string dateCreated = reader.GetString(1);
                        string storedHashedPassword = reader.GetString(2);
                        string computedHash = HashPasswordWithDate(password, dateCreated);

                        if (storedHashedPassword.Equals(computedHash))
                        {
                            return userId;
                        }
                    }
                }
            }
            return -1;
        }


        public int Register(string email, string password)
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
                    string insertQuery = "INSERT INTO Users (Email, HashedPassword, DateCreated) OUTPUT INSERTED.UserID VALUES (@Email, @HashedPassword, @DateCreated)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@Email", email);
                    insertCommand.Parameters.AddWithValue("@HashedPassword", hashedPassword);
                    insertCommand.Parameters.AddWithValue("@DateCreated", todayDate);

                    int newUserId = (int)insertCommand.ExecuteScalar();

                    // Insert a basic entry into UserInformation table
                    string insertInfoQuery = "INSERT INTO UserInformation (UserID, Email) VALUES (@UserID, @Email)";
                    SqlCommand insertInfoCommand = new SqlCommand(insertInfoQuery, connection);
                    insertInfoCommand.Parameters.AddWithValue("@UserID", newUserId);
                    insertInfoCommand.Parameters.AddWithValue("@Email", email);
                    insertInfoCommand.ExecuteNonQuery();

                    return newUserId;
                }
            }
            return -1;
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

        public void InsertEmail(string senderEmail, string receiverEmail, string bccEmails, string ccEmails, string dateSent, string subject, string emailBody)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Define the SQL query to insert a new email
                string query = @"
            INSERT INTO Emails (SenderEmail, ReceiverEmail, BccEmails, CcEmails, DateSent, Subject, EmailBody) 
            VALUES (@SenderEmail, @ReceiverEmail, @BccEmails, @CcEmails, @DateSent, @Subject, @EmailBody)";

                // Create a SqlCommand object
                SqlCommand command = new SqlCommand(query, connection);

                // Add parameters to avoid SQL injection
                command.Parameters.AddWithValue("@SenderEmail", senderEmail);
                command.Parameters.AddWithValue("@ReceiverEmail", receiverEmail);
                command.Parameters.AddWithValue("@BccEmails", bccEmails ?? string.Empty); // Handle null with an empty string
                command.Parameters.AddWithValue("@CcEmails", ccEmails ?? string.Empty);   // Handle null with an empty string
                command.Parameters.AddWithValue("@DateSent", dateSent);
                command.Parameters.AddWithValue("@Subject", subject);
                command.Parameters.AddWithValue("@EmailBody", emailBody);

                // Open the connection, execute the command and close the connection
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public UserInfo GetUserInfo(string emailaddress)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Email, FirstName, LastName, Sex, Profession, ProfilePicture FROM UserInformation WHERE Email = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", emailaddress);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserInfo
                        {
                            UserID = reader.GetInt32(0),
                            Email = reader.GetString(1),
                            FirstName = reader.IsDBNull(2) ? null : reader.GetString(2),
                            LastName = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Sex = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Profession = reader.IsDBNull(5) ? null : reader.GetString(5),
                            ProfilePicture = reader.IsDBNull(6) ? null : reader.GetString(6)
                        };
                    }
                }
            }
            return null;
        }

        public void UpdateUserInfo(UserInfo userInfo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
        UPDATE UserInformation SET 
            Email = @Email, 
            FirstName = @FirstName, 
            LastName = @LastName, 
            Sex = @Sex, 
            Profession = @Profession, 
            ProfilePicture = @ProfilePicture 
        WHERE UserID = @UserID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userInfo.UserID);
                command.Parameters.AddWithValue("@Email", userInfo.Email);
                command.Parameters.AddWithValue("@FirstName", userInfo.FirstName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@LastName", userInfo.LastName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Sex", userInfo.Sex ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Profession", userInfo.Profession ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ProfilePicture", userInfo.ProfilePicture ?? (object)DBNull.Value);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public string GetProfilePictureByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ProfilePicture FROM UserInformation WHERE Email = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return result.ToString();
                }
                return null; // Return null if no profile picture is found
            }
        }





    }
}
