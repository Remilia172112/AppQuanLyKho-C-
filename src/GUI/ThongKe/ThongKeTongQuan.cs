using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; 
using src.BUS;
using src.DAO;
using src.DTO.ThongKe;
using src.Helper;
using Svg; 

namespace src.GUI.ThongKe
{
    public class ThongKeTongQuan : UserControl
    {
        private ThongKeBUS thongkebus;
        private List<ThongKeTungNgayTrongThangDTO> dataset;

        // UI Components
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.Panel pnlChartContainer;
        private DataGridView tableThongKe;
        private Chart chartDoanhThu;

        private string[,] summaryData; 

        public ThongKeTongQuan(ThongKeBUS thongkebus)
        {
            this.thongkebus = thongkebus;
            // Xử lý trường hợp không lấy được dữ liệu thống kê
            try { this.dataset = thongkebus.GetThongKe7NgayGanNhat(); } 
            catch { this.dataset = new List<ThongKeTungNgayTrongThangDTO>(); }
            
            // Lấy số liệu
            string spCount = "0";
            string khCount = "0";
            string nvCount = "0";
            try { spCount = SanPhamDAO.Instance.selectAll().Count.ToString(); } catch {}
            try { khCount = KhachHangDAO.Instance.selectAll().Count.ToString(); } catch {}
            try { nvCount = NhanVienDAO.Instance.selectAll().Count.ToString(); } catch {}

            summaryData = new string[,] {
                {"Sản phẩm trong kho", "brand.svg", spCount},
                {"Khách hàng", "stafff.svg", khCount},
                {"Nhân viên hoạt động", "customerr.svg", nvCount}
            };

            InitComponent();
            LoadDataTable(this.dataset);
            LoadDataChart();
        }

        private void InitComponent()
        {
            this.Size = new Size(1000, 700);
            this.BackColor = Color.FromArgb(240, 247, 250); 
            this.Padding = new Padding(10);

            // --- 1. Panel Top (Các thẻ thống kê) ---
            pnlTop = new System.Windows.Forms.Panel();
            pnlTop.Height = 130;
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Padding = new Padding(0, 0, 0, 10); 
            
            TableLayoutPanel tableTop = new TableLayoutPanel();
            tableTop.Dock = DockStyle.Fill;
            tableTop.ColumnCount = 3;
            tableTop.RowCount = 1;
            // Chia đều 3 cột, mỗi cột 33.33%
            tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));

            for (int i = 0; i < summaryData.GetLength(0); i++)
            {
                System.Windows.Forms.Panel card = CreateSummaryCard(summaryData[i, 0], summaryData[i, 1], summaryData[i, 2]);
                tableTop.Controls.Add(card, i, 0);
            }
            pnlTop.Controls.Add(tableTop);
            this.Controls.Add(pnlTop);

            // --- 2. Panel Center (Biểu đồ) ---
            pnlCenter = new System.Windows.Forms.Panel();
            pnlCenter.Dock = DockStyle.Fill;
            pnlCenter.BackColor = Color.White;
            pnlCenter.Padding = new Padding(10);

            Label lblChartName = new Label();
            lblChartName.Text = "Thống kê doanh thu 7 ngày gần nhất";
            lblChartName.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblChartName.Dock = DockStyle.Top;
            lblChartName.Height = 40;
            lblChartName.TextAlign = ContentAlignment.MiddleLeft;
            pnlCenter.Controls.Add(lblChartName);

            pnlChartContainer = new System.Windows.Forms.Panel();
            pnlChartContainer.Dock = DockStyle.Fill;
            InitChart();
            pnlChartContainer.Controls.Add(chartDoanhThu);
            pnlCenter.Controls.Add(pnlChartContainer);
            
            this.Controls.Add(pnlCenter);

            // --- 3. Panel Bottom (Bảng dữ liệu) ---
            tableThongKe = new DataGridView();
            tableThongKe.Dock = DockStyle.Bottom;
            tableThongKe.Height = 200;
            tableThongKe.AllowUserToAddRows = false;
            tableThongKe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tableThongKe.ReadOnly = true;
            tableThongKe.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tableThongKe.BackgroundColor = Color.White;
            tableThongKe.BorderStyle = BorderStyle.None;

            tableThongKe.Columns.Add("Ngay", "Ngày");
            tableThongKe.Columns.Add("ChiPhi", "Vốn");
            tableThongKe.Columns.Add("DoanhThu", "Doanh thu");
            tableThongKe.Columns.Add("LoiNhuan", "Lợi nhuận");

            tableThongKe.Columns["ChiPhi"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tableThongKe.Columns["DoanhThu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tableThongKe.Columns["LoiNhuan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.Controls.Add(tableThongKe);

            pnlTop.BringToFront();
            tableThongKe.SendToBack();
        }

        // --- HÀM TẠO THẺ THỐNG KÊ (SỬ DỤNG TABLE LAYOUT ĐỂ KHÔNG BỊ MẤT CHỮ) ---
        private System.Windows.Forms.Panel CreateSummaryCard(string title, string iconName, string value)
        {
            // Panel bao ngoài (Card)
            System.Windows.Forms.Panel pnlCard = new System.Windows.Forms.Panel();
            pnlCard.Dock = DockStyle.Fill;
            pnlCard.Margin = new Padding(10); // Khoảng cách giữa các thẻ
            pnlCard.BackColor = Color.White;
            
            // Sử dụng TableLayoutPanel bên trong thẻ để chia cột Icon và Text
            TableLayoutPanel layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.ColumnCount = 2;
            layout.RowCount = 1;
            // Cột 1 (Icon): Cố định 100px
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            // Cột 2 (Text): Chiếm hết phần còn lại (100%)
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            
            // 1. Icon (Bên trái)
            PictureBox pbIcon = new PictureBox();
            pbIcon.Dock = DockStyle.Fill;
            pbIcon.SizeMode = PictureBoxSizeMode.Zoom;
            pbIcon.Margin = new Padding(10); // Căn lề cho icon đẹp
            
            try
            {
                string path = "icon/" + iconName;
                if (System.IO.File.Exists(path))
                {
                    if (path.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
                    {
                        var svgDoc = SvgDocument.Open(path);
                        pbIcon.Image = svgDoc.Draw(80, 80);
                    }
                    else
                    {
                        pbIcon.Image = Image.FromFile(path);
                    }
                }
            }
            catch { }
            
            layout.Controls.Add(pbIcon, 0, 0); // Thêm vào cột 0

            // 2. Text Panel (Bên phải)
            System.Windows.Forms.Panel pnlText = new System.Windows.Forms.Panel();
            pnlText.Dock = DockStyle.Fill;
            pnlText.Padding = new Padding(0, 15, 10, 0); // Padding trên để chữ canh giữa dọc

            // Giá trị (Số to) - Nằm trên
            Label lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblValue.ForeColor = Color.FromArgb(1, 87, 155);
            lblValue.Dock = DockStyle.Top;
            lblValue.Height = 40;
            lblValue.AutoSize = true; // Tự động giãn để không mất chữ

            // Tiêu đề (Chữ nhỏ) - Nằm dưới
            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lblTitle.ForeColor = Color.Gray;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 30;
            lblTitle.AutoSize = true; // Tự động giãn

            // Add vào panel chữ (Thứ tự: Value trước, Title sau sẽ nằm dưới vì Dock Top)
            // Hoặc an toàn hơn dùng FlowLayout cho text
            FlowLayoutPanel flowText = new FlowLayoutPanel();
            flowText.Dock = DockStyle.Fill;
            flowText.FlowDirection = FlowDirection.TopDown;
            flowText.Controls.Add(lblValue);
            flowText.Controls.Add(lblTitle);

            layout.Controls.Add(flowText, 1, 0); // Thêm vào cột 1

            pnlCard.Controls.Add(layout);
            return pnlCard;
        }

        private void InitChart()
        {
            chartDoanhThu = new Chart();
            // QUAN TRỌNG: Fix lỗi height = 0px
            chartDoanhThu.MinimumSize = new Size(100, 100); 
            chartDoanhThu.Dock = DockStyle.Fill;

            ChartArea chartArea = new ChartArea("MainArea");
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chartDoanhThu.ChartAreas.Add(chartArea);

            Legend legend = new Legend("Legend1");
            legend.Docking = Docking.Top;
            legend.Alignment = StringAlignment.Center;
            chartDoanhThu.Legends.Add(legend);

            Series s1 = new Series("Vốn") { ChartType = SeriesChartType.Spline, Color = Color.FromArgb(12, 84, 175), BorderWidth = 3 };
            chartDoanhThu.Series.Add(s1);

            Series s2 = new Series("Doanh thu") { ChartType = SeriesChartType.Spline, Color = Color.FromArgb(54, 4, 143), BorderWidth = 3 };
            chartDoanhThu.Series.Add(s2);

            Series s3 = new Series("Lợi nhuận") { ChartType = SeriesChartType.Spline, Color = Color.FromArgb(211, 84, 0), BorderWidth = 3 };
            chartDoanhThu.Series.Add(s3);
        }

        public void LoadDataChart()
        {
            if (dataset == null) return;
            
            foreach (var s in chartDoanhThu.Series) s.Points.Clear();
            foreach (var i in dataset)
            {
                string label = i.Ngay.ToString("dd/MM");
                chartDoanhThu.Series["Vốn"].Points.AddXY(label, i.Chiphi);
                chartDoanhThu.Series["Doanh thu"].Points.AddXY(label, i.Doanhthu);
                chartDoanhThu.Series["Lợi nhuận"].Points.AddXY(label, i.Loinhuan);
            }
        }

        public void LoadDataTable(List<ThongKeTungNgayTrongThangDTO> list)
        {
            if (list == null) return;

            tableThongKe.Rows.Clear();
            foreach (var i in list)
            {
                tableThongKe.Rows.Add(
                    i.Ngay.ToString("dd/MM/yyyy"),
                    Formater.FormatVND(i.Chiphi),
                    Formater.FormatVND(i.Doanhthu),
                    Formater.FormatVND(i.Loinhuan)
                );
            }
        }
    }
}