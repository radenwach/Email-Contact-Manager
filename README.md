# Email Contact Manager

Email Contact Manager adalah aplikasi desktop yang memungkinkan pengguna untuk mengelola daftar kontak mereka, termasuk menambah, mengedit, menghapus, dan mencari kontak. Aplikasi ini dibangun dengan menggunakan C# di Visual Studio dan menghubungkan ke database MySQL untuk menyimpan data kontak.

## Fitur

- **Tambah Kontak**: Menambahkan kontak baru dengan informasi seperti nama, email, nomor telepon, dan catatan.
- **Edit Kontak**: Mengedit informasi kontak yang sudah ada.
- **Hapus Kontak**: Menghapus kontak dari database.
- **Cari Kontak**: Mencari kontak berdasarkan nama atau email.
- **Tampilan Tabel**: Menampilkan semua kontak dalam format tabel yang mudah dibaca dengan fitur pencarian.

## Teknologi yang Digunakan

- **C#**: Bahasa pemrograman utama untuk pengembangan aplikasi desktop.
- **Windows Forms**: Framework untuk membuat antarmuka pengguna grafis (GUI).
- **MySQL**: Sistem manajemen basis data yang digunakan untuk menyimpan data kontak.
- **Visual Studio**: IDE yang digunakan untuk mengembangkan aplikasi ini.

## Persyaratan Sistem

- **.NET Framework** 4.7.2 atau lebih tinggi.
- **MySQL** Server yang terinstal pada mesin lokal atau remote.
- **Visual Studio** untuk membuka dan menjalankan proyek.

## Instalasi

### 1. **Menyiapkan MySQL Database**
   - Pastikan MySQL terinstal di sistem Anda.
   - Buat database baru dengan nama `email_manager` menggunakan perintah berikut:
     ```sql
     CREATE DATABASE email_manager;
     ```
   - Buat tabel `contacts` untuk menyimpan data kontak:
     ```sql
     CREATE TABLE contacts (
         id INT AUTO_INCREMENT PRIMARY KEY,
         name VARCHAR(255) NOT NULL,
         email VARCHAR(255) NOT NULL,
         phone VARCHAR(20),
         notes TEXT
     );
     ```

### 2. **Mengkonfigurasi Koneksi Database**
   - Pastikan Anda mengubah pengaturan koneksi di dalam file `Form1.cs` untuk mencocokkan kredensial MySQL Anda:
     ```csharp
     private string connectionString = "server=localhost;database=email_manager;uid=root;pwd=;";
     ```
   - Gantilah `localhost` dengan alamat server MySQL Anda jika Anda menggunakan server jarak jauh.

### 3. **Membangun dan Menjalankan Aplikasi**
   - Buka proyek di **Visual Studio**.
   - Pilih **Build** > **Build Solution** untuk membangun aplikasi.
   - Jalankan aplikasi dengan memilih **Start** (atau tekan `F5`).
   - Aplikasi akan terbuka, dan Anda dapat mulai menambah, mengedit, atau menghapus kontak.

## Penggunaan

1. **Tambah Kontak**: Klik tombol "Add" untuk membuka form baru, kemudian isi nama, email, telepon, dan catatan kontak.
2. **Edit Kontak**: Pilih kontak dari daftar, lalu klik tombol "Edit". Data kontak akan dimuat ke dalam form untuk diubah.
3. **Hapus Kontak**: Pilih kontak dari daftar, lalu klik tombol "Delete" untuk menghapusnya dari database setelah konfirmasi.
4. **Cari Kontak**: Gunakan kolom pencarian di bagian atas untuk mencari kontak berdasarkan nama atau email.

## Struktur Proyek

- **Form1.cs**: Form utama yang menampilkan daftar kontak dan menyediakan fungsionalitas untuk menambah, mengedit, menghapus, dan mencari kontak.
- **FormContact.cs**: Form untuk menambah atau mengedit kontak. Ini menangani input pengguna dan menyimpan data ke dalam database.
- **Database**: Menggunakan MySQL untuk menyimpan data kontak, dengan tabel `contacts` yang memiliki kolom `id`, `name`, `email`, `phone`, dan `notes`.
