using MySql.Data.MySqlClient;
using System;

namespace Email_Manager
{
    public class UserQuery
    {
        private MySqlConnection connection;

        // Constructor untuk menginisialisasi koneksi
        public UserQuery()
        {
            string connectionString = "Server=localhost;Database=email_manager;Uid=root;Pwd="; // Sesuaikan dengan konfigurasi MySQL Anda
            connection = new MySqlConnection(connectionString);
        }

        // Method untuk membuka koneksi
        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        // Method untuk menutup koneksi
        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        // Method untuk memeriksa login
        public bool CheckLogin(string username, string password)
        {
            try
            {
                OpenConnection();

                string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Login error: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
