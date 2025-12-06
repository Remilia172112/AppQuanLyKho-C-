using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using src.BUS;
using src.DTO.ThongKe;
using src.Helper;

namespace src.GUI.ThongKe
{
    public class ThongKeNhaCungCap : UserControl
    {
        private ThongKeBUS thongkebus;
        private List<ThongKeNhaCungCapDTO> list;

        // UI Components
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlCenter;
        private DataGridView tblNCC;
        
        // Input Controls
        private TextBox txtTenNCC;
        private DateTimePicker dateStart;
        private DateTimePicker dateEnd;
        private Button btnExport;
        private Button btnReset;

        public ThongKeNhaCungCap(ThongKeBUS thongkebus)
        {
            this.thongkebus = thongkebus;
            this.list = thongkebus.GetAllNCC();
            
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
            
            FlowLayoutPanel flowLeft = new FlowLayoutPanel();
            flowLeft.Dock = DockStyle.Fill;
            flowLeft.FlowDirection = FlowDirection.TopDown;
            flowLeft.WrapContents = false;
            flowLeft.AutoSize = true;

            // Input: Tên NCC
            System.Windows.Forms.Panel pnlTen = CreateInputPanel("Tìm kiếm nhà cung cấp", out txtTenNCC);
            txtTenNCC.TextChanged += (s, e) => Filter();

            // Input: Từ ngày
            System.Windows.Forms.Panel pnlStart = CreateDatePanel("Từ ngày", out dateStart);
            dateStart.Value = DateTime.Now.AddYears(-5);
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

            tblNCC = new DataGridView();
            tblNCC.Dock = DockStyle.Fill;
            tblNCC.AllowUserToAddRows = false;
            tblNCC.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tblNCC.ReadOnly = true;
            tblNCC.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tblNCC.BackgroundColor = Color.White;

            tblNCC.Columns.Add("STT", "STT");
            tblNCC.Columns.Add("MaNCC", "Mã nhà cung cấp");
            tblNCC.Columns.Add("TenNCC", "Tên nhà cung cấp");
            tblNCC.Columns.Add("SoLuong", "Số lượng nhập");
            tblNCC.Columns.Add("TongTien", "Tổng số tiền");

            // Format cột
            tblNCC.Columns["STT"].Width = 10;
            tblNCC.Columns["MaNCC"].Width = 10;
            tblNCC.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tblNCC.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            pnlCenter.Controls.Add(tblNCC);
            this.Controls.Add(pnlCenter);
            
            pnlCenter.BringToFront();
        }

        private System.Windows.Forms.Panel CreateInputPanel(string title, out TextBox txt)
        {
            System.Windows.Forms.Panel p = new System.Windows.Forms.Panel { Size = new Size(280, 60), Margin = new Padding(0, 0, 0, 10) };
            Label lbl = new Label { Text = title, Dock = DockStyle.Top, Height = 25 };
            txt = new TextBox { Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 11) };
            p.Controls.Add(txt);
            p.Controls.Add(lbl);
            return p;
        }

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
                string input = txtTenNCC.Text.Trim();
                DateTime start = dateStart.Value;
                DateTime end = dateEnd.Value;

                this.list = thongkebus.FilterNCC(input, start, end);
                LoadDataTable(this.list);
            }
        }

        private void LoadDataTable(List<ThongKeNhaCungCapDTO> result)
        {
            tblNCC.Rows.Clear();
            int k = 1;
            foreach (var i in result)
            {
                tblNCC.Rows.Add(
                    k,
                    i.Mancc,
                    i.Tenncc,
                    i.Soluong,
                    Formater.FormatVND(i.Tongtien)
                );
                k++;
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            txtTenNCC.Text = "";
            dateStart.Value = DateTime.Now.AddYears(-5);
            dateEnd.Value = DateTime.Now;
            Filter();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            JTableExporter.ExportJTableToExcel(tblNCC);
        }
    }
}