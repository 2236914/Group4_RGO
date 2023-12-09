using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace RGO_LIPA
{
    public partial class ADMIN_LOGIN : Form
    {
        public ADMIN_LOGIN()
        {
            InitializeComponent();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            // Replace "yourAdminPassword" with the actual admin password
            string adminPassword = "1234";

            // Get the password entered by the user
            string enteredPassword = guna2TextBox1.Text;

            // Check if the entered password is correct
            if (enteredPassword == adminPassword)
            {
                // If the password is correct, open Form4
                ADMIN_POTAL form4 = new ADMIN_POTAL();
                form4.ShowDialog();

                // Hide Form3
                this.Hide();

                // Close Form3 after Form4 is closed
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect password. Please try again.");
            }
        }




        private void btnStudent_Click(object sender, EventArgs e)
        {
            // Check if Form1 is already open
            WELCOME form1 = Application.OpenForms.OfType<WELCOME>().FirstOrDefault()!;
            // Close Form1 if it is open (assuming form1 is never null)
            form1.Close();


            // Show Form3
            ADMIN_LOGIN form3 = new ADMIN_LOGIN();
    form3.Show();
        }
    }
}
