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
    public partial class UNIFORM : Form
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=rgo_lipa;Username=postgres;Password=postgres";

        public UNIFORM()
        {
            InitializeComponent();
            CustomizeDataGridView();
        }

        private void CustomizeDataGridView()
        {
            // Customize the appearance of the DataGridView
            dataGridView1.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.RowTemplate.Height = 40;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(128, 0, 0);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
        }

        private void UNIFORM_Load(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM TypeOfUniform ORDER BY ORDER_UNIFORM_ID ASC";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;
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
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        string insertTypeOfUniformQuery = "INSERT INTO TypeOfUniform (ORDER_UNIFORM_ID, SR_CODE, DATE_OF_ORDER, WHITE_FABRIC, CHECKERED, PANTS, PE, PAYMENT_ID) " +
                                                          "VALUES (@orderUniformId, @srCode, @dateOfOrder, @whiteFabric, @checkered, @pants, @pe, @paymentId)";

                        using (NpgsqlCommand insertTypeOfUniformCommand = new NpgsqlCommand(insertTypeOfUniformQuery, connection))
                        {
                            insertTypeOfUniformCommand.Parameters.AddWithValue("@orderUniformId", GetIntValueFromCell(row.Cells["ORDER_UNIFORM_ID"]));
                            insertTypeOfUniformCommand.Parameters.AddWithValue("@srCode", GetIntValueFromCell(row.Cells["SR_CODE"]));
                            insertTypeOfUniformCommand.Parameters.AddWithValue("@dateOfOrder", GetDateTimeValueFromCell(row.Cells["DATE_OF_ORDER"]));
                            insertTypeOfUniformCommand.Parameters.AddWithValue("@whiteFabric", GetBoolValueFromCell(row.Cells["WHITE_FABRIC"]));
                            insertTypeOfUniformCommand.Parameters.AddWithValue("@checkered", GetBoolValueFromCell(row.Cells["CHECKERED"]));
                            insertTypeOfUniformCommand.Parameters.AddWithValue("@pants", GetBoolValueFromCell(row.Cells["PANTS"]));
                            insertTypeOfUniformCommand.Parameters.AddWithValue("@pe", GetBoolValueFromCell(row.Cells["PE"]));
                            insertTypeOfUniformCommand.Parameters.AddWithValue("@paymentId", GetIntValueFromCell(row.Cells["SR_CODE"]));

                            int typeOfUniformRowsAffected = insertTypeOfUniformCommand.ExecuteNonQuery();

                            if (typeOfUniformRowsAffected > 0)
                            {
                                MessageBox.Show("Insert into TypeOfUniform successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Insert into TypeOfUniform failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Helper method to get int value from cell
        private int GetIntValueFromCell(DataGridViewCell cell)
        {
            return cell.Value != null ? Convert.ToInt32(cell.Value) : 0; // Assuming default value is 0
        }

        // Helper method to get DateTime value from cell
        private DateTime GetDateTimeValueFromCell(DataGridViewCell cell)
        {
            return cell.Value != null ? Convert.ToDateTime(cell.Value) : DateTime.MinValue; // Assuming default value is DateTime.MinValue
        }

        // Helper method to get bool value from cell
        private bool GetBoolValueFromCell(DataGridViewCell cell)
        {
            // Use null-conditional operator to safely access cell.Value and handle null case
            return cell.Value?.ToString()?.ToLower() == "true";
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
                        // Update query for TypeOfUniform table
                        string updateTypeOfUniformQuery = "UPDATE typeofuniform SET white_fabric = @whiteFabric, " +
                                                          "checkered = @checkered, pants = @pants, pe = @pe " +
                                                          "WHERE order_uniform_id = @orderUniformId";

                        using (NpgsqlCommand updateTypeOfUniformCommand = new NpgsqlCommand(updateTypeOfUniformQuery, connection))
                        {
                            updateTypeOfUniformCommand.Parameters.AddWithValue("@whiteFabric", GetBoolValueFromCell(row.Cells["WHITE_FABRIC"]));
                            updateTypeOfUniformCommand.Parameters.AddWithValue("@checkered", GetBoolValueFromCell(row.Cells["CHECKERED"]));
                            updateTypeOfUniformCommand.Parameters.AddWithValue("@pants", GetBoolValueFromCell(row.Cells["PANTS"]));
                            updateTypeOfUniformCommand.Parameters.AddWithValue("@pe", GetBoolValueFromCell(row.Cells["PE"]));
                            updateTypeOfUniformCommand.Parameters.AddWithValue("@orderUniformId", Convert.ToInt32(GetValueFromCell(row.Cells["ORDER_UNIFORM_ID"])));

                            int rowsAffected = updateTypeOfUniformCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Update into TypeOfUniform successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Update into TypeOfUniform failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private string GetValueFromCell(DataGridViewCell cell, string defaultValue = "")
        {
            return cell.Value?.ToString() ?? defaultValue;
        }


    }
}
