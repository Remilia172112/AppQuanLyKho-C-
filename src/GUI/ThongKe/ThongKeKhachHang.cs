using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using src.BUS;
using src.DTO.ThongKe;
using src.Helper;

namespace src.GUI.ThongKe
{
    public class ThongKeKhachHang : UserControl
    {
        private ThongKeBUS thongkebus;
        private List<ThongKeKhachHangDTO> list;

        // UI Components
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlCenter;
        private DataGridView tblKH;
        
        // Input Controls
        private TextBox txtTenKhachHang;
        private DateTimePicker dateStart;
        private DateTimePicker dateEnd;
        private Button btnExport;
        private Button btnReset;
        private Button btnSearch; // Thêm nút tìm kiếm

        public ThongKeKhachHang(ThongKeBUS thongkebus)
        {
            this.thongkebus = thongkebus;
            // Load dữ liệu ban đầu
            this.list = thongkebus.GetAllKhachHang();
            
            InitComponent();
            LoadDataTable(this.list);
        }

        private void InitComponent()
        {
            this.Size = new Size(1000, 700);
            this.BackColor = Color.White;
            this.Padding = new Padding(10);

            // --- 1. Panel Left (Bộ lọc) ---
            pnlLeft = new System.Windows.Forms.Panel();
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Width = 300;
            pnlLeft.Padding = new Padding(0, 0, 10, 0); 
            
            // Container cho các input (Xếp dọc)
            FlowLayoutPanel flowLeft = new FlowLayoutPanel();
            flowLeft.Dock = DockStyle.Fill;
            flowLeft.FlowDirection = FlowDirection.TopDown;
            flowLeft.WrapContents = false;
            flowLeft.AutoSize = true;

            // Input: Tên khách hàng
            System.Windows.Forms.Panel pnlTen = CreateInputPanel("Tìm kiếm khách hàng", out txtTenKhachHang);
            // Bấm Enter để tìm
            txtTenKhachHang.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) Filter(); };

            // Input: Từ ngày
            System.Windows.Forms.Panel pnlStart = CreateDatePanel("Từ ngày", out dateStart);
            dateStart.Value = DateTime.Now.AddYears(-5); 

            // Input: Đến ngày
            System.Windows.Forms.Panel pnlEnd = CreateDatePanel("Đến ngày", out dateEnd);

            // --- Buttons Container ---
            System.Windows.Forms.Panel pnlBtn = new System.Windows.Forms.Panel { Size = new Size(290, 50), Padding = new Padding(0, 10, 0, 0) };
            
            // 1. Nút Tìm kiếm - Màu Xanh Dương
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

            // 2. Nút Làm mới - Màu Xám
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

            // 3. Nút Xuất Excel - Màu Xanh Lá
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
            pnlCenter = new System.Windows.Forms.Panel();
            pnlCenter.Dock = DockStyle.Fill;

            tblKH = new DataGridView();
            tblKH.Dock = DockStyle.Fill;
            tblKH.AllowUserToAddRows = false;
            tblKH.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tblKH.ReadOnly = true;
            tblKH.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tblKH.BackgroundColor = Color.White;
            
            // Style Header
            tblKH.EnableHeadersVisualStyles = false;
            tblKH.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
            tblKH.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tblKH.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            tblKH.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tblKH.ColumnHeadersHeight = 40;

            tblKH.Columns.Add("STT", "STT");
            tblKH.Columns.Add("MaKH", "Mã KH");
            tblKH.Columns.Add("TenKH", "Tên khách hàng");
            tblKH.Columns.Add("SoLuong", "Số lượng phiếu");
            tblKH.Columns.Add("TongTien", "Tổng số tiền");

            tblKH.Columns["STT"].Width = 10;
            tblKH.Columns["MaKH"].Width = 15;
            tblKH.Columns["SoLuong"].Width = 28;
            tblKH.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tblKH.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            pnlCenter.Controls.Add(tblKH);
            this.Controls.Add(pnlCenter);
            
            pnlCenter.BringToFront();
        }

        // Hàm tạo Panel nhập liệu Text
        private System.Windows.Forms.Panel CreateInputPanel(string title, out TextBox txt)
        {
            System.Windows.Forms.Panel p = new System.Windows.Forms.Panel { Size = new Size(280, 60), Margin = new Padding(0, 0, 0, 10) };
            Label lbl = new Label { Text = title, Dock = DockStyle.Top, Height = 25 };
            txt = new TextBox { Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 11) };
            p.Controls.Add(txt);
            p.Controls.Add(lbl);
            return p;
        }

        // Hàm tạo Panel chọn ngày
        private System.Windows.Forms.Panel CreateDatePanel(string title, out DateTimePicker dtp)
        {
            System.Windows.Forms.Panel p = new System.Windows.Forms.Panel { Size = new Size(280, 60), Margin = new Padding(0, 0, 0, 10) };
            Label lbl = new Label { Text = title, Dock = DockStyle.Top, Height = 25 };
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
                string input = txtTenKhachHang.Text.Trim();
                DateTime start = dateStart.Value;
                DateTime end = dateEnd.Value;

                // Gọi BUS lọc dữ liệu
                this.list = thongkebus.FilterKhachHang(input, start, end);
                LoadDataTable(this.list);
            }
        }

        private void LoadDataTable(List<ThongKeKhachHangDTO> result)
        {
            tblKH.Rows.Clear();
            int k = 1;
            foreach (var i in result)
            {
                tblKH.Rows.Add(
                    k,
                    i.Makh,
                    i.Tenkh,
                    i.Soluongphieu,
                    Formater.FormatVND(i.Tongtien)
                );
                k++;
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            txtTenKhachHang.Text = "";
            dateStart.Value = DateTime.Now.AddYears(-5);
            dateEnd.Value = DateTime.Now;
            Filter();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try 
            {
                if (tblKH.Rows.Count > 0)
                    TableExporter.ExportTableToExcel(tblKH, "TKKH");
                else
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất Excel: {ex.Message}");
            }
        }
    }
}