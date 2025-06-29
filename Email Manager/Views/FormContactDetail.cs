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
            this.Size = new Size(400, 450);
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblName = new Label() { Text = "Nama:", Left = 20, Top = 20, Width = 100 };
            Label lblEmail = new Label() { Text = "Email:", Left = 20, Top = 60, Width = 100 };
            Label lblPhone = new Label() { Text = "Telepon:", Left = 20, Top = 100, Width = 100 };
            Label lblNotes = new Label() { Text = "Catatan:", Left = 20, Top = 140, Width = 100 };
            Label lblCategory = new Label() { Text = "Kategori:", Left = 20, Top = 180, Width = 100 };

            Label valName = new Label() { Text = name, Left = 130, Top = 20, Width = 220 };
            Label valEmail = new Label() { Text = email, Left = 130, Top = 60, Width = 220 };
            Label valPhone = new Label() { Text = phone, Left = 130, Top = 100, Width = 220 };
            Label valNotes = new Label() { Text = notes, Left = 130, Top = 140, Width = 220 };
            Label valCategory = new Label() { Text = category, Left = 130, Top = 180, Width = 220 };

            PictureBox picPhoto = new PictureBox()
            {
                Left = 130,
                Top = 220,
                Width = 120,
                Height = 120,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            if (!string.IsNullOrEmpty(photoPath) && File.Exists(photoPath))
                picPhoto.Image = Image.FromFile(photoPath);

            this.Controls.Add(lblName);
            this.Controls.Add(lblEmail);
            this.Controls.Add(lblPhone);
            this.Controls.Add(lblNotes);
            this.Controls.Add(lblCategory);
            this.Controls.Add(valName);
            this.Controls.Add(valEmail);
            this.Controls.Add(valPhone);
            this.Controls.Add(valNotes);
            this.Controls.Add(valCategory);
            this.Controls.Add(picPhoto);
        }
    }
}
