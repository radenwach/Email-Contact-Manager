using MySql.Data.MySqlClient;
using System;

namespace Email_Manager
{
    public class UserQuery
    {
        private DatabaseConnection db;

        public UserQuery()
        {
            db = new DatabaseConnection();
        }

        public bool CheckLogin(string username, string password)
        {
            try
            {
                db.Open();
                string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
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
                db.Close();
            }
        }
    }
}
