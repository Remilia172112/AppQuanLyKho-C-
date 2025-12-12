using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; // Thư viện Chart
using src.BUS;
using src.DTO.ThongKe;
using src.Helper;

namespace src.GUI.ThongKe
{
    public class ThongKeDoanhThuTungNam : UserControl
    {
        private ThongKeBUS thongkeBUS;
        private List<ThongKeDoanhThuDTO> dataset;
        private int currentYear;

        // UI Components
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlChartContainer;
        private DataGridView tableThongKe;
        private TextBox txtYearStart;
        private TextBox txtYearEnd;
        private Button btnThongKe;
        private Button btnReset;
        private Button btnExport;
        private Chart chartDoanhThu;

        public ThongKeDoanhThuTungNam(ThongKeBUS thongkeBUS)
        {
            this.thongkeBUS = thongkeBUS;
            this.currentYear = DateTime.Now.Year;
            // Load 5 năm gần nhất
            this.dataset = this.thongkeBUS.GetDoanhThuTheoTungNam(currentYear - 5, currentYear);
            
            InitComponent();
            LoadData(this.dataset);
        }

        private void InitComponent()
        {
            this.Size = new Size(1000, 700);
            this.BackColor = Color.White;
            this.Padding = new Padding(10);

            // --- 1. Panel Top (Input) ---
            pnlTop = new System.Windows.Forms.Panel();
            pnlTop.Height = 50;
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Padding = new Padding(5);

            FlowLayoutPanel flowTop = new FlowLayoutPanel();
            flowTop.Dock = DockStyle.Fill;
            flowTop.FlowDirection = FlowDirection.LeftToRight;
            flowTop.AutoSize = true;

            // Labels & TextBoxes
            Label lblFrom = new Label { Text = "Từ năm:", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(5, 8, 5, 5) };
            txtYearStart = new TextBox { Width = 80 };
            txtYearStart.KeyPress += TxtYear_KeyPress; // Chỉ cho nhập số

            Label lblTo = new Label { Text = "Đến năm:", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(15, 8, 5, 5) };
            txtYearEnd = new TextBox { Width = 80 };
            txtYearEnd.KeyPress += TxtYear_KeyPress;

            // Buttons
            btnThongKe = new Button { Text = "Thống kê", AutoSize = true, Cursor = Cursors.Hand };
            btnReset = new Button { Text = "Làm mới", AutoSize = true, Cursor = Cursors.Hand };
            btnExport = new Button { Text = "Xuất Excel", AutoSize = true, Cursor = Cursors.Hand };

            // Events
            btnThongKe.Click += BtnThongKe_Click;
            btnReset.Click += BtnReset_Click;
            btnExport.Click += BtnExport_Click;

            flowTop.Controls.Add(lblFrom);
            flowTop.Controls.Add(txtYearStart);
            flowTop.Controls.Add(lblTo);
            flowTop.Controls.Add(txtYearEnd);
            flowTop.Controls.Add(btnThongKe);
            flowTop.Controls.Add(btnReset);
            flowTop.Controls.Add(btnExport);

            pnlTop.Controls.Add(flowTop);
            this.Controls.Add(pnlTop);

            // --- 2. DataGridView (Table) ---
            tableThongKe = new DataGridView();
            tableThongKe.Dock = DockStyle.Bottom;
            tableThongKe.Height = 250;
            tableThongKe.AllowUserToAddRows = false;
            tableThongKe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tableThongKe.ReadOnly = true;
            tableThongKe.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tableThongKe.BackgroundColor = Color.White;

            tableThongKe.Columns.Add("Nam", "Năm");
            tableThongKe.Columns.Add("Von", "Vốn");
            tableThongKe.Columns.Add("DoanhThu", "Doanh thu");
            tableThongKe.Columns.Add("LoiNhuan", "Lợi nhuận");

            // Format cột tiền tệ
            tableThongKe.Columns["Von"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tableThongKe.Columns["DoanhThu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tableThongKe.Columns["LoiNhuan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.Controls.Add(tableThongKe);

            // --- 3. Chart (Biểu đồ) ---
            pnlChartContainer = new System.Windows.Forms.Panel();
            pnlChartContainer.Dock = DockStyle.Fill;
            pnlChartContainer.Padding = new Padding(0, 10, 0, 10);

            InitChart();
            pnlChartContainer.Controls.Add(chartDoanhThu);

            this.Controls.Add(pnlChartContainer);

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

        private void LoadData(List<ThongKeDoanhThuDTO> list)
        {
            // 1. Update Chart
            foreach (var s in chartDoanhThu.Series) s.Points.Clear();

            foreach (var item in list)
            {
                string label = "Năm " + item.Thoigian;
                chartDoanhThu.Series["Vốn"].Points.AddXY(label, item.Von);
                chartDoanhThu.Series["Doanh thu"].Points.AddXY(label, item.Doanhthu);
                chartDoanhThu.Series["Lợi nhuận"].Points.AddXY(label, item.Loinhuan);
            }

            // 2. Update Table
            tableThongKe.Rows.Clear();
            foreach (var item in list)
            {
                tableThongKe.Rows.Add(
                    item.Thoigian,
                    Formater.FormatVND(item.Von),
                    Formater.FormatVND(item.Doanhthu),
                    Formater.FormatVND(item.Loinhuan)
                );
            }
        }

        // Sự kiện chỉ cho nhập số
        private void TxtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void BtnThongKe_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtYearStart.Text) && !string.IsNullOrEmpty(txtYearEnd.Text))
            {
                int nambd = int.Parse(txtYearStart.Text);
                int namkt = int.Parse(txtYearEnd.Text);

                if (nambd > currentYear || namkt > currentYear)
                {
                    MessageBox.Show("Năm không được lớn hơn năm hiện tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (namkt < nambd || namkt <= 2015 || nambd <= 2015)
                {
                    MessageBox.Show("Năm kết thúc không được bé hơn năm bắt đầu và phải lớn hơn 2015", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    this.dataset = this.thongkeBUS.GetDoanhThuTheoTungNam(nambd, namkt);
                    LoadData(this.dataset);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ năm bắt đầu và kết thúc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            txtYearStart.Text = "";
            txtYearEnd.Text = "";
            // Reset về 5 năm gần nhất
            this.dataset = this.thongkeBUS.GetDoanhThuTheoTungNam(currentYear - 5, currentYear);
            LoadData(this.dataset);
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            TableExporter.ExportTableToExcel(tableThongKe, "TKDTTN");
        }
    }
}