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
            Boolean success = true;
            try
            {
                databaseHelper.InsertEmail(myEmail, textBox1.Text, null, null, formattedDate, textBox2.Text, richTextBox1.Text);
            } catch 
            {
                
                    MessageBox.Show("The receipient email is invalid, please check for typos.", "Wrong receipient email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    success = false;
            }
                if (success)
                {

                    this.Hide();
                }
        }

    }
}
