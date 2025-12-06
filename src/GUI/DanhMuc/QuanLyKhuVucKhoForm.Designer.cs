using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.DanhMuc
{
    partial class QuanLyKhuVucKhoForm
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
            SuspendLayout();

            this.Text = "Quản lý Khu vực kho";
            this.Size = new Size(1400, 750);
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.Padding = new Padding(0);

            // Initialize controls
            dgvKhuVucKho = new DataGridView();
            txtMaKV = new TextBox();
            txtTenKV = new TextBox();
            txtGhiChu = new TextBox();
            txtTimKiem = new TextBox();
            cboTimKiem = new ComboBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLuu = new Button();
            btnHuy = new Button();
            btnTimKiem = new Button();
            btnRefresh = new Button();

            Label lblTitle = new Label();

            ((ISupportInitialize)(dgvKhuVucKho)).BeginInit();

            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.Location = new Point(30, 20);
            lblTitle.Text = "QUẢN LÝ KHU VỰC KHO";

            // 
            // dgvKhuVucKho
            // 
            dgvKhuVucKho.AllowUserToAddRows = false;
            dgvKhuVucKho.AutoGenerateColumns = false;
            dgvKhuVucKho.BackgroundColor = Color.White;
            dgvKhuVucKho.BorderStyle = BorderStyle.None;
            dgvKhuVucKho.ColumnHeadersHeight = 40;
            dgvKhuVucKho.EnableHeadersVisualStyles = false;
            dgvKhuVucKho.Location = new Point(30, 140);
            dgvKhuVucKho.ReadOnly = true;
            dgvKhuVucKho.RowHeadersVisible = false;
            dgvKhuVucKho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKhuVucKho.Size = new Size(750, 450);
            dgvKhuVucKho.SelectionChanged += DgvKhuVucKho_SelectionChanged;

            // Create panels
            Panel searchPanel = CreateSearchPanel();
            searchPanel.Location = new Point(30, 70);

            Panel formPanel = CreateFormPanel();
            formPanel.Location = new Point(800, 140);

            Panel buttonPanel = CreateButtonPanel();
            buttonPanel.Location = new Point(30, 610);

            // 
            // QuanLyKhuVucKhoForm
            // 
            Controls.Add(lblTitle);
            Controls.Add(searchPanel);
            Controls.Add(dgvKhuVucKho);
            Controls.Add(formPanel);
            Controls.Add(buttonPanel);

            ((ISupportInitialize)(dgvKhuVucKho)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel CreateSearchPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(1300, 50),
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            // 
            // cboTimKiem
            // 
            cboTimKiem.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTimKiem.Location = new Point(10, 12);
            cboTimKiem.Size = new Size(150, 25);
            cboTimKiem.Items.AddRange(new object[] { "Tất cả", "Mã KV", "Tên khu vực" });
            cboTimKiem.SelectedIndex = 0;

            // 
            // txtTimKiem
            // 
            txtTimKiem.Font = new Font("Segoe UI", 10F);
            txtTimKiem.Location = new Point(170, 12);
            txtTimKiem.Size = new Size(300, 25);

            // 
            // btnTimKiem
            // 
            btnTimKiem.BackColor = Color.FromArgb(52, 152, 219);
            btnTimKiem.Cursor = Cursors.Hand;
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.Location = new Point(480, 10);
            btnTimKiem.Size = new Size(100, 30);
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.FlatAppearance.BorderSize = 0;
            btnTimKiem.Click += BtnTimKiem_Click;

            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(149, 165, 166);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(590, 10);
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.Text = "Làm mới";
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;

            panel.Controls.Add(cboTimKiem);
            panel.Controls.Add(txtTimKiem);
            panel.Controls.Add(btnTimKiem);
            panel.Controls.Add(btnRefresh);

            return panel;
        }

        private Panel CreateFormPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(550, 450),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblFormTitle = new Label
            {
                Text = "THÔNG TIN KHU VỰC KHO",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(20, 15),
                AutoSize = true
            };
            panel.Controls.Add(lblFormTitle);

            int yPos = 60;

            // Mã KV
            Label lblMaKV = new Label
            {
                Text = "Mã khu vực:",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtMaKV.BackColor = Color.FromArgb(236, 240, 241);
            txtMaKV.Location = new Point(140, yPos);
            txtMaKV.ReadOnly = true;
            txtMaKV.Size = new Size(380, 25);
            panel.Controls.Add(lblMaKV);
            panel.Controls.Add(txtMaKV);
            yPos += 40;

            // Tên KV
            Label lblTenKV = new Label
            {
                Text = "Tên khu vực: *",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtTenKV.Font = new Font("Segoe UI", 10F);
            txtTenKV.Location = new Point(140, yPos);
            txtTenKV.Size = new Size(380, 25);
            panel.Controls.Add(lblTenKV);
            panel.Controls.Add(txtTenKV);
            yPos += 40;

            // Ghi chú
            Label lblGhiChu = new Label
            {
                Text = "Ghi chú:",
                Location = new Point(20, yPos),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtGhiChu.Font = new Font("Segoe UI", 10F);
            txtGhiChu.Location = new Point(140, yPos);
            txtGhiChu.Multiline = true;
            txtGhiChu.Size = new Size(380, 80);
            panel.Controls.Add(lblGhiChu);
            panel.Controls.Add(txtGhiChu);

            return panel;
        }

        private Panel CreateButtonPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(1300, 50),
                BackColor = Color.Transparent
            };

            int xPos = 0;
            int btnWidth = 100;
            int btnHeight = 35;
            int spacing = 15;

            // 
            // btnThem
            // 
            btnThem.BackColor = Color.FromArgb(39, 174, 96);
            btnThem.Cursor = Cursors.Hand;
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(xPos, 10);
            btnThem.Size = new Size(btnWidth, btnHeight);
            btnThem.Text = "Thêm";
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.Click += BtnThem_Click;
            panel.Controls.Add(btnThem);
            xPos += btnWidth + spacing;

            // 
            // btnSua
            // 
            btnSua.BackColor = Color.FromArgb(241, 196, 15);
            btnSua.Cursor = Cursors.Hand;
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSua.ForeColor = Color.White;
            btnSua.Location = new Point(xPos, 10);
            btnSua.Size = new Size(btnWidth, btnHeight);
            btnSua.Text = "Sửa";
            btnSua.FlatAppearance.BorderSize = 0;
            btnSua.Click += BtnSua_Click;
            panel.Controls.Add(btnSua);
            xPos += btnWidth + spacing;

            // 
            // btnXoa
            // 
            btnXoa.BackColor = Color.FromArgb(231, 76, 60);
            btnXoa.Cursor = Cursors.Hand;
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXoa.ForeColor = Color.White;
            btnXoa.Location = new Point(xPos, 10);
            btnXoa.Size = new Size(btnWidth, btnHeight);
            btnXoa.Text = "Xóa";
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.Click += BtnXoa_Click;
            panel.Controls.Add(btnXoa);
            xPos += btnWidth + spacing;

            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.FromArgb(52, 152, 219);
            btnLuu.Cursor = Cursors.Hand;
            btnLuu.Enabled = false;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLuu.ForeColor = Color.White;
            btnLuu.Location = new Point(xPos, 10);
            btnLuu.Size = new Size(btnWidth, btnHeight);
            btnLuu.Text = "Lưu";
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.Click += BtnLuu_Click;
            panel.Controls.Add(btnLuu);
            xPos += btnWidth + spacing;

            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.FromArgb(149, 165, 166);
            btnHuy.Cursor = Cursors.Hand;
            btnHuy.Enabled = false;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnHuy.ForeColor = Color.White;
            btnHuy.Location = new Point(xPos, 10);
            btnHuy.Size = new Size(btnWidth, btnHeight);
            btnHuy.Text = "Hủy";
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.Click += BtnHuy_Click;
            panel.Controls.Add(btnHuy);

            return panel;
        }

        #region Windows Form Designer generated code

        private DataGridView dgvKhuVucKho;
        private TextBox txtMaKV;
        private TextBox txtTenKV;
        private TextBox txtGhiChu;
        private TextBox txtTimKiem;
        private ComboBox cboTimKiem;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLuu;
        private Button btnHuy;
        private Button btnTimKiem;
        private Button btnRefresh;

        #endregion
    }
}
