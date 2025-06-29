using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Email_Manager.Views
{
    public partial class FormContactDetail : Form
    {
        public FormContactDetail(string name, string email, string phone, string notes, string category, string photoPath)
        {
            this.Text = "Detail Kontak";
            this.Size = new Size(500, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            // PictureBox untuk foto
            PictureBox picPhoto = new PictureBox()
            {
                Left = 330,
                Top = 20,
                Width = 120,
                Height = 120,
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
                // Tampilkan default avatar dari resource
                picPhoto.Image = Properties.Resources.default_avatar; // Pastikan nama file-nya cocok di Resources
            }

            // Label-label
            Label lblName = new Label() { Text = "Nama :", Left = 20, Top = 20, Width = 100 };
            Label lblEmail = new Label() { Text = "Email :", Left = 20, Top = 50, Width = 100 };
            Label lblPhone = new Label() { Text = "Telepon :", Left = 20, Top = 80, Width = 100 };
            Label lblNotes = new Label() { Text = "Catatan :", Left = 20, Top = 110, Width = 100 };
            Label lblCategory = new Label() { Text = "Kategori :", Left = 20, Top = 140, Width = 100 };

            Label valName = new Label() { Text = name, Left = 130, Top = 20, Width = 180 };
            Label valEmail = new Label() { Text = email, Left = 130, Top = 50, Width = 180 };
            Label valPhone = new Label() { Text = phone, Left = 130, Top = 80, Width = 180 };
            Label valNotes = new Label() { Text = notes, Left = 130, Top = 110, Width = 180 };
            Label valCategory = new Label() { Text = category, Left = 130, Top = 140, Width = 180 };

            // Tambahkan ke form
            this.Controls.Add(lblName); this.Controls.Add(valName);
            this.Controls.Add(lblEmail); this.Controls.Add(valEmail);
            this.Controls.Add(lblPhone); this.Controls.Add(valPhone);
            this.Controls.Add(lblNotes); this.Controls.Add(valNotes);
            this.Controls.Add(lblCategory); this.Controls.Add(valCategory);
            this.Controls.Add(picPhoto);
        }
    }
}
