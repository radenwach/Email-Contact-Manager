using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Email_Manager
{
    public partial class Form1 : Form
    {
        // Deklarasi koneksi database
        private string connectionString = "server=localhost;database=email_manager;uid=root;pwd=;";
        private MySqlConnection conn;

        public Form1()
        {
            InitializeComponent();
            conn = new MySqlConnection(connectionString);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                this.BackColor = Color.LightGray;

                conn.Open();
                lblStatus.Text = "Status: Database Connected";
                lblStatus.ForeColor = Color.Green; // Warna teks hijau kalau berhasil
                conn.Close();

                LoadContacts();
            }
            catch
            {
                lblStatus.Text = "Status: Failed to connect to database";
                lblStatus.ForeColor = Color.Red; // Warna teks merah kalau error
            }
        }


        private void LoadContacts()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM contacts";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;

                    // Set properties for better appearance
                    dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
                    dataGridView1.Columns["id"].Width = 30;
                    dataGridView1.Columns["name"].Width = 100;
                    dataGridView1.Columns["email"].Width = 175;
                    dataGridView1.Columns["phone"].Width = 125;
                    dataGridView1.Columns["notes"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    // Setting Header Style
                    dataGridView1.EnableHeadersVisualStyles = false;
                    dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray;
                    dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

                    // Set row style for better readability
                    dataGridView1.RowsDefaultCellStyle.BackColor = Color.LightGray;
                    dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                    dataGridView1.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;
                    dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

                    // Set text alignment
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    }

                    // Disable editing to avoid user changes
                    dataGridView1.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormContact form = new FormContact();
            form.ShowDialog();
            LoadContacts();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int id = Convert.ToInt32(row.Cells["id"].Value);
                string name = row.Cells["name"].Value.ToString();
                string email = row.Cells["email"].Value.ToString();
                string phone = row.Cells["phone"].Value.ToString();
                string notes = row.Cells["notes"].Value.ToString();

                FormContact form = new FormContact(id, name, email, phone, notes);
                form.ShowDialog();
                LoadContacts();
            }
            else
            {
                MessageBox.Show("Pilih kontak yang ingin diedit.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Yakin ingin menghapus kontak ini?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);

                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            conn.Open();
                            string query = "DELETE FROM contacts WHERE id = @id";
                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Kontak berhasil dihapus.");
                            LoadContacts();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Gagal hapus: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Pilih kontak yang ingin dihapus.");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM contacts WHERE name LIKE @search OR email LIKE @search";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error search: " + ex.Message);
            }
        }
    }
}
