using System;
using System.Windows.Forms;
using Email_Manager.Views;


namespace Email_Manager
{
    public partial class FormLogin : Form
    {
        private UserQuery dbHelper;

        public FormLogin()
        {
            InitializeComponent();
            dbHelper = new UserQuery();
            this.FormClosed += (s, args) => Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            try
            {
                bool isLoginValid = dbHelper.CheckLogin(username, password);

                if (isLoginValid)
                {
                    // Login berhasil, buka Form1
                    this.Hide();
                    Form1 mainForm = new Form1();
                    mainForm.Show();
                }
                else
                {
                    lblStatus.Text = "Username atau password salah!";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();  // Menutup aplikasi secara langsung
        }
    }
}
