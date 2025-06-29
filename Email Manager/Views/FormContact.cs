using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Email_Manager.Models;

namespace Email_Manager
{
    public partial class FormContact : Form
    {
        private string selectedPhotoPath = "";

        private int contactId = -1;

        public FormContact()
        {
            InitializeComponent();
            InitializePhotoComponents();
            InitializeCategoryComboBox();
        }

        // Overload constructor untuk mode edit
        public FormContact(int id, string name, string email, string phone, string notes, string category, string photoPath)
        {
            InitializeComponent();
            InitializePhotoComponents();

            selectedPhotoPath = photoPath;
            if (!string.IsNullOrEmpty(photoPath) && File.Exists(photoPath))
            {
                picPhoto.Image = Image.FromFile(photoPath);
            }

            InitializeCategoryComboBox();

            contactId = id;
            txtName.Text = name;
            txtEmail.Text = email;
            txtPhone.Text = phone;
            txtNotes.Text = notes;
            comboCategory.SelectedItem = category;
        }

        // Komponen gambar & tombol upload
        private void InitializePhotoComponents()
        {
        }

        // Upload gambar
        private void BtnUploadPhoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fileInfo = new FileInfo(ofd.FileName);
                    if (fileInfo.Length > 1_000_000) // max 1 MB
                    {
                        MessageBox.Show("Ukuran gambar maksimal 1 MB.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    selectedPhotoPath = ofd.FileName;
                    picPhoto.Image = Image.FromFile(selectedPhotoPath);
                }
            }
        }

        // Salin gambar ke folder lokal dan kembalikan path barunya
        private string CopyPhotoToLocalFolder(string originalPath)
        {
            try
            {
                if (string.IsNullOrEmpty(originalPath) || !File.Exists(originalPath))
                    return "";

                string appPath = Application.StartupPath;
                string photoDir = Path.Combine(appPath, "images");

                if (!Directory.Exists(photoDir))
                    Directory.CreateDirectory(photoDir);

                string fileName = Path.GetFileName(originalPath);
                string newPath = Path.Combine(photoDir, fileName);

                File.Copy(originalPath, newPath, true); // overwrite if exists
                return newPath;
            }
            catch
            {
                return "";
            }
        }

        // Combo kategori
        private void InitializeCategoryComboBox()
        {
            comboCategory.Items.Clear();
            comboCategory.Items.AddRange(new string[] { "Keluarga", "Teman", "Kerja", "Lainnya" });
            comboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboCategory.SelectedIndex = 0;
        }

        // Simpan atau update kontak
        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string notes = txtNotes.Text.Trim();
            string category = comboCategory.SelectedItem.ToString();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Nama dan Email tidak boleh kosong!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string photoPathToSave = CopyPhotoToLocalFolder(selectedPhotoPath);

                ContactModel model = new ContactModel();
                int savedId = model.SaveContact(
                    contactId,
                    name,
                    email,
                    phone,
                    notes,
                    category,
                    photoPathToSave,
                    LoggedInUser.Username
                );

                string message = contactId == -1 ? "Kontak berhasil ditambahkan!" : "Kontak berhasil diperbarui!";
                MessageBox.Show(message, "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat menyimpan kontak: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tutup form
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }

    // Data login user
    public static class LoggedInUser
    {
        public static string Username { get; set; }
    }
}
