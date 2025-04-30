using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Email_Manager
{
    public partial class Form1 : Form
    {
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
                lblStatus.ForeColor = Color.Green;
                conn.Close();

                LoadContacts();
                LoadCategories();
            }
            catch
            {
                lblStatus.Text = "Status: Failed to connect to database";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void LoadCategories()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DISTINCT category FROM contacts";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    cmbCategory.Items.Clear();
                    cmbCategory.Items.Add("All Categories");
                    while (reader.Read())
                    {
                        cmbCategory.Items.Add(reader.GetString("category"));
                    }
                    cmbCategory.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
        }

        private void LoadContacts(string category = "")
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id, name, email, phone, notes, category FROM contacts";

                    if (!string.IsNullOrEmpty(category) && category != "All Categories")
                    {
                        query += " WHERE category = @category";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    if (category != "All Categories")
                    {
                        cmd.Parameters.AddWithValue("@category", category);
                    }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Tambahkan kolom nomor urut
                    DataTable dtWithNo = dt.Copy();
                    dtWithNo.Columns.Add("No", typeof(int)).SetOrdinal(0);
                    for (int i = 0; i < dtWithNo.Rows.Count; i++)
                    {
                        dtWithNo.Rows[i]["No"] = i + 1;
                    }
                    dataGridView1.DataSource = dtWithNo;

                    // Sembunyikan kolom id
                    dataGridView1.Columns["id"].Visible = false;

                    // Pindahkan kolom notes ke paling akhir
                    dataGridView1.Columns["Notes"].DisplayIndex = dataGridView1.Columns.Count - 1;

                    // Tampilan
                    dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

                    dataGridView1.Columns["No"].Width = 40;
                    dataGridView1.Columns["No"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    dataGridView1.Columns["Name"].Width = 100;
                    dataGridView1.Columns["Email"].Width = 175;
                    dataGridView1.Columns["Phone"].Width = 125;
                    dataGridView1.Columns["Notes"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    dataGridView1.EnableHeadersVisualStyles = false;
                    dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray;
                    dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    dataGridView1.RowsDefaultCellStyle.BackColor = Color.LightGray;
                    dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                    dataGridView1.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;
                    dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    }

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
                string name = row.Cells["Name"].Value.ToString();
                string email = row.Cells["Email"].Value.ToString();
                string phone = row.Cells["Phone"].Value.ToString();
                string notes = row.Cells["Notes"].Value.ToString();
                string category = row.Cells["Category"].Value.ToString();

                FormContact form = new FormContact(id, name, email, phone, notes, category);
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
            string selectedCategory = cmbCategory.SelectedItem.ToString();
            string searchText = txtSearch.Text;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT id, name, email, phone, notes, category FROM contacts WHERE (name LIKE @search OR email LIKE @search)";

                    if (selectedCategory != "All Categories")
                    {
                        query += " AND category = @category";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                    if (selectedCategory != "All Categories")
                    {
                        cmd.Parameters.AddWithValue("@category", selectedCategory);
                    }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Tambahkan kolom nomor urut
                    DataTable dtWithNo = dt.Copy();
                    dtWithNo.Columns.Add("No", typeof(int)).SetOrdinal(0);
                    for (int i = 0; i < dtWithNo.Rows.Count; i++)
                    {
                        dtWithNo.Rows[i]["No"] = i + 1;
                    }
                    dataGridView1.DataSource = dtWithNo;

                    dataGridView1.Columns["id"].Visible = false;
                    dataGridView1.Columns["notes"].DisplayIndex = dataGridView1.Columns.Count - 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error search: " + ex.Message);
            }
        }
    }
}
