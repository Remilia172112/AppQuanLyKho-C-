namespace src.GUI.NghiepVu
{
    partial class PhieuNhapForm
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
            this.Text = "Quản lý Phiếu Nhập hàng";
            this.Size = new System.Drawing.Size(1400, 800);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);

            // 
            // Panel Title
            // 
            pnlTop = new System.Windows.Forms.Panel
            {
                Dock = System.Windows.Forms.DockStyle.Top,
                Height = 60,
                BackColor = System.Drawing.Color.FromArgb(8, 133, 204)
            };

            lblTitle = new System.Windows.Forms.Label
            {
                Text = "QUẢN LÝ PHIẾU NHẬP HÀNG",
                Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Dock = System.Windows.Forms.DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            pnlTop.Controls.Add(lblTitle);

            // 
            // Panel Filter
            // 
            pnlFilter = new System.Windows.Forms.Panel
            {
                Dock = System.Windows.Forms.DockStyle.Top,
                Height = 120,
                Padding = new System.Windows.Forms.Padding(10),
                BackColor = System.Drawing.Color.White
            };

            // Row 1 - Search and ComboBoxes
            System.Windows.Forms.Label lblSearch = new System.Windows.Forms.Label
            {
                Text = "Tìm kiếm:",
                Location = new System.Drawing.Point(15, 15),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };
            pnlFilter.Controls.Add(lblSearch);

            txtSearch = new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(100, 12),
                Size = new System.Drawing.Size(200, 25),
                PlaceholderText = "Nhập mã phiếu..."
            };
            pnlFilter.Controls.Add(txtSearch);

            System.Windows.Forms.Label lblNCC = new System.Windows.Forms.Label
            {
                Text = "Nhà cung cấp:",
                Location = new System.Drawing.Point(320, 15),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };
            pnlFilter.Controls.Add(lblNCC);

            cboNhaCungCap = new System.Windows.Forms.ComboBox
            {
                Location = new System.Drawing.Point(425, 12),
                Size = new System.Drawing.Size(200, 25),
                DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            };
            pnlFilter.Controls.Add(cboNhaCungCap);

            System.Windows.Forms.Label lblNV = new System.Windows.Forms.Label
            {
                Text = "Nhân viên:",
                Location = new System.Drawing.Point(645, 15),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };
            pnlFilter.Controls.Add(lblNV);

            cboNhanVien = new System.Windows.Forms.ComboBox
            {
                Location = new System.Drawing.Point(730, 12),
                Size = new System.Drawing.Size(200, 25),
                DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            };
            pnlFilter.Controls.Add(cboNhanVien);

            // Row 2 - Date Range and Status
            System.Windows.Forms.Label lblTuNgay = new System.Windows.Forms.Label
            {
                Text = "Từ ngày:",
                Location = new System.Drawing.Point(15, 55),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };
            pnlFilter.Controls.Add(lblTuNgay);

            dtpTuNgay = new System.Windows.Forms.DateTimePicker
            {
                Location = new System.Drawing.Point(100, 52),
                Size = new System.Drawing.Size(150, 25),
                Format = System.Windows.Forms.DateTimePickerFormat.Custom,
                CustomFormat = "dd-MM-yyyy",
                Enabled = false
            };
            pnlFilter.Controls.Add(dtpTuNgay);

            System.Windows.Forms.Label lblDenNgay = new System.Windows.Forms.Label
            {
                Text = "Đến ngày:",
                Location = new System.Drawing.Point(260, 55),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };
            pnlFilter.Controls.Add(lblDenNgay);

            dtpDenNgay = new System.Windows.Forms.DateTimePicker
            {
                Location = new System.Drawing.Point(345, 52),
                Size = new System.Drawing.Size(150, 25),
                Format = System.Windows.Forms.DateTimePickerFormat.Custom,
                CustomFormat = "dd-MM-yyyy",
                Enabled = false
            };
            pnlFilter.Controls.Add(dtpDenNgay);

            chkLocTheoNgay = new System.Windows.Forms.CheckBox
            {
                Text = "Lọc theo ngày",
                Location = new System.Drawing.Point(505, 54),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };
            chkLocTheoNgay.CheckedChanged += (s, e) => {
                dtpTuNgay.Enabled = chkLocTheoNgay.Checked;
                dtpDenNgay.Enabled = chkLocTheoNgay.Checked;
            };
            pnlFilter.Controls.Add(chkLocTheoNgay);

            System.Windows.Forms.Label lblTrangThai = new System.Windows.Forms.Label
            {
                Text = "Trạng thái:",
                Location = new System.Drawing.Point(625, 55),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };
            pnlFilter.Controls.Add(lblTrangThai);

            rdoTatCa = new System.Windows.Forms.RadioButton
            {
                Text = "Tất cả",
                Location = new System.Drawing.Point(710, 52),
                AutoSize = true,
                Checked = true,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };
            pnlFilter.Controls.Add(rdoTatCa);

            rdoChoDuyet = new System.Windows.Forms.RadioButton
            {
                Text = "Chờ duyệt",
                Location = new System.Drawing.Point(800, 52),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };
            pnlFilter.Controls.Add(rdoChoDuyet);

            rdoDaDuyet = new System.Windows.Forms.RadioButton
            {
                Text = "Đã duyệt",
                Location = new System.Drawing.Point(910, 52),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };
            pnlFilter.Controls.Add(rdoDaDuyet);

            btnLoc = new System.Windows.Forms.Button
            {
                Text = "Lọc",
                Location = new System.Drawing.Point(1050, 12),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.FromArgb(8, 133, 204),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnLoc.FlatAppearance.BorderSize = 0;
            btnLoc.Click += BtnLoc_Click;
            pnlFilter.Controls.Add(btnLoc);

            btnReset = new System.Windows.Forms.Button
            {
                Text = "Đặt lại",
                Location = new System.Drawing.Point(1050, 52),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnReset.FlatAppearance.BorderSize = 0;
            btnReset.Click += BtnReset_Click;
            pnlFilter.Controls.Add(btnReset);

            // 
            // Panel Buttons
            // 
            pnlButtons = new System.Windows.Forms.Panel
            {
                Dock = System.Windows.Forms.DockStyle.Bottom,
                Height = 70,
                BackColor = System.Drawing.Color.WhiteSmoke
            };

            btnThem = new System.Windows.Forms.Button
            {
                Text = "Thêm mới",
                Location = new System.Drawing.Point(20, 15),
                Size = new System.Drawing.Size(120, 40),
                BackColor = System.Drawing.Color.FromArgb(40, 167, 69),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.Click += BtnThem_Click;
            pnlButtons.Controls.Add(btnThem);

            btnXem = new System.Windows.Forms.Button
            {
                Text = "Xem chi tiết",
                Location = new System.Drawing.Point(160, 15),
                Size = new System.Drawing.Size(120, 40),
                BackColor = System.Drawing.Color.FromArgb(8, 133, 204),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnXem.FlatAppearance.BorderSize = 0;
            btnXem.Click += BtnXem_Click;
            pnlButtons.Controls.Add(btnXem);

            btnSua = new System.Windows.Forms.Button
            {
                Text = "Sửa",
                Location = new System.Drawing.Point(300, 15),
                Size = new System.Drawing.Size(120, 40),
                BackColor = System.Drawing.Color.FromArgb(255, 193, 7),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnSua.FlatAppearance.BorderSize = 0;
            btnSua.Click += BtnSua_Click;
            pnlButtons.Controls.Add(btnSua);

            btnXoa = new System.Windows.Forms.Button
            {
                Text = "Xóa",
                Location = new System.Drawing.Point(440, 15),
                Size = new System.Drawing.Size(120, 40),
                BackColor = System.Drawing.Color.FromArgb(220, 53, 69),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.Click += BtnXoa_Click;
            pnlButtons.Controls.Add(btnXoa);

            btnDuyet = new System.Windows.Forms.Button
            {
                Text = "Duyệt phiếu",
                Location = new System.Drawing.Point(580, 15),
                Size = new System.Drawing.Size(120, 40),
                BackColor = System.Drawing.Color.FromArgb(23, 162, 184),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnDuyet.FlatAppearance.BorderSize = 0;
            btnDuyet.Click += BtnDuyet_Click;
            pnlButtons.Controls.Add(btnDuyet);

            btnExport = new System.Windows.Forms.Button
            {
                Text = "Xuất Excel",
                Location = new System.Drawing.Point(720, 15),
                Size = new System.Drawing.Size(120, 40),
                BackColor = System.Drawing.Color.FromArgb(108, 117, 125),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += BtnExport_Click;
            pnlButtons.Controls.Add(btnExport);

            // 
            // DataGridView
            // 
            dgvPhieuNhap = new System.Windows.Forms.DataGridView();
            dgvPhieuNhap.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvPhieuNhap.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhieuNhap.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvPhieuNhap.MultiSelect = false;
            dgvPhieuNhap.AllowUserToAddRows = false;
            dgvPhieuNhap.AllowUserToDeleteRows = false;
            dgvPhieuNhap.ReadOnly = true;
            dgvPhieuNhap.BackgroundColor = System.Drawing.Color.White;
            dgvPhieuNhap.RowHeadersVisible = false;
            dgvPhieuNhap.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(8, 133, 204);
            dgvPhieuNhap.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvPhieuNhap.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgvPhieuNhap.ColumnHeadersHeight = 40;
            dgvPhieuNhap.EnableHeadersVisualStyles = false;
            dgvPhieuNhap.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvPhieuNhap.CellDoubleClick += DgvPhieuNhap_CellDoubleClick;

            // Add columns
            dgvPhieuNhap.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn 
            { 
                Name = "MPN", 
                HeaderText = "Mã phiếu", 
                DataPropertyName = "MPN" 
            });

            dgvPhieuNhap.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn 
            { 
                Name = "TenNCC", 
                HeaderText = "Nhà cung cấp", 
                DataPropertyName = "TenNCC" 
            });

            dgvPhieuNhap.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn 
            { 
                Name = "TenNV", 
                HeaderText = "Nhân viên", 
                DataPropertyName = "TenNV" 
            });

            dgvPhieuNhap.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn 
            { 
                Name = "TG", 
                HeaderText = "Thời gian", 
                DataPropertyName = "TG" 
            });

            dgvPhieuNhap.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn 
            { 
                Name = "TongTien", 
                HeaderText = "Tổng tiền", 
                DataPropertyName = "TongTien" 
            });

            dgvPhieuNhap.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn 
            { 
                Name = "TrangThai", 
                HeaderText = "Trạng thái", 
                DataPropertyName = "TrangThai" 
            });

            // Format currency column
            dgvPhieuNhap.Columns["TongTien"].DefaultCellStyle.Format = "N0";
            dgvPhieuNhap.Columns["TongTien"].DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;

            // 
            // Add controls to form
            // 
            this.Controls.Add(dgvPhieuNhap);
            this.Controls.Add(pnlFilter);
            this.Controls.Add(pnlTop);
            this.Controls.Add(pnlButtons);

            this.ResumeLayout(false);
        }

        #endregion
    }
}
