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
        
        // Input Controls (Thay thế InputForm/InputDate custom)
        private TextBox txtTenKhachHang;
        private DateTimePicker dateStart;
        private DateTimePicker dateEnd;
        private Button btnExport;
        private Button btnReset;

        public ThongKeKhachHang(ThongKeBUS thongkebus)
        {
            this.thongkebus = thongkebus;
            // Load dữ liệu ban đầu (Từ năm 1970 đến hiện tại -> MinValue đến Now)
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
            pnlLeft.Padding = new Padding(0, 0, 10, 0); // Margin phải
            
            // Container cho các input (Xếp dọc)
            FlowLayoutPanel flowLeft = new FlowLayoutPanel();
            flowLeft.Dock = DockStyle.Fill;
            flowLeft.FlowDirection = FlowDirection.TopDown;
            flowLeft.WrapContents = false;
            flowLeft.AutoSize = true;

            // Input: Tên khách hàng
            System.Windows.Forms.Panel pnlTen = CreateInputPanel("Tìm kiếm khách hàng", out txtTenKhachHang);
            txtTenKhachHang.TextChanged += (s, e) => Filter(); // Sự kiện gõ phím

            // Input: Từ ngày
            System.Windows.Forms.Panel pnlStart = CreateDatePanel("Từ ngày", out dateStart);
            dateStart.Value = DateTime.Now.AddYears(-5); // Mặc định lùi 5 năm cho có dữ liệu
            dateStart.ValueChanged += (s, e) => Filter();

            // Input: Đến ngày
            System.Windows.Forms.Panel pnlEnd = CreateDatePanel("Đến ngày", out dateEnd);
            dateEnd.ValueChanged += (s, e) => Filter();

            // Buttons
            System.Windows.Forms.Panel pnlBtn = new System.Windows.Forms.Panel { Size = new Size(280, 50), Padding = new Padding(0, 10, 0, 0) };
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
            pnlCenter = new System.Windows.Forms.Panel();
            pnlCenter.Dock = DockStyle.Fill;

            tblKH = new DataGridView();
            tblKH.Dock = DockStyle.Fill;
            tblKH.AllowUserToAddRows = false;
            tblKH.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tblKH.ReadOnly = true;
            tblKH.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tblKH.BackgroundColor = Color.White;

            tblKH.Columns.Add("STT", "STT");
            tblKH.Columns.Add("MaKH", "Mã khách hàng");
            tblKH.Columns.Add("TenKH", "Tên khách hàng");
            tblKH.Columns.Add("SoLuong", "Số lượng phiếu");
            tblKH.Columns.Add("TongTien", "Tổng số tiền");

            // Format cột
            tblKH.Columns["STT"].Width = 10;
            tblKH.Columns["MaKH"].Width = 10;
            tblKH.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tblKH.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            pnlCenter.Controls.Add(tblKH);
            this.Controls.Add(pnlCenter);
            
            // Đảm bảo thứ tự Dock (Left trước, Fill sau)
            pnlCenter.BringToFront();
        }

        // Hàm tạo Panel nhập liệu Text (Thay InputForm)
        private System.Windows.Forms.Panel CreateInputPanel(string title, out TextBox txt)
        {
            System.Windows.Forms.Panel p = new System.Windows.Forms.Panel { Size = new Size(280, 60), Margin = new Padding(0, 0, 0, 10) };
            Label lbl = new Label { Text = title, Dock = DockStyle.Top, Height = 25 };
            txt = new TextBox { Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 11) };
            p.Controls.Add(txt);
            p.Controls.Add(lbl);
            return p;
        }

        // Hàm tạo Panel chọn ngày (Thay InputDate)
        private System.Windows.Forms.Panel CreateDatePanel(string title, out DateTimePicker dtp)
        {
            System.Windows.Forms.Panel p = new System.Windows.Forms.Panel { Size = new Size(280, 60), Margin = new Padding(0, 0, 0, 10) };
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
                dateStart.Value = current; // Reset về hiện tại
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
                dateEnd.Value = start; // Reset về bằng ngày bắt đầu
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
            dateStart.Value = DateTime.Now.AddYears(-5); // Reset về khoảng rộng để thấy dữ liệu
            dateEnd.Value = DateTime.Now;
            Filter();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            TableExporter.ExportTableToExcel(tblKH, "TKKH");
        }
    }
}