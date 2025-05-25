using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace Email_Manager
{
    public partial class Form1 : Form
    {
        private ContactQuery contactQuery;

        public Form1()
        {
            InitializeComponent();
            contactQuery = new ContactQuery();
            dataGridView1.AllowUserToAddRows = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (contactQuery.TestConnection())
            {
                lblStatus.Text = "Status: Database Connected";
                lblStatus.ForeColor = Color.Green;
                LoadCategories();
                LoadContacts();
            }
            else
            {
                lblStatus.Text = "Status: Failed to connect to database";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void LoadCategories()
        {
            try
            {
                DataTable dt = contactQuery.GetCategories();
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("All Categories");

                foreach (DataRow row in dt.Rows)
                {
                    cmbCategory.Items.Add(row["category"].ToString());
                }

                cmbCategory.SelectedIndex = 0;
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
                if (category == "All Categories")
                    category = "";

                DataTable dt = contactQuery.GetContacts(category);
                BindDataToGrid(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading contacts: " + ex.Message);
            }
        }

        private void BindDataToGrid(DataTable dt)
        {
            DataTable dtWithNo = dt.Copy();
            dtWithNo.Columns.Add("No", typeof(int)).SetOrdinal(0);

            for (int i = 0; i < dtWithNo.Rows.Count; i++)
            {
                dtWithNo.Rows[i]["No"] = i + 1;
            }

            dataGridView1.DataSource = dtWithNo;

            if (dataGridView1.Columns.Contains("id"))
                dataGridView1.Columns["id"].Visible = false;

            if (dataGridView1.Columns.Contains("Notes"))
                dataGridView1.Columns["Notes"].DisplayIndex = dataGridView1.Columns.Count - 1;

            StyleGrid();
        }

        private void StyleGrid()
        {
            dataGridView1.Columns["No"].Width = 40;
            dataGridView1.Columns["No"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            if (dataGridView1.Columns.Contains("Name"))
                dataGridView1.Columns["Name"].Width = 100;
            if (dataGridView1.Columns.Contains("Email"))
                dataGridView1.Columns["Email"].Width = 175;
            if (dataGridView1.Columns.Contains("Phone"))
                dataGridView1.Columns["Phone"].Width = 125;
            if (dataGridView1.Columns.Contains("Notes"))
                dataGridView1.Columns["Notes"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

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
                if (column.Name != "No")
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            dataGridView1.ReadOnly = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormContact form = new FormContact();
            form.ShowDialog();
            LoadContacts();
            LoadCategories();
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
                LoadCategories();
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
                    try
                    {
                        contactQuery.DeleteContact(id);
                        MessageBox.Show("Kontak berhasil dihapus.");
                        LoadContacts();
                        LoadCategories();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal hapus: " + ex.Message);
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
            string category = cmbCategory.SelectedItem.ToString();
            if (category == "All Categories")
                category = "";

            string keyword = txtSearch.Text;

            try
            {
                DataTable dt = contactQuery.SearchContacts(keyword, category);
                BindDataToGrid(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error search: " + ex.Message);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnViewLogs_Click(object sender, EventArgs e)
        {
            FormContactLogs logForm = new FormContactLogs();
            logForm.ShowDialog();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel Files (*.xlsx)|*.xlsx";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var workbook = new XLWorkbook();
                    var ws = workbook.Worksheets.Add("Contacts");

                    ws.Cell(1, 1).Value = "Name";
                    ws.Cell(1, 2).Value = "Email";
                    ws.Cell(1, 3).Value = "Phone";
                    ws.Cell(1, 4).Value = "Notes";
                    ws.Cell(1, 5).Value = "Category";

                    int row = 2;
                    foreach (DataGridViewRow dataRow in dataGridView1.Rows)
                    {
                        ws.Cell(row, 1).Value = dataRow.Cells["Name"].Value?.ToString();
                        ws.Cell(row, 2).Value = dataRow.Cells["Email"].Value?.ToString();
                        ws.Cell(row, 3).Value = dataRow.Cells["Phone"].Value?.ToString();
                        ws.Cell(row, 4).Value = dataRow.Cells["Notes"].Value?.ToString();
                        ws.Cell(row, 5).Value = dataRow.Cells["Category"].Value?.ToString();
                        row++;
                    }

                    ws.Range("A1:E1").Style.Font.Bold = true;
                    ws.Columns().AdjustToContents();

                    workbook.SaveAs(dialog.FileName);
                    MessageBox.Show("Kontak berhasil diekspor ke Excel.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exporting to Excel: " + ex.Message);
                }
            }
        }
    }
}
