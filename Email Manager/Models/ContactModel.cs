using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Email_Manager.Models
{
    public class ContactModel
    {
        private readonly DatabaseConnection _dbConn;

        public ContactModel()
        {
            _dbConn = new DatabaseConnection();
        }

        public bool TestConnection()
        {
            try
            {
                _dbConn.Open();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                _dbConn.Close();
            }
        }

        public DataTable GetCategories()
        {
            var dt = new DataTable();

            try
            {
                var conn = _dbConn.GetConnection();
                _dbConn.Open();

                const string query = "SELECT DISTINCT category FROM contacts";
                using (var cmd = new MySqlCommand(query, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            finally
            {
                _dbConn.Close();
            }

            return dt;
        }

        public DataTable GetContacts(string category = "")
        {
            var dt = new DataTable();

            try
            {
                var conn = _dbConn.GetConnection();
                _dbConn.Open();

                var query = "SELECT id, name, email, phone, notes, category, photo_path FROM contacts";

                if (!string.IsNullOrEmpty(category) && category != "All Categories")
                {
                    query += " WHERE category = @category";
                }

                using (var cmd = new MySqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(category) && category != "All Categories")
                    {
                        cmd.Parameters.AddWithValue("@category", category);
                    }

                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            finally
            {
                _dbConn.Close();
            }

            return dt;
        }

        public DataTable SearchContacts(string keyword, string category = "")
        {
            var dt = new DataTable();

            try
            {
                var conn = _dbConn.GetConnection();
                _dbConn.Open();

                var query = @"SELECT id, name, email, phone, notes, category, photo_path 
                              FROM contacts 
                              WHERE (name LIKE @keyword OR email LIKE @keyword)";

                if (!string.IsNullOrEmpty(category) && category != "All Categories")
                {
                    query += " AND category = @category";
                }

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    if (!string.IsNullOrEmpty(category) && category != "All Categories")
                    {
                        cmd.Parameters.AddWithValue("@category", category);
                    }

                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            finally
            {
                _dbConn.Close();
            }

            return dt;
        }

        public int SaveContact(int contactId, string name, string email, string phone,
                             string notes, string category, string photoPath, string username)
        {
            int savedContactId = contactId;

            try
            {
                var connection = _dbConn.GetConnection();
                _dbConn.Open();

                string query;
                string action;

                if (contactId == -1)
                {
                    query = @"INSERT INTO contacts (name, email, phone, notes, category, photo_path, created_at, updated_at) 
                             VALUES (@name, @email, @phone, @notes, @category, @photo_path, @created_at, @updated_at)";
                    action = "Tambah";
                }
                else
                {
                    query = @"UPDATE contacts 
                             SET name = @name, email = @email, phone = @phone, notes = @notes, 
                                 category = @category, photo_path = @photo_path, updated_at = @updated_at 
                             WHERE id = @id";
                    action = "Update";
                }

                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@notes", notes);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@photo_path", photoPath);
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
                    using (var getIdCmd = new MySqlCommand("SELECT LAST_INSERT_ID()", connection))
                    {
                        savedContactId = Convert.ToInt32(getIdCmd.ExecuteScalar());
                    }
                }

                // Log perubahan
                var description = $"Nama: {name}, Email: {email}, Phone: {phone}, Category: {category}, Photo: {photoPath}";
                const string logQuery = @"INSERT INTO contact_logs (contact_id, action, changed_by, changed_at, description) 
                                        VALUES (@contact_id, @action, @changed_by, @changed_at, @description)";

                using (var logCmd = new MySqlCommand(logQuery, connection))
                {
                    logCmd.Parameters.AddWithValue("@contact_id", savedContactId);
                    logCmd.Parameters.AddWithValue("@action", action);
                    logCmd.Parameters.AddWithValue("@changed_by", username);
                    logCmd.Parameters.AddWithValue("@changed_at", DateTime.Now);
                    logCmd.Parameters.AddWithValue("@description", description);
                    logCmd.ExecuteNonQuery();
                }
            }
            catch
            {
                _dbConn.Close();
                throw;
            }
            finally
            {
                _dbConn.Close();
            }

            return savedContactId;
        }

        public void DeleteContact(int id)
        {
            try
            {
                var conn = _dbConn.GetConnection();
                _dbConn.Open();

                const string query = "DELETE FROM contacts WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                _dbConn.Close();
            }
        }

        public DataTable GetContactLogs()
        {
            var dt = new DataTable();

            try
            {
                var conn = _dbConn.GetConnection();
                _dbConn.Open();

                const string query = @"SELECT 
                                        contact_id AS 'ID Kontak',
                                        action AS 'Aksi',
                                        changed_at AS 'Tanggal & Waktu',
                                        description AS 'Deskripsi Perubahan'
                                    FROM contact_logs
                                    ORDER BY changed_at DESC";

                using (var cmd = new MySqlCommand(query, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            finally
            {
                _dbConn.Close();
            }

            return dt;
        }
    }
}