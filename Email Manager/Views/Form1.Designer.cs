namespace Email_Manager.Views
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnViewLogs = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Location = new System.Drawing.Point(12, 80);
            this.dataGridView1.Size = new System.Drawing.Size(760, 300);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.TabIndex = 0;
            // 
            // cmbCategory
            // 
            this.cmbCategory.Location = new System.Drawing.Point(12, 12);
            this.cmbCategory.Size = new System.Drawing.Size(200, 21);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.TabIndex = 1;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(220, 12);
            this.txtSearch.Size = new System.Drawing.Size(200, 20);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.TabIndex = 2;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(430, 10);
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 400);
            this.btnAdd.Size = new System.Drawing.Size(75, 30);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(93, 400);
            this.btnEdit.Size = new System.Drawing.Size(75, 30);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.TabIndex = 5;
            this.btnEdit.Text = "Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(174, 400);
            this.btnDelete.Size = new System.Drawing.Size(75, 30);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(697, 12);
            this.btnLogout.Size = new System.Drawing.Size(75, 23);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.TabIndex = 7;
            this.btnLogout.Text = "Logout";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnViewLogs
            // 
            this.btnViewLogs.Location = new System.Drawing.Point(255, 400);
            this.btnViewLogs.Size = new System.Drawing.Size(90, 30);
            this.btnViewLogs.Name = "btnViewLogs";
            this.btnViewLogs.TabIndex = 8;
            this.btnViewLogs.Text = "View Logs";
            this.btnViewLogs.Click += new System.EventHandler(this.btnViewLogs_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(351, 400);
            this.btnExportExcel.Size = new System.Drawing.Size(90, 30);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.TabIndex = 9;
            this.btnExportExcel.Text = "Export Excel";
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(12, 45);
            this.lblStatus.Size = new System.Drawing.Size(400, 20);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "Status: ";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnViewLogs);
            this.Controls.Add(this.btnExportExcel);
            this.Controls.Add(this.lblStatus);
            this.Name = "Form1";
            this.Text = "Email Contact Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnViewLogs;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Label lblStatus;
    }
}
