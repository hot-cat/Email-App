using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmailApp
{
    public partial class Form2 : Form
    {

        //private string connectionString = @"Data Source=LAB108PC01\SQLEXPRESS;Initial Catalog=EmailApp;Integrated Security=True";
        private string connectionString = @"Data Source=M2MDEVYH-2T15\SQLEXPRESS;Initial Catalog=EmailApp;Integrated Security=True";
        private string emailUser = "";

        public Form2(string emailUser)
        {
            InitializeComponent();

            this.emailUser = emailUser;
            LoadEmails(emailUser);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }


        private void buttonLoadEmails_Click(object sender, EventArgs e)
        {
            LoadEmails(emailUser);
        }

        private void LoadEmails(string receiverEmail)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT SenderEmail, DateSent, Subject, EmailBody  FROM Emails WHERE ReceiverEmail = @ReceiverEmail ORDER BY DateSent DESC";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ReceiverEmail", receiverEmail);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.Columns["SenderEmail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["Subject"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the double-clicked row is valid (not the header row)
            if (e.RowIndex >= 0)
            {
                // Get the data from the double-clicked row
                DataGridView dgv = sender as DataGridView;
                if (dgv != null)
                {
                    // Assuming you have a column "Email" in your DataGridView
                    string emailSubject = dgv.Rows[e.RowIndex].Cells["Subject"].Value.ToString();
                    string emailBody = dgv.Rows[e.RowIndex].Cells["EmailBody"].Value.ToString();
                    string senderEmail = dgv.Rows[e.RowIndex].Cells["SenderEmail"].Value.ToString();
                    string date = dgv.Rows[e.RowIndex].Cells["DateSent"].Value.ToString();

                    Form3 form = new Form3(senderEmail, date, emailSubject, emailBody);

                    form.Show();


                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form = new Form4(emailUser);
            form.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadEmails(emailUser);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form5 form = new Form5(1);
            form.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Start building the SQL query
                StringBuilder queryBuilder = new StringBuilder("SELECT SenderEmail, DateSent, Subject, EmailBody FROM Emails WHERE ReceiverEmail = @ReceiverEmail");

                // Add conditions based on non-empty text boxes
                List<string> conditions = new List<string>();
                SqlCommand command = new SqlCommand();
                command.Parameters.AddWithValue("@ReceiverEmail", emailUser);

                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    conditions.Add("SenderEmail = @SenderEmail");
                    command.Parameters.AddWithValue("@SenderEmail", textBox1.Text);
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

                // Combine all conditions
                if (conditions.Count > 0)
                {
                    queryBuilder.Append(" AND " + string.Join(" AND ", conditions));
                }

                queryBuilder.Append(" ORDER BY DateSent DESC");

                // Set the query and connection for the command
                command.CommandText = queryBuilder.ToString();
                command.Connection = connection;

                // Execute the command
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                // Display the results in DataGridView
                dataGridView1.DataSource = table;
                dataGridView1.Columns["SenderEmail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["Subject"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form6 form = new Form6(emailUser);
            form.Show();

        }

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

    }
}
