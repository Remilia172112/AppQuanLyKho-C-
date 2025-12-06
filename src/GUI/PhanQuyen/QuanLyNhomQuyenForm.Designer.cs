using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.PhanQuyen
{
    partial class QuanLyNhomQuyenForm
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
            this.Text = "Quản lý Phân quyền";
            this.Size = new Size(1400, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Main Layout: Split Container (Left: Danh sách | Right: Chi tiết quyền)
            mainSplit = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = (int)(this.Width * 0.45), // 45% chiều rộng form
                IsSplitterFixed = false // Cho phép kéo chia lại
            };

            // === LEFT PANEL: Danh sách nhóm quyền ===
            Panel leftPanel = new Panel { Dock = DockStyle.Fill };
            
            // Title
            Label lblTitle = new Label
            {
                Text = "DANH SÁCH NHÓM QUYỀN",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                AutoSize = true,
                Location = new Point(20, 15)
            };
            leftPanel.Controls.Add(lblTitle);

            // Search Panel
            Panel searchPanel = new Panel
            {
                Location = new Point(20, 55),
                Size = new Size(510, 40),
                BorderStyle = BorderStyle.FixedSingle
            };

            txtTimKiem = new TextBox
            {
                Location = new Point(10, 8),
                Size = new Size(380, 25),
                Font = new Font("Segoe UI", 10)
            };
            txtTimKiem.KeyDown += TxtTimKiem_KeyDown;

            btnTimKiem = new Button
            {
                Text = "Tìm kiếm",
                Location = new Point(400, 5),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnTimKiem.Click += BtnTimKiem_Click;

            searchPanel.Controls.AddRange(new Control[] { txtTimKiem, btnTimKiem });
            leftPanel.Controls.Add(searchPanel);

            // DataGridView
            dgvNhomQuyen = new DataGridView
            {
                Location = new Point(20, 105),
                Size = new Size(510, 350),
                AutoGenerateColumns = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            
            InitializeDataGridView();
            dgvNhomQuyen.SelectionChanged += DgvNhomQuyen_SelectionChanged;
            leftPanel.Controls.Add(dgvNhomQuyen);

            // Info Panel
            GroupBox infoGroup = new GroupBox
            {
                Text = "Thông tin nhóm quyền",
                Location = new Point(20, 465),
                Size = new Size(510, 100),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            Label lblMa = new Label
            {
                Text = "Mã nhóm quyền:",
                Location = new Point(15, 30),
                Size = new Size(120, 20)
            };

            txtMaNhomQuyen = new TextBox
            {
                Location = new Point(140, 27),
                Size = new Size(150, 25),
                ReadOnly = true,
                BackColor = Color.LightGray
            };

            Label lblTen = new Label
            {
                Text = "Tên nhóm quyền:",
                Location = new Point(15, 60),
                Size = new Size(120, 20)
            };

            txtTenNhomQuyen = new TextBox
            {
                Location = new Point(140, 57),
                Size = new Size(350, 25),
                Font = new Font("Segoe UI", 10)
            };

            infoGroup.Controls.AddRange(new Control[] { lblMa, txtMaNhomQuyen, lblTen, txtTenNhomQuyen });
            leftPanel.Controls.Add(infoGroup);

            // Button Panel
            Panel btnPanel = new Panel
            {
                Location = new Point(20, 575),
                Size = new Size(510, 50)
            };

            btnThem = CreateButton("Thêm", new Point(0, 0), Color.FromArgb(46, 204, 113));
            btnThem.Click += BtnThem_Click;

            btnSua = CreateButton("Sửa", new Point(105, 0), Color.FromArgb(52, 152, 219));
            btnSua.Click += BtnSua_Click;

            btnXoa = CreateButton("Xóa", new Point(210, 0), Color.FromArgb(231, 76, 60));
            btnXoa.Click += BtnXoa_Click;

            btnLuu = CreateButton("Lưu", new Point(315, 0), Color.FromArgb(26, 188, 156));
            btnLuu.Click += BtnLuu_Click;

            btnHuy = CreateButton("Hủy", new Point(420, 0), Color.FromArgb(149, 165, 166));
            btnHuy.Click += BtnHuy_Click;

            btnPanel.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLuu, btnHuy });
            leftPanel.Controls.Add(btnPanel);

            btnRefresh = new Button
            {
                Text = "Làm mới",
                Location = new Point(20, 635),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            btnRefresh.Click += BtnRefresh_Click;
            leftPanel.Controls.Add(btnRefresh);

            mainSplit.Panel1.Controls.Add(leftPanel);

            // === RIGHT PANEL: Phân quyền chi tiết ===
            Panel rightPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            
            Label lblQuyenTitle = new Label
            {
                Text = "PHÂN QUYỀN CHI TIẾT",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                AutoSize = true,
                Location = new Point(20, 15)
            };
            rightPanel.Controls.Add(lblQuyenTitle);

            pnlChucNang = new Panel
            {
                Location = new Point(20, 55),
                Size = new Size(630, 615), // Giảm chiều ngang panel phải
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.WhiteSmoke
            };
            
            InitializePermissionPanel();
            rightPanel.Controls.Add(pnlChucNang);

            mainSplit.Panel2.Controls.Add(rightPanel);
            this.Controls.Add(mainSplit);
        }

        private Button CreateButton(string text, Point location, Color color)
        {
            return new Button
            {
                Text = text,
                Location = location,
                Size = new Size(100, 40),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        #endregion

        // UI Controls - Left Panel (Danh sách nhóm quyền)
        private DataGridView dgvNhomQuyen;
        private TextBox txtMaNhomQuyen;
        private TextBox txtTenNhomQuyen;
        private TextBox txtTimKiem;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLuu;
        private Button btnHuy;
        private Button btnTimKiem;
        private Button btnRefresh;
        
        // UI Controls - Right Panel (Phân quyền chi tiết)
        private Panel pnlChucNang;
        private Dictionary<string, Dictionary<string, CheckBox>> checkBoxQuyen; // MCN -> (Action -> CheckBox)
        
        private SplitContainer mainSplit; // Lưu lại để xử lý resize
    }
}
