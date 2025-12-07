using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.NghiepVu
{
    partial class ChiTietPhieuKiemKeDialog
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Label lblMaPhieu;
        private TextBox txtMaPhieu;
        private Label lblNhanVien;
        private ComboBox cboNhanVien;
        private Label lblThoiGian;
        private DateTimePicker dtpThoiGian;
        private Label lblTrangThai;
        private TextBox txtTrangThai;
        private DataGridView dgvChiTiet;
        private Button btnThemSP;
        private Button btnXoaSP;
        private Label lblTongSP;
        private Label lblThongKe;
        private Button btnLuu;
        private Button btnHuy;
        private Button btnXuatPDF;

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
            this.lblTitle = new Label();
            this.lblMaPhieu = new Label();
            this.txtMaPhieu = new TextBox();
            this.lblNhanVien = new Label();
            this.cboNhanVien = new ComboBox();
            this.lblThoiGian = new Label();
            this.dtpThoiGian = new DateTimePicker();
            this.lblTrangThai = new Label();
            this.txtTrangThai = new TextBox();
            this.dgvChiTiet = new DataGridView();
            this.btnThemSP = new Button();
            this.btnXoaSP = new Button();
            this.lblTongSP = new Label();
            this.lblThongKe = new Label();
            this.btnLuu = new Button();
            this.btnHuy = new Button();
            this.btnXuatPDF = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = false;
            this.lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(0, 122, 204);
            this.lblTitle.Location = new Point(20, 15);
            this.lblTitle.Size = new Size(1060, 30);
            this.lblTitle.Text = "CHI TIẾT PHIẾU KIỂM KÊ";
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // lblMaPhieu
            this.lblMaPhieu.AutoSize = true;
            this.lblMaPhieu.Font = new Font("Segoe UI", 10F);
            this.lblMaPhieu.Location = new Point(20, 60);
            this.lblMaPhieu.Size = new Size(100, 19);
            this.lblMaPhieu.Text = "Mã phiếu:";

            // txtMaPhieu
            this.txtMaPhieu.Enabled = false;
            this.txtMaPhieu.Font = new Font("Segoe UI", 10F);
            this.txtMaPhieu.Location = new Point(120, 57);
            this.txtMaPhieu.Size = new Size(200, 25);
            this.txtMaPhieu.BackColor = Color.LightGray;

            // lblNhanVien
            this.lblNhanVien.AutoSize = true;
            this.lblNhanVien.Font = new Font("Segoe UI", 10F);
            this.lblNhanVien.Location = new Point(340, 60);
            this.lblNhanVien.Size = new Size(100, 19);
            this.lblNhanVien.Text = "Nhân viên:";

            // cboNhanVien
            this.cboNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboNhanVien.Font = new Font("Segoe UI", 10F);
            this.cboNhanVien.Location = new Point(440, 57);
            this.cboNhanVien.Size = new Size(250, 25);

            // lblThoiGian
            this.lblThoiGian.AutoSize = true;
            this.lblThoiGian.Font = new Font("Segoe UI", 10F);
            this.lblThoiGian.Location = new Point(710, 60);
            this.lblThoiGian.Size = new Size(100, 19);
            this.lblThoiGian.Text = "Thời gian:";

            // dtpThoiGian
            this.dtpThoiGian.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpThoiGian.Font = new Font("Segoe UI", 10F);
            this.dtpThoiGian.Format = DateTimePickerFormat.Custom;
            this.dtpThoiGian.Location = new Point(810, 57);
            this.dtpThoiGian.Size = new Size(180, 25);

            // lblTrangThai
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new Font("Segoe UI", 10F);
            this.lblTrangThai.Location = new Point(20, 100);
            this.lblTrangThai.Size = new Size(100, 19);
            this.lblTrangThai.Text = "Trạng thái:";

            // txtTrangThai
            this.txtTrangThai.Enabled = false;
            this.txtTrangThai.Font = new Font("Segoe UI", 10F);
            this.txtTrangThai.Location = new Point(120, 97);
            this.txtTrangThai.Size = new Size(200, 25);
            this.txtTrangThai.BackColor = Color.LightGray;

            // dgvChiTiet
            this.dgvChiTiet.AllowUserToAddRows = false;
            this.dgvChiTiet.AllowUserToDeleteRows = false;
            this.dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvChiTiet.BackgroundColor = Color.White;
            this.dgvChiTiet.ColumnHeadersHeight = 40;
            this.dgvChiTiet.Location = new Point(20, 140);
            this.dgvChiTiet.SelectionMode = DataGridViewSelectionMode.CellSelect;  // ✅ Cell select for easy editing
            this.dgvChiTiet.MultiSelect = false;
            this.dgvChiTiet.EditMode = DataGridViewEditMode.EditOnEnter;  // ✅ Single click to edit
            this.dgvChiTiet.Size = new Size(1060, 350);  // Reduced height to 350
            this.dgvChiTiet.CellEndEdit += DgvChiTiet_CellEndEdit;
            this.dgvChiTiet.CellEnter += DgvChiTiet_CellEnter;  // ✅ Tooltip on hover

            // btnThemSP
            this.btnThemSP.BackColor = Color.FromArgb(46, 125, 50);
            this.btnThemSP.FlatStyle = FlatStyle.Flat;
            this.btnThemSP.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnThemSP.ForeColor = Color.White;
            this.btnThemSP.Location = new Point(20, 505);  // Moved up
            this.btnThemSP.Size = new Size(150, 35);
            this.btnThemSP.Text = "+ Thêm SP";
            this.btnThemSP.UseVisualStyleBackColor = false;
            this.btnThemSP.Click += BtnThemSP_Click;

            // btnXoaSP
            this.btnXoaSP.BackColor = Color.FromArgb(211, 47, 47);
            this.btnXoaSP.FlatStyle = FlatStyle.Flat;
            this.btnXoaSP.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnXoaSP.ForeColor = Color.White;
            this.btnXoaSP.Location = new Point(180, 505);  // Moved up
            this.btnXoaSP.Size = new Size(150, 35);
            this.btnXoaSP.Text = "- Xóa SP";
            this.btnXoaSP.UseVisualStyleBackColor = false;
            this.btnXoaSP.Click += BtnXoaSP_Click;

            // lblTongSP
            this.lblTongSP.AutoSize = false;
            this.lblTongSP.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTongSP.ForeColor = Color.FromArgb(0, 122, 204);
            this.lblTongSP.Location = new Point(350, 505);  // Moved up
            this.lblTongSP.Size = new Size(150, 35);
            this.lblTongSP.Text = "Tổng SP: 0";
            this.lblTongSP.TextAlign = ContentAlignment.MiddleLeft;

            // lblThongKe
            this.lblThongKe.AutoSize = false;
            this.lblThongKe.Font = new Font("Segoe UI", 9F);
            this.lblThongKe.ForeColor = Color.Black;
            this.lblThongKe.Location = new Point(510, 505);  // Moved up and adjusted position
            this.lblThongKe.Size = new Size(280, 35);
            this.lblThongKe.Text = "Thiếu: 0 SP (0 đ) | Thừa: 0 SP (0 đ)";
            this.lblThongKe.TextAlign = ContentAlignment.MiddleLeft;

            // btnXuatPDF
            this.btnXuatPDF.BackColor = Color.FromArgb(255, 152, 0);
            this.btnXuatPDF.FlatStyle = FlatStyle.Flat;
            this.btnXuatPDF.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnXuatPDF.ForeColor = Color.White;
            this.btnXuatPDF.Location = new Point(800, 505);  // Moved up
            this.btnXuatPDF.Size = new Size(110, 35);
            this.btnXuatPDF.Text = "📄 PDF";
            this.btnXuatPDF.UseVisualStyleBackColor = false;
            this.btnXuatPDF.Click += BtnXuatPDF_Click;

            // btnLuu
            this.btnLuu.BackColor = Color.FromArgb(0, 122, 204);
            this.btnLuu.FlatStyle = FlatStyle.Flat;
            this.btnLuu.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnLuu.ForeColor = Color.White;
            this.btnLuu.Location = new Point(920, 505);  // Moved up
            this.btnLuu.Size = new Size(70, 35);
            this.btnLuu.Text = "💾 Lưu";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Click += BtnLuu_Click;

            // btnHuy
            this.btnHuy.BackColor = Color.FromArgb(158, 158, 158);
            this.btnHuy.FlatStyle = FlatStyle.Flat;
            this.btnHuy.Font = new Font("Segoe UI", 10F);
            this.btnHuy.ForeColor = Color.White;
            this.btnHuy.Location = new Point(1000, 505);  // Moved up
            this.btnHuy.Size = new Size(80, 35);
            this.btnHuy.Text = "✗ Hủy";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Click += BtnHuy_Click;
            this.btnHuy.Location = new Point(1015, 555);
            this.btnHuy.Size = new Size(65, 35);
            this.btnHuy.Text = "✗ Hủy";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Click += BtnHuy_Click;

            // ChiTietPhieuKiemKeDialog
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1100, 610);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblMaPhieu);
            this.Controls.Add(this.txtMaPhieu);
            this.Controls.Add(this.lblNhanVien);
            this.Controls.Add(this.cboNhanVien);
            this.Controls.Add(this.lblThoiGian);
            this.Controls.Add(this.dtpThoiGian);
            this.Controls.Add(this.lblTrangThai);
            this.Controls.Add(this.txtTrangThai);
            this.Controls.Add(this.dgvChiTiet);
            this.Controls.Add(this.btnThemSP);
            this.Controls.Add(this.btnXoaSP);
            this.Controls.Add(this.lblTongSP);
            this.Controls.Add(this.lblThongKe);
            this.Controls.Add(this.btnXuatPDF);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnHuy);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChiTietPhieuKiemKeDialog";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Chi tiết phiếu kiểm kê";
            this.ClientSize = new Size(1100, 560);  // Adjusted height to fit all controls
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
