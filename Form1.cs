using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace EmailApp
{
    public partial class Form1 : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        public Form1()
        {
            InitializeComponent();
            // Set the initial state of the controls when the form loads
            radioButton1.Checked = true; 
            textBox3.Visible = false;
            button1.Text = "Log In"; 
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // Check the state of radioButton1 to toggle visibility and button text
            if (radioButton1.Checked)
            {
                textBox3.Visible = false; // Hide the repeat password box for login
                label4.Visible = false;
                button1.Text = "Log In";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            // Check the state of radioButton2 to toggle visibility and button text
            if (radioButton2.Checked)
            {
                textBox3.Visible = true; // Show the repeat password box for registration
                label4.Visible = true;
                button1.Text = "Register";
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text; // Email input
            string password = textBox2.Text; // Password input

            if (radioButton1.Checked) // Login
            {
                int result = dbHelper.Login(email, password);
                if (result != -1)
                {
                    //MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if(email == "admin")
                    {
                        Form7 form7 = new Form7();
                        form7.Show();
                        
                    } else {
                        Form2 form = new Form2(email);

                        form.Show();
                    }
                    

                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Login failed. Please check your username and password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (radioButton2.Checked) // Register
            {
                string repeatPassword = textBox3.Text; // Repeat password input
                if (password == repeatPassword)
                {
                    int result = dbHelper.Register(email, password);
                    if (result != -1)
                    {
                        MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Transition to login or directly into the application
                        Form2 form = new Form2(email);

                        form.Show();

                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Registration failed. Email may already be in use.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Passwords do not match.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
