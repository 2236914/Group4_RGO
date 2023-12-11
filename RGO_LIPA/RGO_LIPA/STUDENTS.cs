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
using static TheArtOfDevHtmlRenderer.Adapters.RGraphicsPath;

namespace RGO_LIPA
{
    public partial class STUDENTS : Form
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=rgo_lipa;Username=postgres;Password=postgres";

        public STUDENTS()
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

        private void Form5_Load(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Assuming you want to display data from the students table
                    string query = "SELECT * FROM students ORDER BY SR_CODE ASC";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the DataGridView
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
            // Auto-resize columns based on content
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void btnInsert1_Click(object sender, EventArgs e)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);

            try
            {
                connection.Open();

                // Inside the foreach loop
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow && row.Selected)
                    {
                        // Fetching the values of foreign key fields                       
                        string existingSrCode = row.Cells["SR_CODE"].Value?.ToString() ?? "DefaultValue";
                        string existingDateOfOrder = row.Cells["DATE_OF_ORDER"].Value?.ToString() ?? string.Empty;

                        // Insert into Students table
                        string insertStudentsQuery = "INSERT INTO Students (SR_CODE, COLLEGE_DEPARTMENT, STUDENT_NAME, EMAIL, PHONE, DATE_OF_ORDER) " +
                                                     "VALUES (@srCode, @department, @studentName, @email, @phone, @dateOfOrder)";

                        using (NpgsqlCommand insertStudentsCommand = new NpgsqlCommand(insertStudentsQuery, connection))
                        {
                            // Reuse existingSrCode and existingDateOfOrder
                            insertStudentsCommand.Parameters.AddWithValue("@srCode", Convert.ToInt32(existingSrCode));
                            insertStudentsCommand.Parameters.AddWithValue("@department", row.Cells["COLLEGE_DEPARTMENT"].Value?.ToString() ?? string.Empty);
                            insertStudentsCommand.Parameters.AddWithValue("@studentName", row.Cells["STUDENT_NAME"].Value?.ToString() ?? string.Empty);
                            insertStudentsCommand.Parameters.AddWithValue("@email", row.Cells["EMAIL"].Value?.ToString() ?? string.Empty);
                            insertStudentsCommand.Parameters.AddWithValue("@phone", row.Cells["PHONE"].Value?.ToString() ?? string.Empty);
                            insertStudentsCommand.Parameters.AddWithValue("@dateOfOrder", DateTime.Parse(existingDateOfOrder));

                            int rowsAffected = insertStudentsCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Insert into Students successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Insert into Students failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            using (ADMIN_POTAL ADMIN_PORTALForm = new ADMIN_POTAL())
            {
                this.Hide();
                ADMIN_PORTALForm.ShowDialog();
                this.Close();
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
                        string updateQuery = "UPDATE students SET college_department = @collegeDepartment, " +
                                              "student_name = @studentName, email = @email, phone = @phone " +
                                              "WHERE sr_code = @srCode AND date_of_order = @dateOfOrder";

                        using (NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, connection))
                        {
                            // Add parameters for the updateCommand
                            updateCommand.Parameters.AddWithValue("@collegeDepartment", GetValueFromCell(row.Cells["COLLEGE_DEPARTMENT"]));
                            updateCommand.Parameters.AddWithValue("@studentName", GetValueFromCell(row.Cells["STUDENT_NAME"]));
                            updateCommand.Parameters.AddWithValue("@email", GetValueFromCell(row.Cells["EMAIL"]));
                            updateCommand.Parameters.AddWithValue("@phone", GetValueFromCell(row.Cells["PHONE"]));
                            updateCommand.Parameters.AddWithValue("@srCode", Convert.ToInt32(GetValueFromCell(row.Cells["SR_CODE"])));
                            updateCommand.Parameters.AddWithValue("@dateOfOrder", GetDateTimeValueFromCell(row.Cells["DATE_OF_ORDER"]));

                            int rowsAffected = updateCommand.ExecuteNonQuery();

                            // Notify the user about the successful update
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Update successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Update failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private DateTime GetDateTimeValueFromCell(DataGridViewCell cell)
        {
            return DateTime.Parse(GetValueFromCell(cell));
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
                        // Delete associated records in the 'items' table
                        DeleteAssociatedItems(Convert.ToInt32(row.Cells["sr_code"].Value),
                                              Convert.ToDateTime(row.Cells["date_of_order"].Value), connection);

                        // Now delete the student record
                        string deleteQuery = "DELETE FROM students WHERE sr_code = @srCode AND date_of_order = @dateOfOrder";

                        using (NpgsqlCommand deleteCommand = new NpgsqlCommand(deleteQuery, connection))
                        {
                            deleteCommand.Parameters.AddWithValue("@srCode", Convert.ToInt32(row.Cells["sr_code"].Value));
                            deleteCommand.Parameters.AddWithValue("@dateOfOrder", Convert.ToDateTime(row.Cells["date_of_order"].Value));

                            int rowsAffected = deleteCommand.ExecuteNonQuery();

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

                    // Refresh the data in the DataGridView after deletion
                    RefreshDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshDataGridView()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM students";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the DataGridView
                        dataGridView1.DataSource = dataTable;

                        // Adjust column width based on content
                        AutoResizeColumns(dataGridView1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while refreshing data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteAssociatedItems(int srCode, DateTime dateOfOrder, NpgsqlConnection connection)
        {
            string deleteItemsQuery = "DELETE FROM items WHERE sr_code = @srCode AND date_of_order = @dateOfOrder";

            using (NpgsqlCommand deleteItemsCommand = new NpgsqlCommand(deleteItemsQuery, connection))
            {
                deleteItemsCommand.Parameters.AddWithValue("@srCode", srCode);
                deleteItemsCommand.Parameters.AddWithValue("@dateOfOrder", dateOfOrder);

                deleteItemsCommand.ExecuteNonQuery();
            }
        }
    }
}
