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

        private string connectionString = @"Data Source=LAB108PC01\SQLEXPRESS;Initial Catalog=EmailApp;Integrated Security=True";
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
            Form4 form = new Form4();
            form.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadEmails(emailUser);
        }
    }
}
