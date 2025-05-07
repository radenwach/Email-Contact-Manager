using System;
using MySql.Data.MySqlClient;

namespace Email_Manager
{
    public class DatabaseConnection
    {
        private string connectionString = "server=localhost;database=email_manager;uid=root;pwd=;";
        private MySqlConnection connection;

        public DatabaseConnection()
        {
            connection = new MySqlConnection(connectionString);
        }

        public MySqlConnection GetConnection()
        {
            return connection;
        }

        public void Open()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        public void Close()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
    }
}
