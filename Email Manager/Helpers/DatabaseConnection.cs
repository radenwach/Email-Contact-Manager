using System;
using MySql.Data.MySqlClient;

namespace Email_Manager
{
    public class DatabaseConnection
    {
        private const string _connectionString = "server=localhost;database=email_manager;uid=root;pwd=;";
        private readonly MySqlConnection _connection;

        public DatabaseConnection()
        {
            _connection = new MySqlConnection(_connectionString);
        }

        // Mengambil objek koneksi
        public MySqlConnection GetConnection() => _connection;

        // Membuka koneksi jika tertutup
        public void Open()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
                _connection.Open();
        }

        // Menutup koneksi jika terbuka
        public void Close()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
                _connection.Close();
        }
    }
}