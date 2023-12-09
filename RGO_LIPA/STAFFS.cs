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
    public partial class STAFFS : Form
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=rgo_lipa;Username=postgres;Password=postgres";
        public STAFFS()
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

        private void STAFFS_Load(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Assuming you want to display data from the Staff table
                    string query = "SELECT * FROM Staff ORDER BY STAFF_ID ASC";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the DataGridView
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            using (ADMIN_POTAL ADMIN_PORTALForm = new ADMIN_POTAL())
            {
                this.Hide();
                ADMIN_PORTALForm.ShowDialog();
                this.Close();
            }
        }

        private void btnInsert1_Click(object sender, EventArgs e)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);

            try
            {
                connection.Open();

                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    try
                    {
                        string insertStaffQuery = "INSERT INTO Staff (STAFF_ID, STAFF_NAME, STAFF_POSITION, EMAIL, PHONE) " +
                              "VALUES (@staffId, @staffName, @staffPosition, @staffEmail, @staffPhone)";

                        using (NpgsqlCommand insertStaffCommand = new NpgsqlCommand(insertStaffQuery, connection))
                        {
                            string staffId = row.Cells["STAFF_ID"].Value?.ToString() ?? string.Empty;
                            string staffName = row.Cells["STAFF_NAME"].Value?.ToString() ?? string.Empty;
                            string staffPosition = row.Cells["STAFF_POSITION"].Value?.ToString() ?? string.Empty;

                            // Use the correct column name from your DataGridView
                            string staffEmail = row.Cells["Email"].Value?.ToString() ?? string.Empty;

                            string staffPhone = row.Cells["PHONE"].Value?.ToString() ?? string.Empty;

                            insertStaffCommand.Parameters.AddWithValue("@staffId", Convert.ToInt32(staffId));
                            insertStaffCommand.Parameters.AddWithValue("@staffName", staffName);
                            insertStaffCommand.Parameters.AddWithValue("@staffPosition", staffPosition);
                            insertStaffCommand.Parameters.AddWithValue("@staffEmail", staffEmail);
                            insertStaffCommand.Parameters.AddWithValue("@staffPhone", staffPhone);

                            int staffRowsAffected = insertStaffCommand.ExecuteNonQuery();

                            if (staffRowsAffected > 0)
                            {
                                MessageBox.Show("Insert into Staff successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Insert into Staff failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        string updateQuery = "UPDATE staff SET " +
                                              "staff_name = @staffName, " +
                                              "staff_position = @staffPosition, " +
                                              "email = @email, " +
                                              "phone = @phone " +
                                              "WHERE staff_id = @staffId";

                        using (NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@staffName", GetValueFromCell(row.Cells["staff_name"]));
                            updateCommand.Parameters.AddWithValue("@staffPosition", GetValueFromCell(row.Cells["staff_position"]));
                            updateCommand.Parameters.AddWithValue("@email", GetValueFromCell(row.Cells["email"]));
                            updateCommand.Parameters.AddWithValue("@phone", GetValueFromCell(row.Cells["phone"]));

                            // Ensure staff_id is treated as an integer
                            if (int.TryParse(GetValueFromCell(row.Cells["staff_id"]), out int staffId))
                            {
                                updateCommand.Parameters.AddWithValue("@staffId", staffId);

                                int rowsAffected = updateCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Update successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Update failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid staff_id value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetValueFromCell(DataGridViewCell cell)
        {
            return cell.Value?.ToString() ?? string.Empty;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        int staffId = Convert.ToInt32(GetValueFromCell(row.Cells["staff_id"]));

                        // Delete from the staff table
                        string deleteStaffQuery = "DELETE FROM staff WHERE staff_id = @staffId";

                        using (NpgsqlCommand deleteStaffCommand = new NpgsqlCommand(deleteStaffQuery, connection))
                        {
                            deleteStaffCommand.Parameters.AddWithValue("@staffId", staffId);

                            int rowsAffected = deleteStaffCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Delete successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Delete failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }

                    // Rebind the DataGridView to refresh the data
                    LoadStaffData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStaffData()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM staff";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the DataGridView
                        dataGridView1.DataSource = dataTable;

                        // Adjust column width based on content
                        dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
}
