using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using src.BUS;
using src.DTO;
using src.DTO.ThongKe;
using src.GUI.Components;
using src.GUI.NghiepVu;

namespace src.GUI
{
    public partial class DashboardForm : Form
    {
        // BUS instances
        private readonly SanPhamBUS sanPhamBUS;
        private readonly KhachHangBUS khachHangBUS;
        private readonly NhaCungCapBUS nhaCungCapBUS;
        private readonly PhieuNhapBUS phieuNhapBUS;
        private readonly PhieuXuatBUS phieuXuatBUS;
        private readonly ThongKeBUS thongKeBUS;

        // Data for chart
        private List<ThongKeTungNgayTrongThangDTO> chartData;

        public DashboardForm()
        {
            InitializeComponent();

            // Initialize BUS
            sanPhamBUS = new SanPhamBUS();
            khachHangBUS = new KhachHangBUS();
            nhaCungCapBUS = new NhaCungCapBUS();
            phieuNhapBUS = new PhieuNhapBUS();
            phieuXuatBUS = new PhieuXuatBUS();
            thongKeBUS = new ThongKeBUS();

            // Setup events
            SetupEvents();

            // Load data
            LoadDashboardData();

            // Update welcome message
            UpdateWelcomeMessage();
        }

        private void SetupEvents()
        {
            // Timer for datetime update
            timerDateTime.Tick += TimerDateTime_Tick;
            timerDateTime.Start();

            // Card click events
            pnlCardSanPham.Click += (s, e) => NavigateToForm("sanpham");
            pnlCardKhachHang.Click += (s, e) => NavigateToForm("khachhang");
            pnlCardPhieuNhap.Click += (s, e) => NavigateToForm("nhaphang");
            pnlCardPhieuXuat.Click += (s, e) => NavigateToForm("xuathang");
            pnlCardDoanhThu.Click += (s, e) => NavigateToForm("thongke");
            pnlCardTonKho.Click += (s, e) => NavigateToForm("thongke");

            // Add hover effect for cards
            AddCardHoverEffect(pnlCardSanPham, Color.FromArgb(52, 152, 219));
            AddCardHoverEffect(pnlCardKhachHang, Color.FromArgb(46, 204, 113));
            AddCardHoverEffect(pnlCardPhieuNhap, Color.FromArgb(155, 89, 182));
            AddCardHoverEffect(pnlCardPhieuXuat, Color.FromArgb(230, 126, 34));
            AddCardHoverEffect(pnlCardDoanhThu, Color.FromArgb(231, 76, 60));
            AddCardHoverEffect(pnlCardTonKho, Color.FromArgb(52, 73, 94));

            // Quick action buttons
            btnQuickNhap.Click += BtnQuickNhap_Click;
            btnQuickXuat.Click += BtnQuickXuat_Click;
            btnQuickKiemKe.Click += BtnQuickKiemKe_Click;
            btnQuickThongKe.Click += BtnQuickThongKe_Click;

            // Remove button borders
            btnQuickNhap.FlatAppearance.BorderSize = 0;
            btnQuickXuat.FlatAppearance.BorderSize = 0;
            btnQuickKiemKe.FlatAppearance.BorderSize = 0;
            btnQuickThongKe.FlatAppearance.BorderSize = 0;

            // Chart paint event
            pnlChartContainer.Paint += PnlChartContainer_Paint;

            // Form resize event
            this.Resize += DashboardForm_Resize;
        }

        private void AddCardHoverEffect(Panel card, Color originalColor)
        {
            Color hoverColor = ControlPaint.Light(originalColor, 0.1f);

            card.MouseEnter += (s, e) => card.BackColor = hoverColor;
            card.MouseLeave += (s, e) => card.BackColor = originalColor;

            // Apply to child controls as well
            foreach (Control ctrl in card.Controls)
            {
                ctrl.MouseEnter += (s, e) => card.BackColor = hoverColor;
                ctrl.MouseLeave += (s, e) => card.BackColor = originalColor;
            }
        }

        private void TimerDateTime_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            CultureInfo viCulture = new CultureInfo("vi-VN");
            string dayOfWeek = DateTime.Now.ToString("dddd", viCulture);
            dayOfWeek = char.ToUpper(dayOfWeek[0]) + dayOfWeek.Substring(1);
            lblDateTime.Text = $"{dayOfWeek}, {DateTime.Now:dd/MM/yyyy} - {DateTime.Now:HH:mm:ss}";
        }

        private void UpdateWelcomeMessage()
        {
            string userName = SessionManager.CurrentEmployee?.HOTEN ?? "Người dùng";
            string greeting = GetGreeting();
            lblWelcome.Text = $"{greeting}, {userName}!";
        }

        private string GetGreeting()
        {
            int hour = DateTime.Now.Hour;
            if (hour < 12) return "Chào buổi sáng";
            if (hour < 18) return "Chào buổi chiều";
            return "Chào buổi tối";
        }

        private void LoadDashboardData()
        {
            try
            {
                // Load card data
                LoadCardData();

                // Load chart data
                LoadChartData();

                // Load recent activity
                LoadRecentActivity();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi LoadDashboardData: {ex.Message}");
            }
        }

        private void LoadCardData()
        {
            try
            {
                // Sản phẩm
                var sanPhamList = sanPhamBUS.GetAll();
                lblSanPhamValue.Text = sanPhamList.Count.ToString("N0");

                // Khách hàng
                var khachHangList = khachHangBUS.GetAll();
                lblKhachHangValue.Text = khachHangList.Count.ToString("N0");

                // Phiếu nhập (chờ duyệt - TT=2)
                var phieuNhapList = phieuNhapBUS.GetAllList();
                int phieuNhapChoDuyet = phieuNhapList.Count(p => p.TT == 2);
                lblPhieuNhapValue.Text = phieuNhapChoDuyet.ToString("N0");
                lblPhieuNhapTitle.Text = "Phiếu nhập (chờ)";

                // Phiếu xuất (chờ duyệt - TT=2)
                var phieuXuatList = phieuXuatBUS.GetAll();
                int phieuXuatChoDuyet = phieuXuatList.Count(p => p.TT == 2);
                lblPhieuXuatValue.Text = phieuXuatChoDuyet.ToString("N0");
                lblPhieuXuatTitle.Text = "Phiếu xuất (chờ)";

                // Doanh thu tháng này
                var thongKeThang = thongKeBUS.GetThongKeTheoThang(DateTime.Now.Year);
                var thangHienTai = thongKeThang.FirstOrDefault(t => t.Thang == DateTime.Now.Month);
                long doanhThu = thangHienTai?.Doanhthu ?? 0;
                lblDoanhThuValue.Text = FormatCurrency(doanhThu);

                // Tồn kho
                int tongTonKho = sanPhamBUS.GetQuantity();
                lblTonKhoValue.Text = tongTonKho.ToString("N0");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi LoadCardData: {ex.Message}");
            }
        }

        private void LoadChartData()
        {
            try
            {
                chartData = thongKeBUS.GetThongKe7NgayGanNhat();
                pnlChartContainer.Invalidate(); // Trigger repaint
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi LoadChartData: {ex.Message}");
                chartData = new List<ThongKeTungNgayTrongThangDTO>();
            }
        }

        private void LoadRecentActivity()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Loại", typeof(string));
                dt.Columns.Add("Mã phiếu", typeof(string));
                dt.Columns.Add("Thời gian", typeof(string));
                dt.Columns.Add("Số tiền", typeof(string));
                dt.Columns.Add("Trạng thái", typeof(string));

                // Get recent phiếu nhập
                var phieuNhapRecent = phieuNhapBUS.GetAllList()
                    .OrderByDescending(p => p.TG)
                    .Take(5)
                    .ToList();

                foreach (var pn in phieuNhapRecent)
                {
                    string trangThai = pn.TT switch
                    {
                        0 => "Đã hủy",
                        1 => "Đã duyệt",
                        2 => "Chờ duyệt",
                        _ => "N/A"
                    };
                    dt.Rows.Add("📥 Nhập", $"PN{pn.MPN:D4}", pn.TG.ToString("dd/MM/yyyy HH:mm"), FormatCurrency(pn.TIEN), trangThai);
                }

                // Get recent phiếu xuất
                var phieuXuatRecent = phieuXuatBUS.GetAll()
                    .OrderByDescending(p => p.TG)
                    .Take(5)
                    .ToList();

                foreach (var px in phieuXuatRecent)
                {
                    string trangThai = px.TT switch
                    {
                        0 => "Đã hủy",
                        1 => "Đã duyệt",
                        2 => "Chờ duyệt",
                        _ => "N/A"
                    };
                    dt.Rows.Add("📤 Xuất", $"PX{px.MPX:D4}", px.TG.ToString("dd/MM/yyyy HH:mm"), FormatCurrency(px.TIEN), trangThai);
                }

                // Sort by time
                DataView dv = dt.DefaultView;
                dv.Sort = "Thời gian DESC";
                dgvRecentActivity.DataSource = dv.ToTable();

                // Style the grid
                StyleDataGridView();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi LoadRecentActivity: {ex.Message}");
            }
        }

        private void StyleDataGridView()
        {
            if (dgvRecentActivity.Columns.Count > 0)
            {
                dgvRecentActivity.Columns["Loại"].Width = 100;
                dgvRecentActivity.Columns["Mã phiếu"].Width = 100;
                dgvRecentActivity.Columns["Thời gian"].Width = 150;
                dgvRecentActivity.Columns["Số tiền"].Width = 120;
                dgvRecentActivity.Columns["Trạng thái"].Width = 100;
            }
        }

        private void PnlChartContainer_Paint(object sender, PaintEventArgs e)
        {
            DrawBarChart(e.Graphics, pnlChartContainer.ClientRectangle);
        }

        private void DrawBarChart(Graphics g, Rectangle bounds)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            if (chartData == null || chartData.Count == 0)
            {
                using (Font font = new Font("Segoe UI", 12F))
                using (Brush brush = new SolidBrush(Color.Gray))
                {
                    g.DrawString("Không có dữ liệu", font, brush, bounds.Width / 2 - 50, bounds.Height / 2);
                }
                return;
            }

            int padding = 60;
            int chartWidth = bounds.Width - padding * 2;
            int chartHeight = bounds.Height - padding * 2;
            int barWidth = chartWidth / (chartData.Count * 2 + 1);
            int spacing = barWidth / 2;

            // Find max value for scaling
            long maxValue = chartData.Max(d => Math.Max(d.Doanhthu, d.Chiphi));
            if (maxValue == 0) maxValue = 1;

            // Draw axes
            using (Pen axisPen = new Pen(Color.FromArgb(200, 200, 200), 1))
            {
                // Y axis
                g.DrawLine(axisPen, padding, padding - 10, padding, bounds.Height - padding);
                // X axis
                g.DrawLine(axisPen, padding, bounds.Height - padding, bounds.Width - padding, bounds.Height - padding);
            }

            // Draw grid lines
            using (Pen gridPen = new Pen(Color.FromArgb(230, 230, 230), 1))
            {
                for (int i = 1; i <= 5; i++)
                {
                    int y = bounds.Height - padding - (chartHeight * i / 5);
                    g.DrawLine(gridPen, padding, y, bounds.Width - padding, y);

                    // Y axis labels
                    using (Font labelFont = new Font("Segoe UI", 8F))
                    using (Brush labelBrush = new SolidBrush(Color.Gray))
                    {
                        string label = FormatShortCurrency(maxValue * i / 5);
                        g.DrawString(label, labelFont, labelBrush, 5, y - 8);
                    }
                }
            }

            // Draw bars
            using (Brush doanhThuBrush = new SolidBrush(Color.FromArgb(46, 204, 113)))
            using (Brush chiPhiBrush = new SolidBrush(Color.FromArgb(231, 76, 60)))
            using (Font labelFont = new Font("Segoe UI", 8F))
            using (Brush textBrush = new SolidBrush(Color.FromArgb(100, 100, 100)))
            {
                for (int i = 0; i < chartData.Count; i++)
                {
                    var data = chartData[i];
                    int x = padding + spacing + i * (barWidth * 2 + spacing);

                    // Draw doanh thu bar
                    int doanhThuHeight = (int)((double)data.Doanhthu / maxValue * chartHeight);
                    Rectangle doanhThuRect = new Rectangle(
                        x,
                        bounds.Height - padding - doanhThuHeight,
                        barWidth,
                        doanhThuHeight);

                    using (GraphicsPath path = CreateRoundedRectangle(doanhThuRect, 3))
                    {
                        g.FillPath(doanhThuBrush, path);
                    }

                    // Draw chi phí bar
                    int chiPhiHeight = (int)((double)data.Chiphi / maxValue * chartHeight);
                    Rectangle chiPhiRect = new Rectangle(
                        x + barWidth + 2,
                        bounds.Height - padding - chiPhiHeight,
                        barWidth,
                        chiPhiHeight);

                    using (GraphicsPath path = CreateRoundedRectangle(chiPhiRect, 3))
                    {
                        g.FillPath(chiPhiBrush, path);
                    }

                    // X axis label (date)
                    string dateLabel = data.Ngay.ToString("dd/MM");
                    SizeF labelSize = g.MeasureString(dateLabel, labelFont);
                    g.DrawString(dateLabel, labelFont, textBrush,
                        x + barWidth - labelSize.Width / 2,
                        bounds.Height - padding + 5);
                }
            }

            // Draw legend
            DrawLegend(g, bounds);
        }

        private void DrawLegend(Graphics g, Rectangle bounds)
        {
            int legendX = bounds.Width - 200;
            int legendY = 10;

            using (Font legendFont = new Font("Segoe UI", 9F))
            using (Brush textBrush = new SolidBrush(Color.FromArgb(60, 60, 60)))
            using (Brush doanhThuBrush = new SolidBrush(Color.FromArgb(46, 204, 113)))
            using (Brush chiPhiBrush = new SolidBrush(Color.FromArgb(231, 76, 60)))
            {
                // Doanh thu legend
                g.FillRectangle(doanhThuBrush, legendX, legendY, 15, 15);
                g.DrawString("Doanh thu", legendFont, textBrush, legendX + 20, legendY);

                // Chi phí legend
                g.FillRectangle(chiPhiBrush, legendX + 100, legendY, 15, 15);
                g.DrawString("Chi phí", legendFont, textBrush, legendX + 120, legendY);
            }
        }

        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (rect.Height <= 0 || rect.Width <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();

            return path;
        }

        private string FormatCurrency(long amount)
        {
            if (amount >= 1000000000)
                return $"{amount / 1000000000.0:F1}B đ";
            if (amount >= 1000000)
                return $"{amount / 1000000.0:F1}M đ";
            if (amount >= 1000)
                return $"{amount / 1000.0:F0}K đ";
            return $"{amount:N0} đ";
        }

        private string FormatShortCurrency(long amount)
        {
            if (amount >= 1000000000)
                return $"{amount / 1000000000.0:F1}B";
            if (amount >= 1000000)
                return $"{amount / 1000000.0:F1}M";
            if (amount >= 1000)
                return $"{amount / 1000.0:F0}K";
            return amount.ToString("N0");
        }

        private void NavigateToForm(string formType)
        {
            // This will be handled by MainForm
            // For now, just show a message
            MessageBox.Show($"Chuyển đến: {formType}", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnQuickNhap_Click(object sender, EventArgs e)
        {
            if (SessionManager.CanView("nhaphang"))
            {
                // Open PhieuNhapForm
                PhieuNhapForm form = new PhieuNhapForm();
                OpenFormInParent(form);
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnQuickXuat_Click(object sender, EventArgs e)
        {
            if (SessionManager.CanView("xuathang"))
            {
                PhieuXuatForm form = new PhieuXuatForm();
                OpenFormInParent(form);
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnQuickKiemKe_Click(object sender, EventArgs e)
        {
            if (SessionManager.CanView("kiemke"))
            {
                PhieuKiemKeForm form = new PhieuKiemKeForm();
                OpenFormInParent(form);
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnQuickThongKe_Click(object sender, EventArgs e)
        {
            if (SessionManager.CanView("thongke"))
            {
                ThongKe.ThongKe form = new ThongKe.ThongKe();
                OpenFormInParent(form);
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OpenFormInParent(Form childForm)
        {
            // Find parent MainForm and open the form in content panel
            if (this.Parent is Panel contentPanel)
            {
                contentPanel.Controls.Clear();

                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Location = new Point(0, 0);
                childForm.Size = contentPanel.ClientSize;
                childForm.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

                contentPanel.Controls.Add(childForm);
                childForm.Show();
            }
        }

        private void DashboardForm_Resize(object sender, EventArgs e)
        {
            // Redraw chart on resize
            pnlChartContainer?.Invalidate();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateDateTime();

            // Apply rounded corners to cards
            ApplyRoundedCorners();
        }

        private void ApplyRoundedCorners()
        {
            int radius = 10;

            SetRoundedRegion(pnlCardSanPham, radius);
            SetRoundedRegion(pnlCardKhachHang, radius);
            SetRoundedRegion(pnlCardPhieuNhap, radius);
            SetRoundedRegion(pnlCardPhieuXuat, radius);
            SetRoundedRegion(pnlCardDoanhThu, radius);
            SetRoundedRegion(pnlCardTonKho, radius);
            SetRoundedRegion(pnlQuickActions, radius);
            SetRoundedRegion(pnlCharts, radius);
            SetRoundedRegion(pnlRecentActivity, radius);
        }

        private void SetRoundedRegion(Panel panel, int radius)
        {
            try
            {
                GraphicsPath path = new GraphicsPath();
                Rectangle rect = new Rectangle(0, 0, panel.Width, panel.Height);

                path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
                path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
                path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                path.CloseFigure();

                panel.Region = new Region(path);
            }
            catch { }
        }

        /// <summary>
        /// Refresh dashboard data
        /// </summary>
        public void RefreshData()
        {
            LoadDashboardData();
        }
    }
}
