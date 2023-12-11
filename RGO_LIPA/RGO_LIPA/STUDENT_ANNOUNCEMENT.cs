using Npgsql;
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
    public partial class STUDENT_ANNOUNCEMENT : Form
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=rgo_lipa;Username=postgres;Password=postgres";

        public STUDENT_ANNOUNCEMENT()
        {
            InitializeComponent();
            // Customize the appearance of the DataGridView
            dataGridView1.Font = new Font("Segoe UI", 11F, FontStyle.Regular); // Set font, size, and style
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular); // Set font for data cells
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold); // Set font for column headers
            dataGridView1.EnableHeadersVisualStyles = false; // Disable the default styles for column headers

            // Increase row height to make cells longer
            dataGridView1.RowTemplate.Height = 40; // Adjust the height based on your preference

            // Optionally, set other properties for better appearance
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Fill the entire width of the control
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(128, 0, 0); // Set column header background color to a dark but light maroon
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // Set column header text color
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215); // Set selected cell background color
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White; // Set selected cell text color
        }

        private void STUDENT_ANNOUNCEMENT_Load(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Construct the SQL query to retrieve the desired data
                    string query = "SELECT " +
                                   "students.sr_code, students.student_name, " +
                                   "items.order_items_id, items.date_of_order, items.uniform_pins, items.id_lace, " +
                                   "typeofuniform.white_fabric, typeofuniform.checkered, typeofuniform.pants, typeofuniform.pe " +
                                   "FROM " +
                                   "students " +
                                   "JOIN " +
                                   "items ON students.sr_code = items.sr_code " +
                                   "JOIN " +
                                   "typeofuniform ON items.order_items_id = typeofuniform.order_uniform_id";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the DataGridView or display in another way
                        // For example, if you have a DataGridView named dataGridView1:
                        dataGridView1.DataSource = dataTable;

                        // Adjust column width based on content
                        AutoResizeColumns(dataGridView1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AutoResizeColumns(DataGridView dataGridView)
        {
            dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            using (STUDENT_PORTAL STUDENT_PORTALForm = new STUDENT_PORTAL())
            {
                this.Hide();
                STUDENT_PORTALForm.ShowDialog();
                this.Close();
            }
        }
    }
}

