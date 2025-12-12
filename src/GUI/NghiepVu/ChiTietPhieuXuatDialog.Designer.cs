using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.NghiepVu
{
    partial class ChiTietPhieuXuatDialog
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Text = "Chi tiết Phiếu Xuất";
            this.Size = new System.Drawing.Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Initialize controls
            lblTitle = new Label();
            pnlInfo = new Panel();
            lblMaPhieu = new Label();
            txtMaPhieu = new TextBox();
            lblKhachHang = new Label();
            cboKhachHang = new ComboBox();
            lblNhanVien = new Label();
            cboNhanVien = new ComboBox();
            lblThoiGian = new Label();
            dtpThoiGian = new DateTimePicker();
            lblTrangThai = new Label();
            txtTrangThai = new TextBox();
            pnlChiTiet = new Panel();
            lblChiTiet = new Label();
            dgvChiTiet = new DataGridView();
            btnThemSP = new Button();
            btnXoaSP = new Button();
            btnSuaSP = new Button();
            pnlTongTien = new Panel();
            lblTongTien = new Label();
            txtTongTien = new TextBox();
            pnlButtons = new Panel();
            btnLuu = new Button();
            btnHuy = new Button();

            ((ISupportInitialize)(dgvChiTiet)).BeginInit();
            pnlInfo.SuspendLayout();
            pnlChiTiet.SuspendLayout();
            pnlTongTien.SuspendLayout();
            pnlButtons.SuspendLayout();
            SuspendLayout();

            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(220, 53, 69);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Size = new Size(960, 40);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // pnlInfo
            // 
            pnlInfo.BorderStyle = BorderStyle.FixedSingle;
            pnlInfo.Controls.Add(lblMaPhieu);
            pnlInfo.Controls.Add(txtMaPhieu);
            pnlInfo.Controls.Add(lblKhachHang);
            pnlInfo.Controls.Add(cboKhachHang);
            pnlInfo.Controls.Add(lblNhanVien);
            pnlInfo.Controls.Add(cboNhanVien);
            pnlInfo.Controls.Add(lblThoiGian);
            pnlInfo.Controls.Add(dtpThoiGian);
            pnlInfo.Controls.Add(lblTrangThai);
            pnlInfo.Controls.Add(txtTrangThai);
            pnlInfo.Location = new Point(20, 70);
            pnlInfo.Size = new Size(960, 120);

            // 
            // lblMaPhieu
            // 
            lblMaPhieu.AutoSize = true;
            lblMaPhieu.Location = new Point(20, 20);
            lblMaPhieu.Text = "Mã phiếu:";

            // 
            // txtMaPhieu
            // 
            txtMaPhieu.BackColor = Color.LightGray;
            txtMaPhieu.Location = new Point(150, 17);
            txtMaPhieu.ReadOnly = true;
            txtMaPhieu.Size = new Size(150, 25);

            // 
            // lblKhachHang
            // 
            lblKhachHang.AutoSize = true;
            lblKhachHang.Location = new Point(330, 20);
            lblKhachHang.Text = "Khách hàng:";

            // 
            // cboKhachHang
            // 
            cboKhachHang.DropDownStyle = ComboBoxStyle.DropDownList;
            cboKhachHang.Location = new Point(460, 17);
            cboKhachHang.Size = new Size(250, 25);

            // 
            // lblNhanVien
            // 
            lblNhanVien.AutoSize = true;
            lblNhanVien.Location = new Point(20, 60);
            lblNhanVien.Text = "Nhân viên:";

            // 
            // cboNhanVien
            // 
            cboNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNhanVien.Location = new Point(150, 57);
            cboNhanVien.Size = new Size(250, 25);

            // 
            // lblThoiGian
            // 
            lblThoiGian.AutoSize = true;
            lblThoiGian.Location = new Point(430, 60);
            lblThoiGian.Text = "Thời gian:";

            // 
            // dtpThoiGian
            // 
            dtpThoiGian.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpThoiGian.Format = DateTimePickerFormat.Custom;
            dtpThoiGian.Location = new Point(530, 57);
            dtpThoiGian.Size = new Size(180, 25);

            // 
            // lblTrangThai
            // 
            lblTrangThai.AutoSize = true;
            lblTrangThai.Location = new Point(730, 60);
            lblTrangThai.Text = "Trạng thái:";

            // 
            // txtTrangThai
            // 
            txtTrangThai.BackColor = Color.LightGray;
            txtTrangThai.Location = new Point(830, 57);
            txtTrangThai.ReadOnly = true;
            txtTrangThai.Size = new Size(100, 25);

            // 
            // pnlChiTiet
            // 
            pnlChiTiet.BorderStyle = BorderStyle.FixedSingle;
            pnlChiTiet.Controls.Add(lblChiTiet);
            pnlChiTiet.Controls.Add(dgvChiTiet);
            pnlChiTiet.Controls.Add(btnThemSP);
            pnlChiTiet.Controls.Add(btnSuaSP);
            pnlChiTiet.Controls.Add(btnXoaSP);
            pnlChiTiet.Location = new Point(20, 200);
            pnlChiTiet.Size = new Size(960, 350);

            // 
            // lblChiTiet
            // 
            lblChiTiet.AutoSize = true;
            lblChiTiet.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblChiTiet.Location = new Point(10, 10);
            lblChiTiet.Text = "Danh sách sản phẩm";

            // 
            // dgvChiTiet
            // 
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.AllowUserToDeleteRows = false;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.BackgroundColor = Color.White;
            dgvChiTiet.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 53, 69);
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvChiTiet.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvChiTiet.EnableHeadersVisualStyles = false;
            dgvChiTiet.Location = new Point(10, 40);
            dgvChiTiet.MultiSelect = false;
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.RowHeadersVisible = false;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.Size = new Size(940, 250);

            // Add columns to dgvChiTiet
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MSP",
                HeaderText = "Mã SP",
                DataPropertyName = "MSP"
            });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenSP",
                HeaderText = "Tên sản phẩm",
                DataPropertyName = "TenSP"
            });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SL",
                HeaderText = "Số lượng",
                DataPropertyName = "SL"
            });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GIA",
                HeaderText = "Đơn giá",
                DataPropertyName = "GIA"
            });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThanhTien",
                HeaderText = "Thành tiền",
                DataPropertyName = "ThanhTien"
            });

            dgvChiTiet.Columns["SL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvChiTiet.Columns["GIA"].DefaultCellStyle.Format = "N0";
            dgvChiTiet.Columns["GIA"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // 
            // btnThemSP
            // 
            btnThemSP.BackColor = Color.FromArgb(40, 167, 69);
            btnThemSP.FlatStyle = FlatStyle.Flat;
            btnThemSP.ForeColor = Color.White;
            btnThemSP.Location = new Point(10, 300);
            btnThemSP.Size = new Size(130, 35);
            btnThemSP.Text = "Thêm SP";
            btnThemSP.Click += BtnThemSP_Click;

            // 
            // btnSuaSP
            // 
            btnSuaSP.BackColor = Color.FromArgb(255, 193, 7);
            btnSuaSP.FlatStyle = FlatStyle.Flat;
            btnSuaSP.ForeColor = Color.White;
            btnSuaSP.Location = new Point(150, 300);
            btnSuaSP.Size = new Size(100, 35);
            btnSuaSP.Text = "Sửa";
            btnSuaSP.Click += BtnSuaSP_Click;

            // 
            // btnXoaSP
            // 
            btnXoaSP.BackColor = Color.FromArgb(220, 53, 69);
            btnXoaSP.FlatStyle = FlatStyle.Flat;
            btnXoaSP.ForeColor = Color.White;
            btnXoaSP.Location = new Point(260, 300);
            btnXoaSP.Size = new Size(100, 35);
            btnXoaSP.Text = "Xóa";
            btnXoaSP.Click += BtnXoaSP_Click;

            // 
            // pnlTongTien
            // 
            pnlTongTien.Controls.Add(lblTongTien);
            pnlTongTien.Controls.Add(txtTongTien);
            pnlTongTien.Location = new Point(20, 560);
            pnlTongTien.Size = new Size(960, 50);

            // 
            // lblTongTien
            // 
            lblTongTien.AutoSize = true;
            lblTongTien.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTongTien.Location = new Point(600, 12);
            lblTongTien.Text = "TỔNG TIỀN:";

            // 
            // txtTongTien
            // 
            txtTongTien.BackColor = Color.White;
            txtTongTien.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            txtTongTien.ForeColor = Color.Red;
            txtTongTien.Location = new Point(740, 10);
            txtTongTien.ReadOnly = true;
            txtTongTien.Size = new Size(200, 30);
            txtTongTien.TextAlign = HorizontalAlignment.Right;

            // 
            // pnlButtons
            // 
            pnlButtons.Controls.Add(btnLuu);
            pnlButtons.Controls.Add(btnHuy);
            pnlButtons.Location = new Point(20, 620);
            pnlButtons.Size = new Size(960, 50);

            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.FromArgb(40, 167, 69);
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLuu.ForeColor = Color.White;
            btnLuu.Location = new Point(730, 5);
            btnLuu.Size = new Size(100, 40);
            btnLuu.Text = "Lưu";
            btnLuu.Click += BtnLuu_Click;

            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.Gray;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnHuy.ForeColor = Color.White;
            btnHuy.Location = new Point(840, 5);
            btnHuy.Size = new Size(100, 40);
            btnHuy.Text = "Hủy";
            btnHuy.Click += BtnHuy_Click;

            // 
            // ChiTietPhieuXuatDialog
            // 
            Controls.Add(lblTitle);
            Controls.Add(pnlInfo);
            Controls.Add(pnlChiTiet);
            Controls.Add(pnlTongTien);
            Controls.Add(pnlButtons);

            ((ISupportInitialize)(dgvChiTiet)).EndInit();
            pnlInfo.ResumeLayout(false);
            pnlInfo.PerformLayout();
            pnlChiTiet.ResumeLayout(false);
            pnlChiTiet.PerformLayout();
            pnlTongTien.ResumeLayout(false);
            pnlTongTien.PerformLayout();
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #region Windows Form Designer generated code

        private Label lblTitle;
        private Panel pnlInfo;
        private Label lblMaPhieu;
        private TextBox txtMaPhieu;
        private Label lblKhachHang;
        private ComboBox cboKhachHang;
        private Label lblNhanVien;
        private ComboBox cboNhanVien;
        private Label lblThoiGian;
        private DateTimePicker dtpThoiGian;
        private Label lblTrangThai;
        private TextBox txtTrangThai;
        private Panel pnlChiTiet;
        private Label lblChiTiet;
        private DataGridView dgvChiTiet;
        private Button btnThemSP;
        private Button btnXoaSP;
        private Button btnSuaSP;
        private Panel pnlTongTien;
        private Label lblTongTien;
        private TextBox txtTongTien;
        private Panel pnlButtons;
        private Button btnLuu;
        private Button btnHuy;

        #endregion
    }
}
