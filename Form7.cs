using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace EmailApp
{
    public partial class Form7 : Form
    {
        private string connectionString = @"Data Source=M2MDEVYH-2T15\SQLEXPRESS;Initial Catalog=EmailApp;Integrated Security=True";

        public Form7()
        {
            InitializeComponent();
            LoadAllEmails();
            LoadUserInformation();
        }

        // Load all emails
        private void LoadAllEmails()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT SenderEmail, ReceiverEmail, DateSent, Subject, EmailBody FROM Emails ORDER BY DateSent DESC";
                SqlCommand command = new SqlCommand(query, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.Columns["SenderEmail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["ReceiverEmail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["Subject"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        // Load user information
        private void LoadUserInformation()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Email, FirstName, LastName, Sex, Profession FROM UserInformation ORDER BY UserID";
                SqlCommand command = new SqlCommand(query, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView2.DataSource = table;
                dataGridView2.Columns["Email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView2.Columns["FirstName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView2.Columns["LastName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        // Search in emails based on textboxes 1, 2, 3, and 4
        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                StringBuilder queryBuilder = new StringBuilder("SELECT SenderEmail, ReceiverEmail, DateSent, Subject, EmailBody FROM Emails WHERE 1=1");
                List<string> conditions = new List<string>();
                SqlCommand command = new SqlCommand();

                // Add conditions based on the non-empty textboxes
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    conditions.Add("SenderEmail = @SenderEmail");
                    command.Parameters.AddWithValue("@SenderEmail", textBox1.Text);
                }
                if (!string.IsNullOrEmpty(textBox2.Text))
                {
                    conditions.Add("ReceiverEmail = @ReceiverEmail");
                    command.Parameters.AddWithValue("@ReceiverEmail", textBox2.Text);
                }
                if (!string.IsNullOrEmpty(textBox3.Text))
                {
                    conditions.Add("DateSent = @DateSent");
                    command.Parameters.AddWithValue("@DateSent", textBox3.Text);
                }
                if (!string.IsNullOrEmpty(textBox4.Text))
                {
                    conditions.Add("Subject LIKE @Subject");
                    command.Parameters.AddWithValue("@Subject", "%" + textBox4.Text + "%");
                }

                if (conditions.Count > 0)
                {
                    queryBuilder.Append(" AND " + string.Join(" AND ", conditions));
                }
                queryBuilder.Append(" ORDER BY DateSent DESC");

                command.CommandText = queryBuilder.ToString();
                command.Connection = connection;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.Columns["SenderEmail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["ReceiverEmail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["Subject"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        // Search in user information based on textboxes 5, 6, 7, and 8
        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                StringBuilder queryBuilder = new StringBuilder("SELECT UserID, Email, FirstName, LastName, Sex, Profession FROM UserInformation WHERE 1=1");
                List<string> conditions = new List<string>();
                SqlCommand command = new SqlCommand();

                if (!string.IsNullOrEmpty(textBox5.Text))
                {
                    conditions.Add("Email LIKE @Email");
                    command.Parameters.AddWithValue("@Email", "%" + textBox5.Text + "%");
                }
                if (!string.IsNullOrEmpty(textBox6.Text))
                {
                    conditions.Add("FirstName LIKE @FirstName");
                    command.Parameters.AddWithValue("@FirstName", "%" + textBox6.Text + "%");
                }
                if (!string.IsNullOrEmpty(textBox7.Text))
                {
                    conditions.Add("LastName LIKE @LastName");
                    command.Parameters.AddWithValue("@LastName", "%" + textBox7.Text + "%");
                }
                if (!string.IsNullOrEmpty(textBox8.Text))
                {
                    conditions.Add("Profession LIKE @Profession");
                    command.Parameters.AddWithValue("@Profession", "%" + textBox8.Text + "%");
                }

                if (conditions.Count > 0)
                {
                    queryBuilder.Append(" AND " + string.Join(" AND ", conditions));
                }
                queryBuilder.Append(" ORDER BY UserID");

                command.CommandText = queryBuilder.ToString();
                command.Connection = connection;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView2.DataSource = table;
                dataGridView2.Columns["Email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView2.Columns["FirstName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView2.Columns["LastName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        // Refresh both DataGridViews
        private void button2_Click(object sender, EventArgs e)
        {
            LoadAllEmails();
            LoadUserInformation();
        }
    }
}
