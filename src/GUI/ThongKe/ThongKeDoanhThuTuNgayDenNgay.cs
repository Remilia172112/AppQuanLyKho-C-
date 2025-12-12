using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using src.BUS;
using src.DTO;
using src.Helper;

namespace src.GUI.ThongKe
{
    public class ThongKeDoanhThuTuNgayDenNgay : UserControl
    {
        private ThongKeBUS thongkeBUS;

        // UI Components
        private System.Windows.Forms.Panel pnlTop;
        private DataGridView tableThongKe;
        private DateTimePicker dateFrom;
        private DateTimePicker dateTo;
        private Button btnThongKe;
        private Button btnReset;
        private Button btnExport;

        public ThongKeDoanhThuTuNgayDenNgay(ThongKeBUS thongkeBUS)
        {
            this.thongkeBUS = thongkeBUS;
            InitComponent();
        }

        private void InitComponent()
        {
            this.Size = new Size(1000, 700);
            this.BackColor = Color.White;
            this.Padding = new Padding(10);

            // --- 1. Panel Top (Chứa control chọn ngày) ---
            pnlTop = new System.Windows.Forms.Panel();
            pnlTop.Height = 50;
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Padding = new Padding(5);

            FlowLayoutPanel flowTop = new FlowLayoutPanel();
            flowTop.Dock = DockStyle.Fill;
            flowTop.FlowDirection = FlowDirection.LeftToRight;
            flowTop.AutoSize = true;

            // DatePicker From
            Label lblFrom = new Label { Text = "Từ ngày:", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(5, 8, 5, 5) };
            dateFrom = new DateTimePicker 
            { 
                Format = DateTimePickerFormat.Custom, // Sửa thành Custom
                CustomFormat = "dd/MM/yyyy",          // Định dạng dd/MM/yyyy
                Width = 100 
            };

            // DatePicker To
            Label lblTo = new Label { Text = "Đến ngày:", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(15, 8, 5, 5) };
            dateTo = new DateTimePicker 
            { 
                Format = DateTimePickerFormat.Custom, // Sửa thành Custom
                CustomFormat = "dd/MM/yyyy",          // Định dạng dd/MM/yyyy
                Width = 100 
            };

            // Buttons
            btnThongKe = new Button { Text = "Thống kê", AutoSize = true, Cursor = Cursors.Hand };
            btnReset = new Button { Text = "Làm mới", AutoSize = true, Cursor = Cursors.Hand };
            btnExport = new Button { Text = "Xuất Excel", AutoSize = true, Cursor = Cursors.Hand };

            // Events
            btnThongKe.Click += BtnThongKe_Click;
            btnReset.Click += BtnReset_Click;
            btnExport.Click += BtnExport_Click;
            
            // Validate Date Change
            dateFrom.ValueChanged += Date_ValueChanged;
            dateTo.ValueChanged += Date_ValueChanged;

            flowTop.Controls.Add(lblFrom);
            flowTop.Controls.Add(dateFrom);
            flowTop.Controls.Add(lblTo);
            flowTop.Controls.Add(dateTo);
            flowTop.Controls.Add(btnThongKe);
            flowTop.Controls.Add(btnExport);
            flowTop.Controls.Add(btnReset);

            pnlTop.Controls.Add(flowTop);
            this.Controls.Add(pnlTop);

            // --- 2. DataGridView (Table) ---
            tableThongKe = new DataGridView();
            tableThongKe.Dock = DockStyle.Fill;
            tableThongKe.AllowUserToAddRows = false;
            tableThongKe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tableThongKe.ReadOnly = true;
            tableThongKe.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tableThongKe.BackgroundColor = Color.White;

            tableThongKe.Columns.Add("Ngay", "Ngày");
            tableThongKe.Columns.Add("ChiPhi", "Chi phí");
            tableThongKe.Columns.Add("DoanhThu", "Doanh thu");
            tableThongKe.Columns.Add("LoiNhuan", "Lợi nhuận");

            // Alignment
            tableThongKe.Columns["ChiPhi"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tableThongKe.Columns["DoanhThu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tableThongKe.Columns["LoiNhuan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.Controls.Add(tableThongKe);
            
            // Z-Order
            tableThongKe.BringToFront(); // Để table chiếm phần Fill còn lại
            pnlTop.Dock = DockStyle.Top; // Đảm bảo top nằm trên
            pnlTop.SendToBack();
        }

        private bool ValidateSelectDate()
        {
            DateTime start = dateFrom.Value.Date;
            DateTime end = dateTo.Value.Date;
            DateTime current = DateTime.Now.Date;

            if (start > current)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày hiện tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateFrom.Value = current;
                return false;
            }
            if (end > current)
            {
                MessageBox.Show("Ngày kết thúc không được lớn hơn ngày hiện tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateTo.Value = current;
                return false;
            }
            if (start > end)
            {
                MessageBox.Show("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateTo.Value = start;
                return false;
            }
            return true;
        }

        private void Date_ValueChanged(object sender, EventArgs e)
        {
            // Tự động validate khi chọn ngày
            ValidateSelectDate();
        }

        public void LoadThongKeTungNgayTrongThang(string start, string end)
        {
            var list = thongkeBUS.GetThongKeTuNgayDenNgay(start, end);
            tableThongKe.Rows.Clear();
            foreach (var item in list)
            {
                tableThongKe.Rows.Add(
                    item.Ngay.ToString("dd/MM/yyyy"),
                    Formater.FormatVND(item.Chiphi),
                    Formater.FormatVND(item.Doanhthu),
                    Formater.FormatVND(item.Loinhuan)
                );
            }
        }

        private void BtnThongKe_Click(object sender, EventArgs e)
        {
            if (ValidateSelectDate())
            {
                // Format yyyy-MM-dd để query SQL
                string start = dateFrom.Value.ToString("yyyy-MM-dd");
                string end = dateTo.Value.ToString("yyyy-MM-dd");
                LoadThongKeTungNgayTrongThang(start, end);
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            dateFrom.Value = DateTime.Now;
            dateTo.Value = DateTime.Now;
            tableThongKe.Rows.Clear();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            TableExporter.ExportTableToExcel(tableThongKe, "TKDTTNN");
        }
    }
}