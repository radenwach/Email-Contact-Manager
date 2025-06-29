using System;
using System.Windows.Forms;
using Email_Manager.Models; // tambahkan ini

namespace Email_Manager
{
    public partial class FormContact : Form
    {
        private int contactId = -1;

        public FormContact()
        {
            InitializeComponent();
            InitializeCategoryComboBox();
        }

        // Overload constructor untuk mode edit
        public FormContact(int id, string name, string email, string phone, string notes, string category)
        {
            InitializeComponent();
            InitializeCategoryComboBox();

            contactId = id;
            txtName.Text = name;
            txtEmail.Text = email;
            txtPhone.Text = phone;
            txtNotes.Text = notes;
            comboCategory.SelectedItem = category;
        }

        // Inisialisasi kategori
        private void InitializeCategoryComboBox()
        {
            comboCategory.Items.Clear();
            comboCategory.Items.AddRange(new string[] { "Keluarga", "Teman", "Kerja", "Lainnya" });
            comboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboCategory.SelectedIndex = 0;
        }

        // Simpan kontak baru atau update yang lama
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
                ContactModel model = new ContactModel(); // ganti dari ContactQuery
                int savedId = model.SaveContact(
                    contactId,
                    name,
                    email,
                    phone,
                    notes,
                    category,
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

        // Tutup form tanpa menyimpan
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    // Menyimpan username pengguna yang login
    public static class LoggedInUser
    {
        public static string Username { get; set; }
    }
}
