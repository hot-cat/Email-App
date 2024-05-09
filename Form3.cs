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
        private string label1text = "";

        private void DisplayInformation()
        {
            // Assigning values to labels and rich text box
            label1.Text = $"{label1text} {sender}";
            label2.Text = $"Date: {date}";
            label3.Text = emailSubject;
            richTextBox1.Text = emailBody;
            richTextBox1.ReadOnly = true;
        }
        public Form3(string sender, string date, string emailSubject, string emailBody, string label)
        {
            this.sender = sender;
            this.date = date;
            this.emailBody = emailBody;
            this.emailSubject = emailSubject;
            this.label1text = label;
            InitializeComponent();
            DisplayInformation();
            LoadProfilePicture();
        }

        private void LoadProfilePicture()
        {
            DatabaseHelper db = new DatabaseHelper();
            string profilePictureBase64 = db.GetProfilePictureByEmail(sender);
            if (!string.IsNullOrEmpty(profilePictureBase64))
            {
                byte[] imageBytes = Convert.FromBase64String(profilePictureBase64);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    pictureBox1.Image = Image.FromStream(ms);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            else
            {
                // Optionally set a default picture or leave it blank
                //pictureBox1.Image = Properties.Resources.defaultProfilePic; // Assuming you have a default picture
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
