using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Email_Manager
{
    public class ContactQuery
    {
        private DatabaseConnection dbConn;

        public ContactQuery()
        {
            dbConn = new DatabaseConnection();
        }

       
        public int SaveContact(int contactId, string name, string email, string phone, string notes, string category, string username)
        {
            int savedContactId = contactId;
            try
            {
                MySqlConnection connection = dbConn.GetConnection();
                dbConn.Open();

                string query;
                string action;

                if (contactId == -1)
                {
                    query = "INSERT INTO contacts (name, email, phone, notes, category, created_at, updated_at) " +
                            "VALUES (@name, @email, @phone, @notes, @category, @created_at, @updated_at)";
                    action = "Tambah";
                }
                else
                {
                    query = "UPDATE contacts SET name = @name, email = @email, phone = @phone, notes = @notes, " +
                            "category = @category, updated_at = @updated_at WHERE id = @id";
                    action = "Update";
                }

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@notes", notes);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);

                    if (contactId == -1)
                    {
                        cmd.Parameters.AddWithValue("@created_at", DateTime.Now);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@id", contactId);
                    }

                    cmd.ExecuteNonQuery();
                }

                if (contactId == -1)
                {
                    MySqlCommand getIdCmd = new MySqlCommand("SELECT LAST_INSERT_ID()", connection);
                    savedContactId = Convert.ToInt32(getIdCmd.ExecuteScalar());
                }

                // Log perubahan
                string description = $"Nama: {name}, Email: {email}, Phone: {phone}, Category: {category}";
                string logQuery = "INSERT INTO contact_logs (contact_id, action, changed_by, changed_at, description) " +
                                  "VALUES (@contact_id, @action, @changed_by, @changed_at, @description)";
                using (MySqlCommand logCmd = new MySqlCommand(logQuery, connection))
                {
                    logCmd.Parameters.AddWithValue("@contact_id", savedContactId);
                    logCmd.Parameters.AddWithValue("@action", action);
                    logCmd.Parameters.AddWithValue("@changed_by", username);
                    logCmd.Parameters.AddWithValue("@changed_at", DateTime.Now);
                    logCmd.Parameters.AddWithValue("@description", description);
                    logCmd.ExecuteNonQuery();
                }

                dbConn.Close();
            }
            catch
            {
                dbConn.Close();
                throw;
            }

            return savedContactId;
        }

      
        public DataTable GetCategories()
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlConnection conn = dbConn.GetConnection();
                dbConn.Open();
                string query = "SELECT DISTINCT category FROM contacts";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            finally
            {
                dbConn.Close();
            }
            return dt;
        }

        
        public DataTable GetContacts(string category = "")
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlConnection conn = dbConn.GetConnection();
                dbConn.Open();

                string query = "SELECT id, name, email, phone, notes, category FROM contacts";
                if (!string.IsNullOrEmpty(category) && category != "All Categories")
                    query += " WHERE category = @category";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                if (!string.IsNullOrEmpty(category) && category != "All Categories")
                    cmd.Parameters.AddWithValue("@category", category);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            finally
            {
                dbConn.Close();
            }
            return dt;
        }

       
        public DataTable SearchContacts(string searchText, string category = "")
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlConnection conn = dbConn.GetConnection();
                dbConn.Open();

                string query = "SELECT id, name, email, phone, notes, category FROM contacts WHERE (name LIKE @search OR email LIKE @search)";
                if (!string.IsNullOrEmpty(category) && category != "All Categories")
                    query += " AND category = @category";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search", $"%{searchText}%");

                if (!string.IsNullOrEmpty(category) && category != "All Categories")
                    cmd.Parameters.AddWithValue("@category", category);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            finally
            {
                dbConn.Close();
            }
            return dt;
        }

        
        public void DeleteContact(int id)
        {
            try
            {
                MySqlConnection conn = dbConn.GetConnection();
                dbConn.Open();
                string query = "DELETE FROM contacts WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                dbConn.Close();
            }
        }

    
        public bool TestConnection()
        {
            try
            {
                dbConn.Open();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                dbConn.Close();
            }
        }

    
        public DataTable GetContactLogs()
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlConnection conn = dbConn.GetConnection();
                dbConn.Open();

                string query = @"
                    SELECT 
                        contact_id AS 'ID Kontak',
                        action AS 'Aksi',
                        changed_at AS 'Tanggal & Waktu',
                        description AS 'Deskripsi Perubahan'
                    FROM contact_logs
                    ORDER BY changed_at DESC";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            finally
            {
                dbConn.Close();
            }
            return dt;
        }
    }
}
