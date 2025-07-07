using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Email_Manager.Controllers;

namespace Email_Manager
{
    public partial class FormContact : Form
    {
        /* ───── Field ─────────────────────────────────────────── */
        private readonly ContactController controller = new ContactController();
        private string selectedPhotoPath = string.Empty;
        private int contactId = -1;

        /* ───── Constructor: tambah baru ───────────────────────── */
        public FormContact()
        {
            InitializeComponent();
            InitializePhotoComponents();
            InitializeCategoryComboBox();
        }

        /* ───── Constructor: mode edit ─────────────────────────── */
        public FormContact(int id, string name, string email, string phone,
                           string notes, string category, string photoPath) : this()
        {
            contactId = id;
            selectedPhotoPath = photoPath;

            if (!string.IsNullOrEmpty(photoPath) && File.Exists(photoPath))
                picPhoto.Image = Image.FromFile(photoPath);

            txtName.Text = name;
            txtEmail.Text = email;
            txtPhone.Text = phone;
            txtNotes.Text = notes;

            if (comboCategory.Items.Contains(category))
                comboCategory.SelectedItem = category;
        }

       
        private void InitializePhotoComponents()
        {
            
        }

        /* ───── Isi Combo Kategori ─────────────────────────────── */
        private void InitializeCategoryComboBox()
        {
            comboCategory.Items.Clear();
            comboCategory.Items.AddRange(new[] { "Keluarga", "Teman", "Kerja", "Lainnya" });
            comboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboCategory.SelectedIndex = 0;
        }

        /* ───── Upload Foto ────────────────────────────────────── */
        private void BtnUploadPhoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                FileInfo fi = new FileInfo(ofd.FileName);
                if (fi.Length > 1_000_000)
                {
                    MessageBox.Show("Ukuran gambar maksimal 1 MB.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                selectedPhotoPath = ofd.FileName;
                picPhoto.Image = Image.FromFile(selectedPhotoPath);
            }

        }

        /* ───── Salin Foto ke folder lokal aplikasi ────────────── */
        private static string CopyPhotoToLocalFolder(string originalPath)
        {
            try
            {
                if (string.IsNullOrEmpty(originalPath) || !File.Exists(originalPath))
                    return string.Empty;

                string destDir = Path.Combine(Application.StartupPath, "images");
                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);

                string destPath = Path.Combine(destDir, Path.GetFileName(originalPath));
                File.Copy(originalPath, destPath, true);   // overwrite
                return destPath;
            }
            catch
            {
                return string.Empty;
            }
        }

        /* ───── Simpan / Update ────────────────────────────────── */
        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string notes = txtNotes.Text.Trim();
            string category = comboCategory.SelectedItem.ToString();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Nama dan Email tidak boleh kosong!", "Validasi",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string photoPathToSave = CopyPhotoToLocalFolder(selectedPhotoPath);

                int savedId = controller.SaveContact(
                    contactId,
                    name, email, phone, notes,
                    category,
                    photoPathToSave,
                    LoggedInUser.Username);

                string msg = contactId == -1
                             ? "Kontak berhasil ditambahkan!"
                             : "Kontak berhasil diperbarui!";
                MessageBox.Show(msg, "Sukses",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat menyimpan kontak: " + ex.Message,
                                "Kesalahan",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /* ───── Tutup form tanpa simpan ────────────────────────── */
        private void btnCancel_Click(object sender, EventArgs e) => Close();

        /* ───── Tombol lain (jika ada) ─────────────────────────── */
        private void button3_Click(object sender, EventArgs e) { }
    }

    /* ───── Data login user (sementara) ───────────────────────── */
    public static class LoggedInUser
    {
        public static string Username { get; set; } = "admin";
    }
}
