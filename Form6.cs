using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace EmailApp
{
    public partial class Form6 : Form
    {
        // Connection string to the SQL database
        private string connectionString = @"Data Source=M2MDEVYH-2T15\SQLEXPRESS;Initial Catalog=EmailApp;Integrated Security=True";
        private string emailUser = "";

        public Form6(string emailUser)
        {
            InitializeComponent();

            this.emailUser = emailUser;
            LoadSentEmails(emailUser);
        }

        // Loads all sent emails for the given user
        private void LoadSentEmails(string senderEmail)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ReceiverEmail, DateSent, Subject, EmailBody FROM Emails WHERE SenderEmail = @SenderEmail ORDER BY DateSent DESC";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SenderEmail", senderEmail);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.Columns["ReceiverEmail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["Subject"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        // Handles the search button click
        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                StringBuilder queryBuilder = new StringBuilder("SELECT ReceiverEmail, DateSent, Subject, EmailBody FROM Emails WHERE SenderEmail = @SenderEmail");
                List<string> conditions = new List<string>();
                SqlCommand command = new SqlCommand();
                command.Parameters.AddWithValue("@SenderEmail", emailUser);

                // Add search criteria based on non-empty textboxes
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    conditions.Add("ReceiverEmail = @ReceiverEmail");
                    command.Parameters.AddWithValue("@ReceiverEmail", textBox1.Text);
                }
                if (!string.IsNullOrEmpty(textBox2.Text))
                {
                    conditions.Add("DateSent = @DateSent");
                    command.Parameters.AddWithValue("@DateSent", textBox2.Text);
                }
                if (!string.IsNullOrEmpty(textBox3.Text))
                {
                    conditions.Add("Subject LIKE @Subject");
                    command.Parameters.AddWithValue("@Subject", "%" + textBox3.Text + "%");
                }

                // Append all conditions to the query
                if (conditions.Count > 0)
                {
                    queryBuilder.Append(" AND " + string.Join(" AND ", conditions));
                }
                queryBuilder.Append(" ORDER BY DateSent DESC");

                command.CommandText = queryBuilder.ToString();
                command.Connection = connection;

                // Execute and update DataGridView
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.Columns["ReceiverEmail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["Subject"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        // Refreshes the sent emails
        private void button2_Click(object sender, EventArgs e)
        {
            LoadSentEmails(emailUser);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView dgv = sender as DataGridView;
                if (dgv != null)
                {
                    string emailSubject = dgv.Rows[e.RowIndex].Cells["Subject"].Value.ToString();
                    string emailBody = dgv.Rows[e.RowIndex].Cells["EmailBody"].Value.ToString();
                    string receiverEmail = dgv.Rows[e.RowIndex].Cells["ReceiverEmail"].Value.ToString();
                    string date = dgv.Rows[e.RowIndex].Cells["DateSent"].Value.ToString();

                    Form3 form = new Form3(receiverEmail, date, emailSubject, emailBody);
                    form.Show();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
