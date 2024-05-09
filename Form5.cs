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
    public partial class Form5 : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        private string emailUser; // This should be set appropriately
        private UserInfo userInfo;

        public Form5(string email)
        {
            this.emailUser = email;
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            LoadUserData();
        }

        private void LoadUserData()
        {
            userInfo = dbHelper.GetUserInfo(emailUser);
            if (userInfo != null)
            {
                textBox1.Text = userInfo.Email; // Email
                textBox1.ReadOnly = true; // Make email non-editable
                textBox2.Text = userInfo.FirstName; // First Name
                textBox3.Text = userInfo.LastName; // Last Name
                textBox4.Text = userInfo.Profession; // Profession

                // Set up the ComboBox for sex
                comboBox1.Items.Add("Male");
                comboBox1.Items.Add("Female");
                comboBox1.Items.Add("Non-binary");
                if (!string.IsNullOrEmpty(userInfo.Sex))
                {
                    comboBox1.SelectedItem = userInfo.Sex;
                }

                // Load profile picture from Base64 string
                if (!string.IsNullOrEmpty(userInfo.ProfilePicture))
                {
                    byte[] imageBytes = Convert.FromBase64String(userInfo.ProfilePicture);
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            SaveUserData();
        }

        private void SaveUserData()
        {
            // Update userInfo object with new data from the form
            userInfo.FirstName = textBox2.Text;
            userInfo.LastName = textBox3.Text;
            userInfo.Profession = textBox4.Text;
            userInfo.Sex = comboBox1.SelectedItem?.ToString();

            // Save updated userInfo to the database
            dbHelper.UpdateUserInfo(userInfo);

            MessageBox.Show("User information updated successfully!");
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Load the image into the PictureBox
                pictureBox1.Image = Image.FromFile(openFileDialog.FileName);

                // Convert image to Base64 string
                using (Image image = Image.FromFile(openFileDialog.FileName))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // Convert Image to byte[]
                        image.Save(ms, image.RawFormat);
                        byte[] imageBytes = ms.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        userInfo.ProfilePicture = base64String;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }

}
