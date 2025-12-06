namespace src.GUI.NghiepVu
{
    partial class ChonSanPhamDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // Form Configuration
            // 
            this.Text = "Chọn sản phẩm";
            this.Size = new System.Drawing.Size(900, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 
            // Panel Search
            // 
            pnlSearch = new System.Windows.Forms.Panel();
            pnlSearch.Location = new System.Drawing.Point(20, 20);
            pnlSearch.Size = new System.Drawing.Size(860, 50);

            System.Windows.Forms.Label lblSearch = new System.Windows.Forms.Label();
            lblSearch.Text = "Tìm kiếm sản phẩm:";
            lblSearch.Location = new System.Drawing.Point(10, 15);
            lblSearch.AutoSize = true;

            txtSearch = new System.Windows.Forms.TextBox();
            txtSearch.Location = new System.Drawing.Point(150, 12);
            txtSearch.Size = new System.Drawing.Size(500, 25);
            txtSearch.PlaceholderText = "Nhập tên hoặc mã sản phẩm...";

            btnSearch = new System.Windows.Forms.Button();
            btnSearch.Text = "Tìm";
            btnSearch.Location = new System.Drawing.Point(670, 10);
            btnSearch.Size = new System.Drawing.Size(80, 30);
            btnSearch.BackColor = System.Drawing.Color.FromArgb(8, 133, 204);
            btnSearch.ForeColor = System.Drawing.Color.White;
            btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSearch.Click += BtnSearch_Click;

            pnlSearch.Controls.AddRange(new System.Windows.Forms.Control[] { lblSearch, txtSearch, btnSearch });

            // 
            // DataGridView
            // 
            dgvSanPham = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(dgvSanPham)).BeginInit();
            dgvSanPham.Location = new System.Drawing.Point(20, 80);
            dgvSanPham.Size = new System.Drawing.Size(860, 350);
            dgvSanPham.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvSanPham.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvSanPham.MultiSelect = false;
            dgvSanPham.AllowUserToAddRows = false;
            dgvSanPham.AllowUserToDeleteRows = false;
            dgvSanPham.ReadOnly = true;
            dgvSanPham.BackgroundColor = System.Drawing.Color.White;
            dgvSanPham.RowHeadersVisible = false;
            dgvSanPham.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(8, 133, 204);
            dgvSanPham.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvSanPham.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            dgvSanPham.EnableHeadersVisualStyles = false;
            dgvSanPham.CellClick += DgvSanPham_CellClick;

            // Add columns
            dgvSanPham.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "MSP", HeaderText = "Mã SP", DataPropertyName = "MSP" });
            dgvSanPham.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "TEN", HeaderText = "Tên sản phẩm", DataPropertyName = "TEN" });
            dgvSanPham.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "SL", HeaderText = "Tồn kho", DataPropertyName = "SL" });
            dgvSanPham.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "TIENN", HeaderText = "Giá", DataPropertyName = "TIENN" });

            dgvSanPham.Columns["SL"].DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dgvSanPham.Columns["TIENN"].DefaultCellStyle.Format = "N0";
            dgvSanPham.Columns["TIENN"].DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            ((System.ComponentModel.ISupportInitialize)(dgvSanPham)).EndInit();

            // 
            // Panel Input
            // 
            pnlInput = new System.Windows.Forms.Panel();
            pnlInput.Location = new System.Drawing.Point(20, 440);
            pnlInput.Size = new System.Drawing.Size(860, 60);
            pnlInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            lblSoLuong = new System.Windows.Forms.Label();
            lblSoLuong.Text = "Số lượng:";
            lblSoLuong.Location = new System.Drawing.Point(20, 20);
            lblSoLuong.AutoSize = true;

            nudSoLuong = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(nudSoLuong)).BeginInit();
            nudSoLuong.Location = new System.Drawing.Point(100, 17);
            nudSoLuong.Size = new System.Drawing.Size(150, 25);
            nudSoLuong.Minimum = 1;
            nudSoLuong.Maximum = 1000000;
            nudSoLuong.Value = 1;
            ((System.ComponentModel.ISupportInitialize)(nudSoLuong)).EndInit();

            lblGia = new System.Windows.Forms.Label();
            lblGia.Text = "Đơn giá:";
            lblGia.Location = new System.Drawing.Point(280, 20);
            lblGia.AutoSize = true;

            txtGia = new System.Windows.Forms.TextBox();
            txtGia.Location = new System.Drawing.Point(350, 17);
            txtGia.Size = new System.Drawing.Size(200, 25);
            txtGia.Text = "0";

            btnChon = new System.Windows.Forms.Button();
            btnChon.Text = "Chọn";
            btnChon.Location = new System.Drawing.Point(650, 12);
            btnChon.Size = new System.Drawing.Size(90, 35);
            btnChon.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            btnChon.ForeColor = System.Drawing.Color.White;
            btnChon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnChon.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            btnChon.Click += BtnChon_Click;

            btnHuy = new System.Windows.Forms.Button();
            btnHuy.Text = "Hủy";
            btnHuy.Location = new System.Drawing.Point(750, 12);
            btnHuy.Size = new System.Drawing.Size(90, 35);
            btnHuy.BackColor = System.Drawing.Color.Gray;
            btnHuy.ForeColor = System.Drawing.Color.White;
            btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnHuy.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            btnHuy.Click += BtnHuy_Click;

            pnlInput.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblSoLuong, nudSoLuong, lblGia, txtGia, btnChon, btnHuy
            });

            // 
            // Add to form
            // 
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                pnlSearch, dgvSanPham, pnlInput
            });

            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvSanPham;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.Label lblSoLuong;
        private System.Windows.Forms.NumericUpDown nudSoLuong;
        private System.Windows.Forms.Label lblGia;
        private System.Windows.Forms.TextBox txtGia;
        private System.Windows.Forms.Button btnChon;
        private System.Windows.Forms.Button btnHuy;
    }
}
