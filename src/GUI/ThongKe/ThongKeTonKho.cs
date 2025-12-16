using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using src.BUS;
using src.DTO.ThongKe;
using src.Helper;

namespace src.GUI.ThongKe
{
    public class ThongKeTonKho : UserControl
    {
        private ThongKeBUS thongkeBUS;
        private List<ThongKeTonKhoDTO> listSp;

        // UI Components
        private Panel pnlLeft;
        private Panel pnlCenter;
        private DataGridView tblTonKho;
        
        // Input Controls
        private TextBox txtTenSanPham;
        private DateTimePicker dateStart;
        private DateTimePicker dateEnd;
        private Button btnExport;
        private Button btnReset;
        private Button btnSearch; // Nút tìm kiếm mới

        public ThongKeTonKho(ThongKeBUS thongkeBUS)
        {
            this.thongkeBUS = thongkeBUS;
            this.listSp = thongkeBUS.GetTonKho(); // BUS trả về List<DTO>
            
            InitComponent();
            LoadDataTable(this.listSp);
        }

        private void InitComponent()
        {
            this.Size = new Size(1000, 700);
            this.BackColor = Color.White;
            this.Padding = new Padding(10);

            // --- 1. Panel Left (Bộ lọc) ---
            pnlLeft = new Panel();
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Width = 300;
            pnlLeft.Padding = new Padding(0, 0, 10, 0);

            FlowLayoutPanel flowLeft = new FlowLayoutPanel();
            flowLeft.Dock = DockStyle.Fill;
            flowLeft.FlowDirection = FlowDirection.TopDown;
            flowLeft.WrapContents = false;
            flowLeft.AutoSize = true;

            // Input: Tên sản phẩm
            Panel pnlTen = CreateInputPanel("Tìm kiếm sản phẩm", out txtTenSanPham);
            txtTenSanPham.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) Filter(); }; // Enter để tìm

            // Input: Từ ngày
            Panel pnlStart = CreateDatePanel("Từ ngày", out dateStart);
            dateStart.Value = DateTime.Now.AddYears(-5);
            // dateStart.ValueChanged += (s, e) => Filter(); // Bỏ auto filter nếu muốn phải ấn nút tìm

            // Input: Đến ngày
            Panel pnlEnd = CreateDatePanel("Đến ngày", out dateEnd);
            // dateEnd.ValueChanged += (s, e) => Filter();

            // Buttons Container
            Panel pnlBtn = new Panel { Size = new Size(290, 50), Padding = new Padding(0, 10, 0, 0) };
            
            // 1. Nút Tìm kiếm (Mới) - Màu Xanh Dương
            btnSearch = new Button 
            { 
                Text = "Tìm kiếm", 
                Width = 90, 
                Height = 35, 
                Location = new Point(0, 10), 
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += (s, e) => Filter();

            // 2. Nút Làm mới (Kế bên Tìm kiếm) - Màu Xám
            btnReset = new Button 
            { 
                Text = "Làm mới", 
                Width = 90, 
                Height = 35, 
                Location = new Point(95, 10), 
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnReset.FlatAppearance.BorderSize = 0;
            btnReset.Click += BtnReset_Click;

            // 3. Nút Xuất Excel (Sau Làm mới) - Màu Xanh Lá
            btnExport = new Button 
            { 
                Text = "Xuất Excel", 
                Width = 90, 
                Height = 35, 
                Location = new Point(190, 10), 
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += BtnExport_Click;

            pnlBtn.Controls.Add(btnSearch);
            pnlBtn.Controls.Add(btnReset);
            pnlBtn.Controls.Add(btnExport);

            flowLeft.Controls.Add(pnlTen);
            flowLeft.Controls.Add(pnlStart);
            flowLeft.Controls.Add(pnlEnd);
            flowLeft.Controls.Add(pnlBtn);
            pnlLeft.Controls.Add(flowLeft);

            this.Controls.Add(pnlLeft);

            // --- 2. Panel Center (Bảng dữ liệu) ---
            pnlCenter = new Panel();
            pnlCenter.Dock = DockStyle.Fill;

            tblTonKho = new DataGridView();
            tblTonKho.Dock = DockStyle.Fill;
            tblTonKho.AllowUserToAddRows = false;
            tblTonKho.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tblTonKho.ReadOnly = true;
            tblTonKho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tblTonKho.BackgroundColor = Color.White;
            tblTonKho.ColumnHeadersHeight = 40;
            tblTonKho.RowTemplate.Height = 30;
            
            // Header Style
            tblTonKho.EnableHeadersVisualStyles = false;
            tblTonKho.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
            tblTonKho.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tblTonKho.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            tblTonKho.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Sự kiện double click
            tblTonKho.CellDoubleClick += TblTonKho_CellDoubleClick;

            tblTonKho.Columns.Add("STT", "STT");
            tblTonKho.Columns.Add("MaSP", "Mã SP");
            tblTonKho.Columns.Add("TenSP", "Tên sản phẩm");
            tblTonKho.Columns.Add("TonDau", "Tồn đầu kỳ");
            tblTonKho.Columns.Add("Nhap", "Nhập trong kỳ");
            tblTonKho.Columns.Add("Xuat", "Xuất trong kỳ");
            tblTonKho.Columns.Add("TonCuoi", "Tồn cuối kỳ");

            // Format cột
            tblTonKho.Columns["STT"].Width = 10;
            tblTonKho.Columns["MaSP"].Width = 15;
            tblTonKho.Columns["TenSP"].Width = 50;
            tblTonKho.Columns["STT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tblTonKho.Columns["MaSP"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tblTonKho.Columns["TonDau"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tblTonKho.Columns["Nhap"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tblTonKho.Columns["Xuat"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tblTonKho.Columns["TonCuoi"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            pnlCenter.Controls.Add(tblTonKho);
            this.Controls.Add(pnlCenter);
            
            pnlCenter.BringToFront();
        }

        private Panel CreateInputPanel(string title, out TextBox txt)
        {
            Panel p = new Panel { Size = new Size(280, 60), Margin = new Padding(0, 0, 0, 10) };
            Label lbl = new Label { Text = title, Dock = DockStyle.Top, Height = 25, Font = new Font("Segoe UI", 10) };
            txt = new TextBox { Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 11) };
            p.Controls.Add(txt);
            p.Controls.Add(lbl);
            return p;
        }

        private Panel CreateDatePanel(string title, out DateTimePicker dtp)
        {
            Panel p = new Panel { Size = new Size(280, 60), Margin = new Padding(0, 0, 0, 10) };
            Label lbl = new Label { Text = title, Dock = DockStyle.Top, Height = 25, Font = new Font("Segoe UI", 10) };
            dtp = new DateTimePicker 
            { 
                Dock = DockStyle.Top, 
                Format = DateTimePickerFormat.Custom, 
                CustomFormat = "dd/MM/yyyy", 
                Height = 30, 
                Font = new Font("Segoe UI", 11) 
            };
            p.Controls.Add(dtp);
            p.Controls.Add(lbl);
            return p;
        }

        private bool ValidateSelectDate()
        {
            DateTime start = dateStart.Value.Date;
            DateTime end = dateEnd.Value.Date;
            DateTime current = DateTime.Now.Date;

            if (start > current)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày hiện tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateStart.Value = current;
                return false;
            }
            if (end > current)
            {
                MessageBox.Show("Ngày kết thúc không được lớn hơn ngày hiện tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateEnd.Value = current;
                return false;
            }
            if (start > end)
            {
                MessageBox.Show("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateEnd.Value = start;
                return false;
            }
            return true;
        }

        private void Filter()
        {
            if (ValidateSelectDate())
            {
                string input = txtTenSanPham.Text.Trim();
                DateTime start = dateStart.Value;
                DateTime end = dateEnd.Value;

                this.listSp = thongkeBUS.FilterTonKho(input, start, end);
                LoadDataTable(this.listSp);
            }
        }

        private void LoadDataTable(List<ThongKeTonKhoDTO> list)
        {
            tblTonKho.Rows.Clear();
            int k = 1;
            foreach (var item in list)
            {
                tblTonKho.Rows.Add(
                    k,
                    item.Masp,
                    item.Tensanpham,
                    item.Tondauky,
                    item.Nhaptrongky,
                    item.Xuattrongky,
                    item.Toncuoiky
                );
                k++;
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            txtTenSanPham.Text = "";
            dateStart.Value = DateTime.Now.AddYears(-5);
            dateEnd.Value = DateTime.Now;
            Filter();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (tblTonKho.Rows.Count > 0)
                    TableExporter.ExportTableToExcel(tblTonKho, "ThongKeTonKho");
                else
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất Excel: {ex.Message}");
            }
        }

        private void TblTonKho_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int masp = (int)tblTonKho.Rows[e.RowIndex].Cells["MaSP"].Value;
                MessageBox.Show($"Xem chi tiết tồn kho sản phẩm ID: {masp} (Tính năng đang phát triển)");
            }
        }
    }
}