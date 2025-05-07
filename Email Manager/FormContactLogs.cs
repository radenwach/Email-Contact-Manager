using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Email_Manager
{
    public partial class FormContactLogs : Form
    {
        private ContactQuery logQuery;
        private DataGridView dgvLogs;

        public FormContactLogs()
        {
            InitializeComponent();
            logQuery = new ContactQuery();
            InitializeLogGrid();
        }

        private void FormContactLogs_Load(object sender, EventArgs e)
        {
            LoadContactLogs();
        }

        private void InitializeLogGrid()
        {
            dgvLogs = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };

            Label lblTitle = new Label
            {
                Text = "Riwayat Log Perubahan Kontak",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter
            };

            this.Controls.Add(dgvLogs);
            this.Controls.Add(lblTitle);
        }

        private void LoadContactLogs()
        {
            try
            {
                DataTable dt = logQuery.GetContactLogs();
                dgvLogs.DataSource = dt;

                if (dgvLogs.Columns.Count > 0)
                {
                    dgvLogs.Columns["ID Kontak"].Width = 50;
                    dgvLogs.Columns["Aksi"].Width = 100;
                    dgvLogs.Columns["Tanggal & Waktu"].Width = 150;
                    dgvLogs.Columns["Deskripsi Perubahan"].Width = 500;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data log: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
