using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGO_LIPA
{
    public partial class STUDENT_PORTAL : Form
    {
        public STUDENT_PORTAL()
        {
            InitializeComponent();

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            using (STUDENT_ANNOUNCEMENT STUDENT_ANNOUNCEMENTForm = new STUDENT_ANNOUNCEMENT())
            {
                this.Hide();
                STUDENT_ANNOUNCEMENTForm.ShowDialog();
                this.Close();
            }
        }

        private void btnClick_Click(object sender, EventArgs e)
        {
            try
            {
                // Specify the URL
                string url = "https://docs.google.com/forms/d/e/1FAIpQLSdZKc2C0wqtqaxwragWj6zWYbrKYkRtzWGkk2Mmg8FqY0uf-g/formrestricted";

                // Use ProcessStartInfo to specify that the target is a URL
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };

                // Start the process
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            using (WELCOME WELCOMEForm = new WELCOME())
            {
                this.Hide();
                WELCOMEForm.ShowDialog();
                this.Close();
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Specify the URL
                string url = "https://www.facebook.com/RGOLipaOfficial/";

                // Use ProcessStartInfo to specify that the target is a URL
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };

                // Start the process
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
