using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Email_Manager
{
    public partial class FormContact : Form
    {
        private int contactId = -1;
        private DatabaseConnection dbConn;

        public FormContact()
        {
            InitializeComponent();
            dbConn = new DatabaseConnection();
            InitializeCategoryComboBox();
        }

        public FormContact(int id, string name, string email, string phone, string notes, string category)
        {
            InitializeComponent();
            dbConn = new DatabaseConnection();
            InitializeCategoryComboBox();

            contactId = id;
            txtName.Text = name;
            txtEmail.Text = email;
            txtPhone.Text = phone;
            txtNotes.Text = notes;
            comboCategory.SelectedItem = category;
        }

        private void FormContact_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.LightGray;
        }

        private void InitializeCategoryComboBox()
        {
            comboCategory.Items.Clear();
            comboCategory.Items.AddRange(new string[] { "Keluarga", "Teman", "Kerja", "Lainnya" });
            comboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboCategory.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Nama dan Email tidak boleh kosong!");
                return;
            }

            try
            {
                ContactQuery query = new ContactQuery();
                contactId = query.SaveContact(
                    contactId,
                    txtName.Text,
                    txtEmail.Text,
                    txtPhone.Text,
                    txtNotes.Text,
                    comboCategory.SelectedItem.ToString(),
                    LoggedInUser.Username
                );

                MessageBox.Show(contactId == -1 ? "Kontak berhasil ditambahkan!" : "Kontak berhasil diperbarui!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public static class LoggedInUser
    {
        public static string Username { get; set; }
    }
}
