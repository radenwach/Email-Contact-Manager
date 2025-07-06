using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Email_Manager.Views
{
    public partial class FormContactDetail : Form
    {
        // Buat font global untuk digunakan semua kontrol
        private readonly Font _segoeFont = new Font("Segoe UI Semibold", 9f, FontStyle.Bold, GraphicsUnit.Point);

        public FormContactDetail(string name, string email, string phone, string notes, string category, string photoPath)
        {
            this.Text = "Detail Kontak";
            this.Size = new Size(500, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            // PictureBox untuk foto
            PictureBox picPhoto = new PictureBox()
            {
                Left = 310,
                Top = 20,
                Width = 159,
                Height = 150,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            // Tampilkan gambar dari file jika ada, kalau tidak tampilkan default
            if (!string.IsNullOrEmpty(photoPath) && File.Exists(photoPath))
            {
                picPhoto.Image = Image.FromFile(photoPath);
            }
            else
            {
                picPhoto.Image = Properties.Resources.default_avatar;
            }

            // Label-label dengan font yang konsisten
            Label lblName = new Label()
            {
                Text = "Nama :",
                Left = 20,
                Top = 20,
                Width = 100,
                Font = _segoeFont
            };

            Label lblEmail = new Label()
            {
                Text = "Email :",
                Left = 20,
                Top = 50,
                Width = 100,
                Font = _segoeFont
            };

            Label lblPhone = new Label()
            {
                Text = "Telepon :",
                Left = 20,
                Top = 80,
                Width = 100,
                Font = _segoeFont
            };

            Label lblNotes = new Label()
            {
                Text = "Catatan :",
                Left = 20,
                Top = 110,
                Width = 100,
                Font = _segoeFont
            };

            Label lblCategory = new Label()
            {
                Text = "Kategori :",
                Left = 20,
                Top = 140,
                Width = 100,
                Font = _segoeFont
            };

            // Value labels dengan font yang sama
            Label valName = new Label()
            {
                Text = name,
                Left = 130,
                Top = 20,
                Width = 180,
                Font = _segoeFont
            };

            Label valEmail = new Label()
            {
                Text = email,
                Left = 130,
                Top = 50,
                Width = 180,
                Font = _segoeFont
            };

            Label valPhone = new Label()
            {
                Text = phone,
                Left = 130,
                Top = 80,
                Width = 180,
                Font = _segoeFont
            };

            Label valNotes = new Label()
            {
                Text = notes,
                Left = 130,
                Top = 110,
                Width = 180,
                Font = _segoeFont
            };

            Label valCategory = new Label()
            {
                Text = category,
                Left = 130,
                Top = 140,
                Width = 180,
                Font = _segoeFont
            };

            // Tambahkan ke form
            this.Controls.AddRange(new Control[] {
                lblName, valName,
                lblEmail, valEmail,
                lblPhone, valPhone,
                lblNotes, valNotes,
                lblCategory, valCategory,
                picPhoto
            });
        }

        // Bersihkan resource font ketika form di dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _segoeFont?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}