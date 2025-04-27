using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Email_Manager
{
    public partial class FormContact : Form
    {
        private string connString = "server=localhost;database=email_manager;uid=root;pwd=;";
        private int contactId = -1; // Default -1 artinya tambah baru

        public FormContact()
        {
            InitializeComponent();
        }

        // Constructor untuk mode Edit
        public FormContact(int id, string name, string email, string phone, string notes)
        {
            InitializeComponent();
            contactId = id;
            txtName.Text = name;
            txtEmail.Text = email;
            txtPhone.Text = phone;
            txtNotes.Text = notes;
        }

        private void FormContact_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.LightGray;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Nama dan Email tidak boleh kosong!");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    string query = contactId == -1
                        ? "INSERT INTO contacts (name, email, phone, notes) VALUES (@name, @email, @phone, @notes)"
                        : "UPDATE contacts SET name = @name, email = @email, phone = @phone, notes = @notes WHERE id = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@notes", txtNotes.Text);

                    if (contactId != -1)
                        cmd.Parameters.AddWithValue("@id", contactId);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show(contactId == -1 ? "Kontak berhasil ditambahkan!" : "Kontak berhasil diperbarui!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
