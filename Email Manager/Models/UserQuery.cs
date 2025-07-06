using MySql.Data.MySqlClient;
using System;

namespace Email_Manager
{
    public class UserQuery
    {
        // Gunakan readonly untuk koneksi database
        private readonly DatabaseConnection _dbConnection;

        public UserQuery()
        {
            _dbConnection = new DatabaseConnection();
        }

        // Memeriksa login user dengan username dan password
        public bool CheckLogin(string username, string password)
        {
            try
            {
                _dbConnection.Open();

                const string query = @"
                    SELECT COUNT(*) 
                    FROM users 
                    WHERE username = @username 
                    AND password = @password";

                using (var command = new MySqlCommand(query, _dbConnection.GetConnection()))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal melakukan login. Error: {ex.Message}");
            }
            finally
            {
                _dbConnection.Close();
            }
        }
    }
}