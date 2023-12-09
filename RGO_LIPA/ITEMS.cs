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
    public partial class ITEMS : Form
    {
        // Define your connection string
        private const string ConnectionString = "Host=localhost;Port=5432;Database=rgo_lipa;Username=postgres;Password=postgres";

        public ITEMS()
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

        private void ITEMS_Load(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Assuming you want to display data from the Items table
                    string query = "SELECT * FROM Items ORDER BY SR_CODE ASC";

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
                        string insertItemsQuery = "INSERT INTO Items (ORDER_ITEMS_ID, SR_CODE, DATE_OF_ORDER, UNIFORM_PINS, ID_LACE, PAYMENT_ID) " +
                                                  "VALUES (@orderItemsId, @srCode, @dateOfOrder, @uniformPins, @idLace, @paymentId)";

                        using (NpgsqlCommand insertItemsCommand = new NpgsqlCommand(insertItemsQuery, connection))
                        {
                            string orderItemsId = row.Cells["ORDER_ITEMS_ID"].Value?.ToString() ?? string.Empty;
                            string srCode = row.Cells["SR_CODE"].Value?.ToString() ?? string.Empty;
                            string dateOfOrder = row.Cells["DATE_OF_ORDER"].Value?.ToString() ?? string.Empty;
                            string paymentId = row.Cells["PAYMENT_ID"].Value?.ToString() ?? string.Empty;

                            bool uniformPins = GetBooleanValue(row.Cells["UNIFORM_PINS"].Value);
                            bool idLace = GetBooleanValue(row.Cells["ID_LACE"].Value);

                            // Additional checks for data types and foreign key existence
                            if (int.TryParse(orderItemsId, out int orderItemsIdValue) &&
                                int.TryParse(srCode, out int srCodeValue) &&
                                DateTime.TryParse(dateOfOrder, out DateTime dateOfOrderValue) &&
                                int.TryParse(paymentId, out int paymentIdValue))
                            {
                                if (CheckForeignKeyExistence(connection, "students", "sr_code", srCodeValue) &&
                                    CheckForeignKeyExistence(connection, "payment", "payment_id", paymentIdValue))
                                {
                                    insertItemsCommand.Parameters.AddWithValue("@orderItemsId", orderItemsIdValue);
                                    insertItemsCommand.Parameters.AddWithValue("@srCode", srCodeValue);
                                    insertItemsCommand.Parameters.AddWithValue("@dateOfOrder", dateOfOrderValue);
                                    insertItemsCommand.Parameters.AddWithValue("@uniformPins", uniformPins);
                                    insertItemsCommand.Parameters.AddWithValue("@idLace", idLace);
                                    insertItemsCommand.Parameters.AddWithValue("@paymentId", paymentIdValue);

                                    int itemsRowsAffected = insertItemsCommand.ExecuteNonQuery();

                                    if (itemsRowsAffected > 0)
                                    {
                                        MessageBox.Show("Insert into Items successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Insert into Items failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Foreign key values do not exist in the referenced tables.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid data types.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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




        private bool GetBooleanValue(object value)
        {
            if (value == null || Convert.IsDBNull(value))
            {
                return false;
            }

            return Convert.ToBoolean(value);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        string updateQuery = "UPDATE Items SET " +
                                              "sr_code = @srCode, " +
                                              "date_of_order = @dateOfOrder, " +
                                              "uniform_pins = @uniformPins, " +
                                              "id_lace = @idLace, " +
                                              "payment_id = @paymentId " +
                                              "WHERE order_items_id = @orderItemsId";

                        using (NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@srCode", Convert.ToInt32(GetValueFromCell(row.Cells["SR_CODE"])));
                            updateCommand.Parameters.AddWithValue("@dateOfOrder", GetDateTimeValueFromCell(row.Cells["DATE_OF_ORDER"]));
                            updateCommand.Parameters.AddWithValue("@uniformPins", GetBoolValueFromCell(row.Cells["UNIFORM_PINS"]));
                            updateCommand.Parameters.AddWithValue("@idLace", GetBoolValueFromCell(row.Cells["ID_LACE"]));
                            updateCommand.Parameters.AddWithValue("@paymentId", Convert.ToInt32(GetValueFromCell(row.Cells["PAYMENT_ID"])));
                            updateCommand.Parameters.AddWithValue("@orderItemsId", Convert.ToInt32(GetValueFromCell(row.Cells["ORDER_ITEMS_ID"])));

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

        private bool GetBoolValueFromCell(DataGridViewCell cell)
        {
            if (cell.Value != null && bool.TryParse(cell.Value.ToString(), out bool result))
            {
                return result;
            }
            return false; // Return a default value if the conversion fails or the value is null
        }

    }
}
