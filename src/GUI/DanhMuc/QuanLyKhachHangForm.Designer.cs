namespace src.GUI.DanhMuc
{
    partial class QuanLyKhachHangForm
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
            this.SuspendLayout();
            
            // 
            // Form Configuration
            // 
            this.Text = "Quản lý Khách hàng";
            this.Size = new System.Drawing.Size(1350, 700);
            this.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
            this.Padding = new Padding(0);
            this.StartPosition = FormStartPosition.CenterScreen;

            // 
            // Header Label
            // 
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ KHÁCH HÀNG",
                Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(41, 128, 185),
                Location = new System.Drawing.Point(20, 15),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // 
            // Search Panel
            // 
            Panel searchPanel = CreateSearchPanel();
            searchPanel.Location = new System.Drawing.Point(20, 70);
            this.Controls.Add(searchPanel);

            // 
            // DataGridView
            // 
            dgvKhachHang = new DataGridView
            {
                Location = new System.Drawing.Point(20, 130),
                Size = new System.Drawing.Size(850, 400),
                BackgroundColor = System.Drawing.Color.White,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false,
                RowHeadersVisible = false,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 40,
                EnableHeadersVisualStyles = false
            };
            dgvKhachHang.SelectionChanged += DgvKhachHang_SelectionChanged;
            
            // Configure DataGridView Header Style
            dgvKhachHang.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvKhachHang.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvKhachHang.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgvKhachHang.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            this.Controls.Add(dgvKhachHang);

            // 
            // Form Panel
            // 
            Panel formPanel = CreateFormPanel();
            formPanel.Location = new System.Drawing.Point(890, 130);
            this.Controls.Add(formPanel);

            // 
            // Button Panel
            // 
            Panel buttonPanel = CreateButtonPanel();
            buttonPanel.Location = new System.Drawing.Point(20, 550);
            this.Controls.Add(buttonPanel);

            this.ResumeLayout(false);
        }

        #endregion

        private Panel CreateSearchPanel()
        {
            Panel panel = new Panel
            {
                Size = new System.Drawing.Size(1300, 45),
                BackColor = System.Drawing.Color.White,
                Padding = new Padding(8)
            };

            cboTimKiem = new ComboBox
            {
                Location = new System.Drawing.Point(10, 12),
                Size = new System.Drawing.Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboTimKiem.Items.AddRange(new object[] { "Tất cả", "Mã KH", "Họ tên", "Số điện thoại", "Email" });
            cboTimKiem.SelectedIndex = 0;
            panel.Controls.Add(cboTimKiem);

            txtTimKiem = new TextBox
            {
                Location = new System.Drawing.Point(170, 12),
                Size = new System.Drawing.Size(300, 25),
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtTimKiem);

            btnTimKiem = new Button
            {
                Text = "Tìm kiếm",
                Location = new System.Drawing.Point(480, 10),
                Size = new System.Drawing.Size(100, 30),
                BackColor = System.Drawing.Color.FromArgb(52, 152, 219),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnTimKiem.FlatAppearance.BorderSize = 0;
            btnTimKiem.Click += BtnTimKiem_Click;
            panel.Controls.Add(btnTimKiem);

            btnRefresh = new Button
            {
                Text = "Làm mới",
                Location = new System.Drawing.Point(590, 10),
                Size = new System.Drawing.Size(100, 30),
                BackColor = System.Drawing.Color.FromArgb(149, 165, 166),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;
            panel.Controls.Add(btnRefresh);

            btnExport = new Button
            {
                Text = "Xuất Excel",
                Location = new System.Drawing.Point(700, 10),
                Size = new System.Drawing.Size(100, 30),
                BackColor = System.Drawing.Color.FromArgb(39, 174, 96),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += BtnExport_Click;
            panel.Controls.Add(btnExport);

            return panel;
        }

        private Panel CreateFormPanel()
        {
            Panel panel = new Panel
            {
                Size = new System.Drawing.Size(450, 450),
                BackColor = System.Drawing.Color.White,
                Padding = new Padding(20)
            };

            Label lblFormTitle = new Label
            {
                Text = "THÔNG TIN KHÁCH HÀNG",
                Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(41, 128, 185),
                Location = new System.Drawing.Point(20, 15),
                AutoSize = true
            };
            panel.Controls.Add(lblFormTitle);

            int yPos = 60;

            // Mã KH
            Label lblMaKH = new Label
            {
                Text = "Mã KH:",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(120, 25),
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblMaKH);

            txtMaKH = new TextBox
            {
                Location = new System.Drawing.Point(140, yPos),
                Size = new System.Drawing.Size(280, 25),
                ReadOnly = true,
                BackColor = System.Drawing.Color.FromArgb(236, 240, 241)
            };
            panel.Controls.Add(txtMaKH);
            yPos += 40;

            // Họ tên
            Label lblHoTen = new Label
            {
                Text = "Họ tên: *",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(120, 25),
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblHoTen);

            txtHoTen = new TextBox
            {
                Location = new System.Drawing.Point(140, yPos),
                Size = new System.Drawing.Size(280, 25),
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtHoTen);
            yPos += 40;

            // Địa chỉ
            Label lblDiaChi = new Label
            {
                Text = "Địa chỉ: *",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(120, 25),
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblDiaChi);

            txtDiaChi = new TextBox
            {
                Location = new System.Drawing.Point(140, yPos),
                Size = new System.Drawing.Size(280, 25),
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtDiaChi);
            yPos += 40;

            // Số điện thoại
            Label lblSDT = new Label
            {
                Text = "Số điện thoại: *",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(120, 25),
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblSDT);

            txtSDT = new TextBox
            {
                Location = new System.Drawing.Point(140, yPos),
                Size = new System.Drawing.Size(280, 25),
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtSDT);
            yPos += 40;

            // Email
            Label lblEmail = new Label
            {
                Text = "Email: *",
                Location = new System.Drawing.Point(20, yPos),
                Size = new System.Drawing.Size(120, 25),
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            panel.Controls.Add(lblEmail);

            txtEmail = new TextBox
            {
                Location = new System.Drawing.Point(140, yPos),
                Size = new System.Drawing.Size(280, 25),
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            panel.Controls.Add(txtEmail);

            return panel;
        }

        private Panel CreateButtonPanel()
        {
            Panel panel = new Panel
            {
                Size = new System.Drawing.Size(1300, 50),
                BackColor = System.Drawing.Color.Transparent
            };

            int xPos = 0;
            int btnWidth = 100;
            int btnHeight = 35;
            int spacing = 15;

            btnThem = new Button
            {
                Text = "➕ Thêm",
                Location = new System.Drawing.Point(xPos, 10),
                Size = new System.Drawing.Size(btnWidth, btnHeight),
                BackColor = System.Drawing.Color.FromArgb(39, 174, 96),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold)
            };
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.Click += BtnThem_Click;
            panel.Controls.Add(btnThem);
            xPos += btnWidth + spacing;

            btnSua = new Button
            {
                Text = "✏️ Sửa",
                Location = new System.Drawing.Point(xPos, 10),
                Size = new System.Drawing.Size(btnWidth, btnHeight),
                BackColor = System.Drawing.Color.FromArgb(241, 196, 15),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold)
            };
            btnSua.FlatAppearance.BorderSize = 0;
            btnSua.Click += BtnSua_Click;
            panel.Controls.Add(btnSua);
            xPos += btnWidth + spacing;

            btnXoa = new Button
            {
                Text = "🗑️ Xóa",
                Location = new System.Drawing.Point(xPos, 10),
                Size = new System.Drawing.Size(btnWidth, btnHeight),
                BackColor = System.Drawing.Color.FromArgb(231, 76, 60),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold)
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.Click += BtnXoa_Click;
            panel.Controls.Add(btnXoa);
            xPos += btnWidth + spacing;

            btnLuu = new Button
            {
                Text = "💾 Lưu",
                Location = new System.Drawing.Point(xPos, 10),
                Size = new System.Drawing.Size(btnWidth, btnHeight),
                BackColor = System.Drawing.Color.FromArgb(52, 152, 219),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Enabled = false
            };
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.Click += BtnLuu_Click;
            panel.Controls.Add(btnLuu);
            xPos += btnWidth + spacing;

            btnHuy = new Button
            {
                Text = "❌ Hủy",
                Location = new System.Drawing.Point(xPos, 10),
                Size = new System.Drawing.Size(btnWidth, btnHeight),
                BackColor = System.Drawing.Color.FromArgb(149, 165, 166),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Enabled = false
            };
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.Click += BtnHuy_Click;
            panel.Controls.Add(btnHuy);

            return panel;
        }
    }
}
