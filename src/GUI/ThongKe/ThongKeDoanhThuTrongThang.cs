using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using src.BUS;
using src.DTO;
using src.Helper;

namespace src.GUI.ThongKe
{
    public class ThongKeDoanhThuTrongThang : UserControl
    {
        private ThongKeBUS thongkeBUS;

        // UI Components
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlChartContainer; // Thay cho PanelBorderRadius
        private DataGridView tableThongKe;
        private ComboBox cboThang; // Thay JMonthChooser
        private ComboBox cboNam;   // Thay JYearChooser
        private Button btnThongKe;
        private Button btnExport;

        // Chart Component
        private Chart chartDoanhThu;

        public ThongKeDoanhThuTrongThang(ThongKeBUS thongkeBUS)
        {
            this.thongkeBUS = thongkeBUS;
            InitComponent();

            // Mặc định load dữ liệu tháng hiện tại
            LoadThongKeTungNgayTrongThang(DateTime.Now.Month, DateTime.Now.Year);
        }

        private void InitComponent()
        {
            this.Size = new Size(1000, 700); // Kích thước mặc định
            this.BackColor = Color.White;
            this.Padding = new Padding(10);

            // --- 1. Panel Top (Chứa nút chọn tháng/năm) ---
            pnlTop = new System.Windows.Forms.Panel();
            pnlTop.Height = 50;
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Padding = new Padding(5);

            // FlowLayoutPanel để xếp ngang các control chọn ngày
            FlowLayoutPanel flowTop = new FlowLayoutPanel();
            flowTop.Dock = DockStyle.Fill;
            flowTop.FlowDirection = FlowDirection.LeftToRight;
            flowTop.AutoSize = true;

            // Label & ComboBox Tháng
            Label lblThang = new Label { Text = "Chọn tháng:", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(5, 8, 5, 5) };
            cboThang = new ComboBox { Width = 60, DropDownStyle = ComboBoxStyle.DropDownList };
            for (int i = 1; i <= 12; i++) cboThang.Items.Add(i);
            cboThang.SelectedItem = DateTime.Now.Month; // Chọn tháng hiện tại

            // Label & ComboBox Năm
            Label lblNam = new Label { Text = "Chọn năm:", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(15, 8, 5, 5) };
            cboNam = new ComboBox { Width = 80, DropDownStyle = ComboBoxStyle.DropDownList };
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 10; i <= currentYear + 10; i++) cboNam.Items.Add(i);
            cboNam.SelectedItem = currentYear; // Chọn năm hiện tại

            // Button Thống kê
            btnThongKe = new Button { Text = "Thống kê", AutoSize = true, Cursor = Cursors.Hand };
            btnThongKe.Click += BtnThongKe_Click;

            // Button Xuất Excel
            btnExport = new Button { Text = "Xuất Excel", AutoSize = true, Cursor = Cursors.Hand };
            btnExport.Click += BtnExport_Click;

            // Add vào FlowLayout
            flowTop.Controls.Add(lblThang);
            flowTop.Controls.Add(cboThang);
            flowTop.Controls.Add(lblNam);
            flowTop.Controls.Add(cboNam);
            flowTop.Controls.Add(btnThongKe);
            flowTop.Controls.Add(btnExport);

            pnlTop.Controls.Add(flowTop);
            this.Controls.Add(pnlTop);

            // --- 2. DataGridView (Bảng thống kê - Bottom) ---
            tableThongKe = new DataGridView();
            tableThongKe.Dock = DockStyle.Bottom;
            tableThongKe.Height = 250; // Chiều cao bảng
            tableThongKe.AllowUserToAddRows = false;
            tableThongKe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tableThongKe.ReadOnly = true;
            tableThongKe.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tableThongKe.BackgroundColor = Color.White;
            
            // Add cột
            tableThongKe.Columns.Add("Ngay", "Ngày");
            tableThongKe.Columns.Add("ChiPhi", "Chi phí");
            tableThongKe.Columns.Add("DoanhThu", "Doanh thu");
            tableThongKe.Columns.Add("LoiNhuan", "Lợi nhuận");

            // Format cột tiền tệ (Căn phải)
            tableThongKe.Columns["ChiPhi"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tableThongKe.Columns["DoanhThu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tableThongKe.Columns["LoiNhuan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.Controls.Add(tableThongKe);

            // --- 3. Chart Area (Biểu đồ - Center) ---
            pnlChartContainer = new System.Windows.Forms.Panel();
            pnlChartContainer.Dock = DockStyle.Fill;
            pnlChartContainer.Padding = new Padding(0, 10, 0, 10);
            
            InitChart(); // Hàm khởi tạo biểu đồ
            pnlChartContainer.Controls.Add(chartDoanhThu);

            this.Controls.Add(pnlChartContainer);
            
            // Đảm bảo thứ tự hiển thị đúng (Top -> Chart -> Bottom)
            pnlTop.BringToFront();
            tableThongKe.SendToBack();
        }

        private void InitChart()
        {
            chartDoanhThu = new Chart();
            chartDoanhThu.MinimumSize = new Size(100, 100);
            chartDoanhThu.Dock = DockStyle.Fill;

            // Tạo ChartArea
            ChartArea chartArea = new ChartArea("MainArea");
            chartDoanhThu.ChartAreas.Add(chartArea);

            // Tạo Legend (Chú thích)
            Legend legend = new Legend("Legend1");
            legend.Docking = Docking.Top; // Chú thích nằm trên cùng
            chartDoanhThu.Legends.Add(legend);

            // Tạo các Series (Dữ liệu) - Vốn (Chi phí), Doanh thu, Lợi nhuận
            // Series 1: Vốn (Chi phí)
            Series seriesVon = new Series("Vốn");
            seriesVon.ChartType = SeriesChartType.Column; // Biểu đồ cột
            seriesVon.Color = Color.FromArgb(245, 189, 135);
            chartDoanhThu.Series.Add(seriesVon);

            // Series 2: Doanh thu
            Series seriesDoanhThu = new Series("Doanh thu");
            seriesDoanhThu.ChartType = SeriesChartType.Column;
            seriesDoanhThu.Color = Color.FromArgb(135, 189, 245);
            chartDoanhThu.Series.Add(seriesDoanhThu);

            // Series 3: Lợi nhuận
            Series seriesLoiNhuan = new Series("Lợi nhuận");
            seriesLoiNhuan.ChartType = SeriesChartType.Column;
            seriesLoiNhuan.Color = Color.FromArgb(189, 135, 245);
            chartDoanhThu.Series.Add(seriesLoiNhuan);
        }

        public void LoadThongKeTungNgayTrongThang(int thang, int nam)
        {
            // Lấy dữ liệu từ BUS
            // Lưu ý: Hàm getThongKeTungNgayTrongThang trong BUS trả về List<ThongKeTungNgayTrongThangDTO>
            var list = thongkeBUS.GetThongKeTungNgayTrongThang(thang, nam);

            // 1. Cập nhật Biểu đồ
            // Xóa dữ liệu cũ
            chartDoanhThu.Series["Vốn"].Points.Clear();
            chartDoanhThu.Series["Doanh thu"].Points.Clear();
            chartDoanhThu.Series["Lợi nhuận"].Points.Clear();

            // Logic gộp nhóm 3 ngày (giống code Java cũ)
            long sum_chiphi = 0;
            long sum_doanhthu = 0;
            long sum_loinhuan = 0;

            for (int i = 0; i < list.Count; i++)
            {
                int index = i + 1;
                sum_chiphi += list[i].Chiphi;
                sum_doanhthu += list[i].Doanhthu;
                sum_loinhuan += list[i].Loinhuan;

                // Cứ mỗi 3 ngày (hoặc ngày cuối cùng) thì vẽ 1 cột gom nhóm
                // Logic Java cũ: if (index % 3 == 0) -> vẽ. Nhưng nếu tháng có 31 ngày, ngày 31 sẽ bị sót nếu không check thêm.
                // Ở đây mình giữ nguyên logic Java của bạn: gom nhóm 3 ngày.
                if (index % 3 == 0 || index == list.Count) 
                {
                    string label = $"Ngày {(index - (index % 3 == 0 ? 2 : index % 3 - 1))}->{index}";
                    if (index % 3 == 0) label = $"Ngày {index - 2}->{index}";
                    else label = $"Ngày {index - (index % 3) + 1}->{index}"; // Xử lý ngày lẻ cuối tháng

                    chartDoanhThu.Series["Vốn"].Points.AddXY(label, sum_chiphi);
                    chartDoanhThu.Series["Doanh thu"].Points.AddXY(label, sum_doanhthu);
                    chartDoanhThu.Series["Lợi nhuận"].Points.AddXY(label, sum_loinhuan);

                    // Reset biến cộng dồn
                    sum_chiphi = 0;
                    sum_doanhthu = 0;
                    sum_loinhuan = 0;
                }
            }

            // 2. Cập nhật Bảng (DataGridView)
            tableThongKe.Rows.Clear();
            foreach (var item in list)
            {
                tableThongKe.Rows.Add(
                    item.Ngay.ToString("dd/MM/yyyy"), // Format ngày
                    Formater.FormatVND(item.Chiphi),
                    Formater.FormatVND(item.Doanhthu),
                    Formater.FormatVND(item.Loinhuan)
                );
            }
        }

        private void BtnThongKe_Click(object sender, EventArgs e)
        {
            int thang = (int)cboThang.SelectedItem;
            int nam = (int)cboNam.SelectedItem;
            LoadThongKeTungNgayTrongThang(thang, nam);
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            // Gọi hàm export đã viết ở bài trước
            JTableExporter.ExportJTableToExcel(tableThongKe);
        }
    }
}