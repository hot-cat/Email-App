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
    public partial class Form3 : Form
    {

        // Declaring the variables
        private string sender = "John Doe";
        private string date = DateTime.Now.ToString("yyyy-MM-dd");
        private string emailSubject = "johndoe@example.com";
        private string emailBody = "";

        private void DisplayInformation()
        {
            // Assigning values to labels and rich text box
            label1.Text = $"Sender: {sender}";
            label2.Text = $"Date: {date}";
            label3.Text = emailSubject;
            richTextBox1.Text = emailBody;
            richTextBox1.ReadOnly = true;
        }
        public Form3(string sender, string date, string emailSubject, string emailBody)
        {
            this.sender = sender;
            this.date = date;
            this.emailBody = emailBody;
            this.emailSubject = emailSubject;
            InitializeComponent();
            DisplayInformation();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
