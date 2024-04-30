using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmailApp
{
    public partial class Form4 : Form
    {
        private string myEmail = "";
        public Form4(string myEmail)
        {
            InitializeComponent();
            this.myEmail = myEmail;
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;

            string formattedDate = today.ToString("yyyy-MM-dd");

            DatabaseHelper databaseHelper = new DatabaseHelper();
            databaseHelper.InsertEmail(myEmail, textBox1.Text, null, null, formattedDate, textBox2.Text, richTextBox1.Text);
            this.Hide();
        }

    }
}
