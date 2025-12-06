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
            txtTenSanPham.TextChanged += (s, e) => Filter();

            // Input: Từ ngày
            Panel pnlStart = CreateDatePanel("Từ ngày", out dateStart);
            dateStart.Value = DateTime.Now.AddYears(-5);
            dateStart.ValueChanged += (s, e) => Filter();

            // Input: Đến ngày
            Panel pnlEnd = CreateDatePanel("Đến ngày", out dateEnd);
            dateEnd.ValueChanged += (s, e) => Filter();

            // Buttons
            Panel pnlBtn = new Panel { Size = new Size(280, 50), Padding = new Padding(0, 10, 0, 0) };
            btnExport = new Button { Text = "Xuất Excel", Width = 100, Height = 35, Location = new Point(0, 10), Cursor = Cursors.Hand };
            btnReset = new Button { Text = "Làm mới", Width = 100, Height = 35, Location = new Point(110, 10), Cursor = Cursors.Hand };
            
            btnExport.Click += BtnExport_Click;
            btnReset.Click += BtnReset_Click;

            pnlBtn.Controls.Add(btnExport);
            pnlBtn.Controls.Add(btnReset);

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
            tblTonKho.Columns["MaSP"].Width = 10;
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
            Label lbl = new Label { Text = title, Dock = DockStyle.Top, Height = 25 };
            txt = new TextBox { Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 11) };
            p.Controls.Add(txt);
            p.Controls.Add(lbl);
            return p;
        }

        private Panel CreateDatePanel(string title, out DateTimePicker dtp)
        {
            Panel p = new Panel { Size = new Size(280, 60), Margin = new Padding(0, 0, 0, 10) };
            Label lbl = new Label { Text = title, Dock = DockStyle.Top, Height = 25 };
            dtp = new DateTimePicker 
            { 
                Dock = DockStyle.Top, 
                Format = DateTimePickerFormat.Custom,  // 1. Chế độ Custom
                CustomFormat = "dd/MM/yyyy",           // 2. Định dạng ngày/tháng/năm
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
                // Logic tính toán số lượng: Trong C# BUS, hàm GetSoluong nhận List<DTO>
                // Nhưng ở đây listSp là List phẳng, không phải HashMap như Java
                // Nên ta hiển thị trực tiếp giá trị của item
                
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
            JTableExporter.ExportJTableToExcel(tblTonKho);
        }

        private void TblTonKho_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int masp = (int)tblTonKho.Rows[e.RowIndex].Cells["MaSP"].Value;
                MessageBox.Show($"Xem chi tiết tồn kho sản phẩm ID: {masp} (Tính năng đang phát triển)");
                // ThongKePBSPTonKho dialog = new ThongKePBSPTonKho(...);
                // dialog.ShowDialog();
            }
        }
    }
}