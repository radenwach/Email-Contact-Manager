using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ClosedXML.Excel;
using Email_Manager.Controllers;

namespace Email_Manager.Views
{
    public partial class Form1 : Form
    {
        private ContactController controller;
        private PictureBox picPhoto;
        private DataTable originalData; // Stores complete original data

        public Form1()
        {
            InitializeComponent();

            // Initialize and configure the photo display
            picPhoto = new PictureBox
            {
                Size = new Size(150, 150),
                Location = new Point(dataGridView1.Right + 10, dataGridView1.Top),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            this.Controls.Add(picPhoto);

            controller = new ContactController();
            dataGridView1.AllowUserToAddRows = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;

            if (controller.CheckDatabaseConnection())
            {
                lblStatus.Text = "Status: Database Connected";
                lblStatus.ForeColor = Color.Green;
                LoadCategories();
                LoadContacts();
            }
            else
            {
                lblStatus.Text = "Status: Database Not Connected";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void LoadCategories()
        {
            DataTable dt = controller.GetAllCategories();
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("All Categories");

            foreach (DataRow row in dt.Rows)
            {
                cmbCategory.Items.Add(row["category"].ToString());
            }

            cmbCategory.SelectedIndex = 0;
        }

        private void LoadContacts(string category = "")
        {
            if (category == "All Categories")
                category = "";

            originalData = controller.GetAllContacts(category);
            BindDataToGrid(originalData);
        }

        private void BindDataToGrid(DataTable dt)
        {
            DataTable dtWithNo = new DataTable();
            dtWithNo.Columns.Add("No", typeof(int));
            dtWithNo.Columns.Add("Name", typeof(string));
            dtWithNo.Columns.Add("Email", typeof(string));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow sourceRow = dt.Rows[i];
                DataRow newRow = dtWithNo.NewRow();
                newRow["No"] = i + 1;
                newRow["Name"] = sourceRow["Name"];
                newRow["Email"] = sourceRow["Email"];
                dtWithNo.Rows.Add(newRow);
            }

            dataGridView1.DataSource = dtWithNo;
            StyleGrid();
        }

        private void StyleGrid()
        {
            // Set general properties
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ReadOnly = true;

            // Column header style
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Row styles
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

            // Auto-size columns to fit the table
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (column.Name == "No")
                {
                    column.Width = 40;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var row = dataGridView1.SelectedRows[0];
                if (dataGridView1.Columns.Contains("photo_path"))
                {
                    string photoPath = row.Cells["photo_path"].Value?.ToString();
                    if (!string.IsNullOrEmpty(photoPath) && System.IO.File.Exists(photoPath))
                    {
                        picPhoto.Image = Image.FromFile(photoPath);
                    }
                    else
                    {
                        picPhoto.Image = null; // default image or blank
                    }
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && originalData != null && e.RowIndex < originalData.Rows.Count)
            {
                DataRow row = originalData.Rows[e.RowIndex];

                FormContactDetail detailForm = new FormContactDetail(
                    row["Name"].ToString(),
                    row["Email"].ToString(),
                    row["Phone"].ToString(),
                    row["Notes"].ToString(),
                    row["Category"].ToString(),
                    row["photo_path"].ToString()
                );
                detailForm.ShowDialog();
            }
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
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                if (selectedIndex < originalData.Rows.Count)
                {
                    DataRow row = originalData.Rows[selectedIndex];

                    FormContact form = new FormContact(
                        Convert.ToInt32(row["id"]),
                        row["Name"].ToString(),
                        row["Email"].ToString(),
                        row["Phone"].ToString(),
                        row["Notes"].ToString(),
                        row["Category"].ToString(),
                        row["photo_path"].ToString()
                    );
                    form.ShowDialog();
                    LoadContacts();
                    LoadCategories();
                }
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
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                if (selectedIndex < originalData.Rows.Count)
                {
                    int id = Convert.ToInt32(originalData.Rows[selectedIndex]["id"]);

                    if (MessageBox.Show("Hapus kontak?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        controller.DeleteContactById(id);
                        LoadContacts();
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

            DataTable dt = controller.SearchContacts(txtSearch.Text, category);
            BindDataToGrid(dt);
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
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var workbook = new XLWorkbook();
                    var ws = workbook.Worksheets.Add("Contacts");

                    // Header
                    ws.Cell(1, 1).Value = "Name";
                    ws.Cell(1, 2).Value = "Email";
                    ws.Cell(1, 3).Value = "Phone";
                    ws.Cell(1, 4).Value = "Notes";
                    ws.Cell(1, 5).Value = "Category";

                    // Data rows
                    int row = 2;
                    foreach (DataRow dataRow in originalData.Rows)
                    {
                        ws.Cell(row, 1).Value = dataRow["Name"].ToString();
                        ws.Cell(row, 2).Value = dataRow["Email"].ToString();
                        ws.Cell(row, 3).Value = dataRow["Phone"].ToString();
                        ws.Cell(row, 4).Value = dataRow["Notes"].ToString();
                        ws.Cell(row, 5).Value = dataRow["Category"].ToString();
                        row++;
                    }

                    // Formatting
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