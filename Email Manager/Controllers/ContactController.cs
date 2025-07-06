using System.Data;
using Email_Manager.Models;

namespace Email_Manager.Controllers
{
    public class ContactController
    {
        // Field model dibuat readonly karena tidak akan diubah setelah inisialisasi
        private readonly ContactModel model;

        // Constructor untuk inisialisasi model
        public ContactController()
        {
            model = new ContactModel();
        }

        // ================= OPERASI DATABASE =================
        public bool CheckDatabaseConnection() => model.TestConnection();

        // ================= OPERASI KATEGORI =================
        public DataTable GetAllCategories() => model.GetCategories();

        // ================= OPERASI KONTAK =================
        public DataTable GetAllContacts(string category = "") => model.GetContacts(category);

        public DataTable SearchContacts(string keyword, string category)
            => model.SearchContacts(keyword, category);

        public int SaveContact(
            int contactId,
            string name,
            string email,
            string phone,
            string notes,
            string category,
            string photoPath,
            string username)
            => model.SaveContact(contactId, name, email, phone, notes, category, photoPath, username);

        public void DeleteContactById(int id) => model.DeleteContact(id);

        // ================= OPERASI LOG =================
        public DataTable GetContactLogs() => model.GetContactLogs();
    }
}