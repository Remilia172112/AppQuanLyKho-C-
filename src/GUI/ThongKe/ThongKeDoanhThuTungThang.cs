using System.Windows.Forms.DataVisualization.Charting; // Thư viện Chart
using src.BUS;
using src.DTO.ThongKe;
using src.Helper;

namespace src.GUI.ThongKe
{
    public class ThongKeDoanhThuTungThang : UserControl
    {
        private ThongKeBUS thongkeBUS;
        private List<ThongKeTheoThangDTO> dataset;

        // UI Components
        private Panel pnlTop;
        private Panel pnlChartContainer;
        private DataGridView tableThongKe;
        private ComboBox cboNam;
        private Button btnExport;
        private Chart chartDoanhThu;

        public ThongKeDoanhThuTungThang(ThongKeBUS thongkeBUS)
        {
            this.thongkeBUS = thongkeBUS;
            InitComponent();
            
            // Load dữ liệu mặc định (Năm hiện tại)
            int currentYear = DateTime.Now.Year;
            cboNam.SelectedItem = currentYear;
            LoadThongKeThang(currentYear);
        }

        private void InitComponent()
        {
            Size = new Size(1000, 700);
            BackColor = Color.White;
            Padding = new Padding(10);

            // --- 1. Panel Top (Chọn năm) ---
            pnlTop = new Panel();
            pnlTop.Height = 50;
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Padding = new Padding(5);

            FlowLayoutPanel flowTop = new FlowLayoutPanel();
            flowTop.Dock = DockStyle.Fill;
            flowTop.FlowDirection = FlowDirection.LeftToRight;
            flowTop.AutoSize = true;

            // Label & ComboBox Năm
            Label lblChonNam = new Label { Text = "Chọn năm thống kê:", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(5, 8, 5, 5) };
            
            cboNam = new ComboBox { Width = 100, DropDownStyle = ComboBoxStyle.DropDownList };
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 10; i <= currentYear + 10; i++) cboNam.Items.Add(i);
            
            // Sự kiện chọn năm -> Load lại dữ liệu
            cboNam.SelectedIndexChanged += CboNam_SelectedIndexChanged;

            // Button Export
            btnExport = new Button { Text = "Xuất Excel", AutoSize = true, Cursor = Cursors.Hand };
            btnExport.Click += BtnExport_Click;

            flowTop.Controls.Add(lblChonNam);
            flowTop.Controls.Add(cboNam);
            flowTop.Controls.Add(btnExport);

            pnlTop.Controls.Add(flowTop);
            Controls.Add(pnlTop);

            // --- 2. DataGridView (Table) ---
            tableThongKe = new DataGridView();
            tableThongKe.Dock = DockStyle.Bottom;
            tableThongKe.Height = 250;
            tableThongKe.AllowUserToAddRows = false;
            tableThongKe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tableThongKe.ReadOnly = true;
            tableThongKe.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tableThongKe.BackgroundColor = Color.White;

            tableThongKe.Columns.Add("Thang", "Tháng");
            tableThongKe.Columns.Add("ChiPhi", "Chi phí");
            tableThongKe.Columns.Add("DoanhThu", "Doanh thu");
            tableThongKe.Columns.Add("LoiNhuan", "Lợi nhuận");

            // Format cột tiền tệ
            tableThongKe.Columns["ChiPhi"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tableThongKe.Columns["DoanhThu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tableThongKe.Columns["LoiNhuan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            Controls.Add(tableThongKe);

            // --- 3. Chart Area ---
            pnlChartContainer = new System.Windows.Forms.Panel();
            pnlChartContainer.Dock = DockStyle.Fill;
            pnlChartContainer.Padding = new Padding(0, 10, 0, 10);

            InitChart();
            pnlChartContainer.Controls.Add(chartDoanhThu);

            Controls.Add(pnlChartContainer);

            // Z-Order
            pnlTop.BringToFront();
            tableThongKe.SendToBack();
        }

        private void InitChart()
        {
            chartDoanhThu = new Chart();
            chartDoanhThu.MinimumSize = new Size(100, 100);
            chartDoanhThu.Dock = DockStyle.Fill;

            ChartArea chartArea = new ChartArea("MainArea");
            chartDoanhThu.ChartAreas.Add(chartArea);

            Legend legend = new Legend("Legend1");
            legend.Docking = Docking.Top;
            chartDoanhThu.Legends.Add(legend);

            // Series 1: Vốn
            Series s1 = new Series("Vốn") { ChartType = SeriesChartType.Column, Color = Color.FromArgb(245, 189, 135) };
            chartDoanhThu.Series.Add(s1);

            // Series 2: Doanh thu
            Series s2 = new Series("Doanh thu") { ChartType = SeriesChartType.Column, Color = Color.FromArgb(135, 189, 245) };
            chartDoanhThu.Series.Add(s2);

            // Series 3: Lợi nhuận
            Series s3 = new Series("Lợi nhuận") { ChartType = SeriesChartType.Column, Color = Color.FromArgb(189, 135, 245) };
            chartDoanhThu.Series.Add(s3);
        }

        public void LoadThongKeThang(int nam)
        {
            var list = thongkeBUS.GetThongKeTheoThang(nam);

            // 1. Update Chart
            foreach (var s in chartDoanhThu.Series) s.Points.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                string label = "Tháng " + (i + 1);
                chartDoanhThu.Series["Vốn"].Points.AddXY(label, list[i].Chiphi);
                chartDoanhThu.Series["Doanh thu"].Points.AddXY(label, list[i].Doanhthu);
                chartDoanhThu.Series["Lợi nhuận"].Points.AddXY(label, list[i].Loinhuan);
            }

            // 2. Update Table
            tableThongKe.Rows.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                tableThongKe.Rows.Add(
                    "Tháng " + (i + 1),
                    Formater.FormatVND(list[i].Chiphi),
                    Formater.FormatVND(list[i].Doanhthu),
                    Formater.FormatVND(list[i].Loinhuan)
                );
            }
        }

        private void CboNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboNam.SelectedItem != null)
            {
                int nam = (int)cboNam.SelectedItem;
                LoadThongKeThang(nam);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            TableExporter.ExportTableToExcel(tableThongKe, "TKDTTM");
        }
    }
}