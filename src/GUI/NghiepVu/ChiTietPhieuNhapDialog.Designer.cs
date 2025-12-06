namespace src.GUI.NghiepVu
{
    partial class ChiTietPhieuNhapDialog
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
            this.Text = "Chi tiết Phiếu Nhập";
            this.Size = new System.Drawing.Size(1000, 740);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 
            // Title
            // 
            lblTitle = new System.Windows.Forms.Label();
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(8, 133, 204);
            lblTitle.Location = new System.Drawing.Point(20, 20);
            lblTitle.Size = new System.Drawing.Size(960, 40);
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // Panel Info
            // 
            pnlInfo = new System.Windows.Forms.Panel();
            pnlInfo.Location = new System.Drawing.Point(20, 70);
            pnlInfo.Size = new System.Drawing.Size(960, 120);
            pnlInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // Row 1
            lblMaPhieu = new System.Windows.Forms.Label();
            lblMaPhieu.Text = "Mã phiếu:";
            lblMaPhieu.Location = new System.Drawing.Point(20, 20);
            lblMaPhieu.AutoSize = true;

            txtMaPhieu = new System.Windows.Forms.TextBox();
            txtMaPhieu.Location = new System.Drawing.Point(150, 17);
            txtMaPhieu.Size = new System.Drawing.Size(150, 25);
            txtMaPhieu.ReadOnly = true;
            txtMaPhieu.BackColor = System.Drawing.Color.LightGray;

            lblNhaCungCap = new System.Windows.Forms.Label();
            lblNhaCungCap.Text = "Nhà cung cấp:";
            lblNhaCungCap.Location = new System.Drawing.Point(330, 20);
            lblNhaCungCap.AutoSize = true;

            cboNhaCungCap = new System.Windows.Forms.ComboBox();
            cboNhaCungCap.Location = new System.Drawing.Point(460, 17);
            cboNhaCungCap.Size = new System.Drawing.Size(250, 25);
            cboNhaCungCap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Row 2
            lblNhanVien = new System.Windows.Forms.Label();
            lblNhanVien.Text = "Nhân viên:";
            lblNhanVien.Location = new System.Drawing.Point(20, 60);
            lblNhanVien.AutoSize = true;

            cboNhanVien = new System.Windows.Forms.ComboBox();
            cboNhanVien.Location = new System.Drawing.Point(150, 57);
            cboNhanVien.Size = new System.Drawing.Size(250, 25);
            cboNhanVien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            lblThoiGian = new System.Windows.Forms.Label();
            lblThoiGian.Text = "Thời gian:";
            lblThoiGian.Location = new System.Drawing.Point(430, 60);
            lblThoiGian.AutoSize = true;

            dtpThoiGian = new System.Windows.Forms.DateTimePicker();
            dtpThoiGian.Location = new System.Drawing.Point(530, 57);
            dtpThoiGian.Size = new System.Drawing.Size(180, 25);
            dtpThoiGian.Format = System.Windows.Forms.DateTimePickerFormat.Short;

            lblTrangThai = new System.Windows.Forms.Label();
            lblTrangThai.Text = "Trạng thái:";
            lblTrangThai.Location = new System.Drawing.Point(730, 60);
            lblTrangThai.AutoSize = true;

            txtTrangThai = new System.Windows.Forms.TextBox();
            txtTrangThai.Location = new System.Drawing.Point(820, 57);
            txtTrangThai.Size = new System.Drawing.Size(120, 25);
            txtTrangThai.ReadOnly = true;
            txtTrangThai.BackColor = System.Drawing.Color.LightGray;

            pnlInfo.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblMaPhieu, txtMaPhieu, lblNhaCungCap, cboNhaCungCap,
                lblNhanVien, cboNhanVien, lblThoiGian, dtpThoiGian,
                lblTrangThai, txtTrangThai
            });

            // 
            // Panel Chi Tiet
            // 
            pnlChiTiet = new System.Windows.Forms.Panel();
            pnlChiTiet.Location = new System.Drawing.Point(20, 200);
            pnlChiTiet.Size = new System.Drawing.Size(960, 370);
            pnlChiTiet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            lblChiTiet = new System.Windows.Forms.Label();
            lblChiTiet.Text = "Danh sách sản phẩm:";
            lblChiTiet.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            lblChiTiet.Location = new System.Drawing.Point(10, 10);
            lblChiTiet.AutoSize = true;

            dgvChiTiet = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(dgvChiTiet)).BeginInit();
            dgvChiTiet.Location = new System.Drawing.Point(10, 40);
            dgvChiTiet.Size = new System.Drawing.Size(940, 270);
            dgvChiTiet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.MultiSelect = false;
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.AllowUserToDeleteRows = false;
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.BackgroundColor = System.Drawing.Color.White;
            dgvChiTiet.RowHeadersVisible = false;
            dgvChiTiet.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(8, 133, 204);
            dgvChiTiet.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);
            dgvChiTiet.EnableHeadersVisualStyles = false;

            // Add columns
            dgvChiTiet.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "MSP", HeaderText = "Mã SP", Width = 80 });
            dgvChiTiet.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "TenSP", HeaderText = "Tên sản phẩm", Width = 200 });
            dgvChiTiet.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "SL", HeaderText = "Số lượng", Width = 100 });
            dgvChiTiet.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "TIENNHAP", HeaderText = "Đơn giá", Width = 120 });
            dgvChiTiet.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "ThanhTien", HeaderText = "Thành tiền", Width = 120 });

            dgvChiTiet.Columns["SL"].DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dgvChiTiet.Columns["TIENNHAP"].DefaultCellStyle.Format = "N0";
            dgvChiTiet.Columns["TIENNHAP"].DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            ((System.ComponentModel.ISupportInitialize)(dgvChiTiet)).EndInit();

            btnThemSP = new System.Windows.Forms.Button();
            btnThemSP.Text = "Thêm sản phẩm";
            btnThemSP.Location = new System.Drawing.Point(550, 320);
            btnThemSP.Size = new System.Drawing.Size(120, 35);
            btnThemSP.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            btnThemSP.ForeColor = System.Drawing.Color.White;
            btnThemSP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnThemSP.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);
            btnThemSP.Click += BtnThemSP_Click;

            btnSuaSP = new System.Windows.Forms.Button();
            btnSuaSP.Text = "Sửa";
            btnSuaSP.Location = new System.Drawing.Point(680, 320);
            btnSuaSP.Size = new System.Drawing.Size(80, 35);
            btnSuaSP.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            btnSuaSP.ForeColor = System.Drawing.Color.White;
            btnSuaSP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSuaSP.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);
            btnSuaSP.Click += BtnSuaSP_Click;

            btnXoaSP = new System.Windows.Forms.Button();
            btnXoaSP.Text = "Xóa";
            btnXoaSP.Location = new System.Drawing.Point(770, 320);
            btnXoaSP.Size = new System.Drawing.Size(80, 35);
            btnXoaSP.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            btnXoaSP.ForeColor = System.Drawing.Color.White;
            btnXoaSP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnXoaSP.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);
            btnXoaSP.Click += BtnXoaSP_Click;

            pnlChiTiet.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblChiTiet, dgvChiTiet, btnThemSP, btnSuaSP, btnXoaSP
            });

            // 
            // Panel Tong Tien
            // 
            pnlTongTien = new System.Windows.Forms.Panel();
            pnlTongTien.Location = new System.Drawing.Point(20, 580);
            pnlTongTien.Size = new System.Drawing.Size(960, 50);
            pnlTongTien.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            lblTongTien = new System.Windows.Forms.Label();
            lblTongTien.Text = "Tổng tiền:";
            lblTongTien.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            lblTongTien.Location = new System.Drawing.Point(630, 13);
            lblTongTien.AutoSize = true;

            txtTongTien = new System.Windows.Forms.TextBox();
            txtTongTien.Location = new System.Drawing.Point(740, 10);
            txtTongTien.Size = new System.Drawing.Size(200, 30);
            txtTongTien.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            txtTongTien.ReadOnly = true;
            txtTongTien.BackColor = System.Drawing.Color.LightYellow;
            txtTongTien.ForeColor = System.Drawing.Color.Red;
            txtTongTien.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            pnlTongTien.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblTongTien, txtTongTien
            });

            // 
            // Panel Buttons
            // 
            pnlButtons = new System.Windows.Forms.Panel();
            pnlButtons.Location = new System.Drawing.Point(20, 640);
            pnlButtons.Size = new System.Drawing.Size(960, 50);

            btnLuu = new System.Windows.Forms.Button();
            btnLuu.Text = "Lưu";
            btnLuu.Location = new System.Drawing.Point(720, 8);
            btnLuu.Size = new System.Drawing.Size(100, 35);
            btnLuu.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            btnLuu.ForeColor = System.Drawing.Color.White;
            btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnLuu.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            btnLuu.Click += BtnLuu_Click;

            btnHuy = new System.Windows.Forms.Button();
            btnHuy.Text = "Hủy";
            btnHuy.Location = new System.Drawing.Point(840, 8);
            btnHuy.Size = new System.Drawing.Size(100, 35);
            btnHuy.BackColor = System.Drawing.Color.Gray;
            btnHuy.ForeColor = System.Drawing.Color.White;
            btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnHuy.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            btnHuy.Click += BtnHuy_Click;

            pnlButtons.Controls.AddRange(new System.Windows.Forms.Control[] {
                btnLuu, btnHuy
            });

            // 
            // Add to form
            // 
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblTitle, pnlInfo, pnlChiTiet, pnlTongTien, pnlButtons
            });
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblMaPhieu;
        private System.Windows.Forms.TextBox txtMaPhieu;
        private System.Windows.Forms.Label lblNhaCungCap;
        private System.Windows.Forms.ComboBox cboNhaCungCap;
        private System.Windows.Forms.Label lblNhanVien;
        private System.Windows.Forms.ComboBox cboNhanVien;
        private System.Windows.Forms.Label lblThoiGian;
        private System.Windows.Forms.DateTimePicker dtpThoiGian;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.TextBox txtTrangThai;
        private System.Windows.Forms.Panel pnlChiTiet;
        private System.Windows.Forms.Label lblChiTiet;
        private System.Windows.Forms.DataGridView dgvChiTiet;
        private System.Windows.Forms.Button btnThemSP;
        private System.Windows.Forms.Button btnXoaSP;
        private System.Windows.Forms.Button btnSuaSP;
        private System.Windows.Forms.Panel pnlTongTien;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.TextBox txtTongTien;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
    }
}
