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

        public Form2()
        {
            InitializeComponent();

            LoadEmails("yasen@gmail.com");
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }


        private void buttonLoadEmails_Click(object sender, EventArgs e)
        {
            LoadEmails("yasen@gmail.com");
        }

        private void LoadEmails(string receiverEmail)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT SenderEmail, DateSent, Subject FROM Emails WHERE ReceiverEmail = @ReceiverEmail ORDER BY DateSent DESC";
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
                    string email = dgv.Rows[e.RowIndex].Cells["Subject"].Value.ToString();
                    MessageBox.Show("Double-clicked row email: " + email);
                }
            }
        }

    }
}
