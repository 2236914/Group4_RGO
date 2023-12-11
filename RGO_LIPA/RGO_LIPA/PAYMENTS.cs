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
    public partial class PAYMENTS : Form
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=rgo_lipa;Username=postgres;Password=postgres;Include Error Detail=true";

        public PAYMENTS()
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

        private void PAYMENTS_Load(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Assuming you want to display data from the Payment table
                    string query = "SELECT * FROM Payment ORDER BY SR_CODE ASC";

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
                        string paymentId = row.Cells["PAYMENT_ID"].Value?.ToString() ?? string.Empty;
                        string srCode = row.Cells["SR_CODE"].Value?.ToString() ?? string.Empty;
                        string dateOfOrder = row.Cells["DATE_OF_ORDER"].Value?.ToString() ?? string.Empty;
                        string amount = row.Cells["AMOUNT"].Value?.ToString() ?? string.Empty;
                        string staffId = row.Cells["STAFF_ID"].Value?.ToString() ?? string.Empty;

                        // Validate the IDs before attempting to convert
                        if (!string.IsNullOrEmpty(paymentId) && int.TryParse(srCode, out int srCodeValue) &&
                            DateTime.TryParse(dateOfOrder, out DateTime dateOfOrderValue) && decimal.TryParse(amount, out decimal amountValue) &&
                            int.TryParse(staffId, out int staffIdValue))
                        {
                            if (CheckForeignKeyExistence(connection, "students", "sr_code", srCodeValue) &&
                                CheckForeignKeyExistence(connection, "staff", "staff_id", staffIdValue))
                            {
                                string insertPaymentQuery = "INSERT INTO Payment (PAYMENT_ID, SR_CODE, DATE_OF_ORDER, AMOUNT, STAFF_ID) " +
                                                            "VALUES (@paymentId, @srCode, @dateOfOrder, @amount, @staffId)";

                                using (NpgsqlCommand insertPaymentCommand = new NpgsqlCommand(insertPaymentQuery, connection))
                                {
                                    insertPaymentCommand.Parameters.AddWithValue("@paymentId", Convert.ToInt32(paymentId));
                                    insertPaymentCommand.Parameters.AddWithValue("@srCode", srCodeValue);
                                    insertPaymentCommand.Parameters.AddWithValue("@dateOfOrder", dateOfOrderValue);
                                    insertPaymentCommand.Parameters.AddWithValue("@amount", amountValue);
                                    insertPaymentCommand.Parameters.AddWithValue("@staffId", staffIdValue);

                                    int paymentRowsAffected = insertPaymentCommand.ExecuteNonQuery();

                                    if (paymentRowsAffected > 0)
                                    {
                                        MessageBox.Show("Insert into Payment successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Insert into Payment failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("SR_CODE or STAFF_ID does not exist in the referenced table.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid data for payment. Please check the values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


        // Helper method to check if a foreign key value exists in the referenced table
        private bool CheckForeignKeyExistence(NpgsqlConnection connection, string tableName, string columnName, int value)
        {
            string query = $"SELECT 1 FROM {tableName} WHERE {columnName} = @value";

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@value", value);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
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
                        string updateQuery = "UPDATE payment SET " +
                                              "sr_code = @srCode, " +
                                              "date_of_order = @dateOfOrder, " +
                                              "amount = @amount, " +
                                              "staff_id = @staffId " +
                                              "WHERE payment_id = @paymentId";

                        using (NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@srCode", Convert.ToInt32(GetValueFromCell(row.Cells["sr_code"])));
                            updateCommand.Parameters.AddWithValue("@dateOfOrder", GetDateTimeValueFromCell(row.Cells["date_of_order"]));
                            updateCommand.Parameters.AddWithValue("@amount", GetDecimalValueFromCell(row.Cells["amount"]));
                            updateCommand.Parameters.AddWithValue("@staffId", Convert.ToInt32(GetIntValueFromCell(row.Cells["staff_id"])));

                            // Ensure payment_id is treated as an integer
                            if (int.TryParse(GetValueFromCell(row.Cells["payment_id"]), out int paymentId))
                            {
                                updateCommand.Parameters.AddWithValue("@paymentId", paymentId);

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
                                MessageBox.Show("Invalid payment_id value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private decimal GetDecimalValueFromCell(DataGridViewCell cell)
        {
            if (decimal.TryParse(GetValueFromCell(cell), out decimal result))
            {
                return result;
            }
            return 0;
        }

        private int GetIntValueFromCell(DataGridViewCell cell)
        {
            if (int.TryParse(GetValueFromCell(cell), out int result))
            {
                return result;
            }
            return 0;
        }
    }
}
