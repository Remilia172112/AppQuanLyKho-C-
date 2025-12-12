namespace src.GUI
{
    partial class DashboardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Main layout panel
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlCards = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlCharts = new System.Windows.Forms.Panel();
            this.pnlRecentActivity = new System.Windows.Forms.Panel();

            // Header controls
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.timerDateTime = new System.Windows.Forms.Timer(this.components);

            // Stat cards
            this.pnlCardSanPham = new System.Windows.Forms.Panel();
            this.pnlCardKhachHang = new System.Windows.Forms.Panel();
            this.pnlCardPhieuNhap = new System.Windows.Forms.Panel();
            this.pnlCardPhieuXuat = new System.Windows.Forms.Panel();
            this.pnlCardDoanhThu = new System.Windows.Forms.Panel();
            this.pnlCardTonKho = new System.Windows.Forms.Panel();

            // Labels for cards
            this.lblSanPhamTitle = new System.Windows.Forms.Label();
            this.lblSanPhamValue = new System.Windows.Forms.Label();
            this.lblSanPhamIcon = new System.Windows.Forms.Label();

            this.lblKhachHangTitle = new System.Windows.Forms.Label();
            this.lblKhachHangValue = new System.Windows.Forms.Label();
            this.lblKhachHangIcon = new System.Windows.Forms.Label();

            this.lblPhieuNhapTitle = new System.Windows.Forms.Label();
            this.lblPhieuNhapValue = new System.Windows.Forms.Label();
            this.lblPhieuNhapIcon = new System.Windows.Forms.Label();

            this.lblPhieuXuatTitle = new System.Windows.Forms.Label();
            this.lblPhieuXuatValue = new System.Windows.Forms.Label();
            this.lblPhieuXuatIcon = new System.Windows.Forms.Label();

            this.lblDoanhThuTitle = new System.Windows.Forms.Label();
            this.lblDoanhThuValue = new System.Windows.Forms.Label();
            this.lblDoanhThuIcon = new System.Windows.Forms.Label();

            this.lblTonKhoTitle = new System.Windows.Forms.Label();
            this.lblTonKhoValue = new System.Windows.Forms.Label();
            this.lblTonKhoIcon = new System.Windows.Forms.Label();

            // Charts panel
            this.lblChartTitle = new System.Windows.Forms.Label();
            this.pnlChartContainer = new System.Windows.Forms.Panel();

            // Recent activity
            this.lblRecentTitle = new System.Windows.Forms.Label();
            this.dgvRecentActivity = new System.Windows.Forms.DataGridView();

            // Quick actions
            this.pnlQuickActions = new System.Windows.Forms.Panel();
            this.lblQuickActionsTitle = new System.Windows.Forms.Label();
            this.btnQuickNhap = new System.Windows.Forms.Button();
            this.btnQuickXuat = new System.Windows.Forms.Button();
            this.btnQuickKiemKe = new System.Windows.Forms.Button();
            this.btnQuickThongKe = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentActivity)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlCards.SuspendLayout();
            this.pnlCardSanPham.SuspendLayout();
            this.pnlCardKhachHang.SuspendLayout();
            this.pnlCardPhieuNhap.SuspendLayout();
            this.pnlCardPhieuXuat.SuspendLayout();
            this.pnlCardDoanhThu.SuspendLayout();
            this.pnlCardTonKho.SuspendLayout();
            this.pnlCharts.SuspendLayout();
            this.pnlRecentActivity.SuspendLayout();
            this.pnlQuickActions.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Padding = new System.Windows.Forms.Padding(20);
            this.pnlMain.Controls.Add(this.pnlRecentActivity);
            this.pnlMain.Controls.Add(this.pnlCharts);
            this.pnlMain.Controls.Add(this.pnlQuickActions);
            this.pnlMain.Controls.Add(this.pnlCards);
            this.pnlMain.Controls.Add(this.pnlHeader);

            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 80;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(10);
            this.pnlHeader.Controls.Add(this.lblDateTime);
            this.pnlHeader.Controls.Add(this.lblWelcome);

            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblWelcome.Location = new System.Drawing.Point(10, 10);
            this.lblWelcome.Text = "Chào mừng trở lại!";

            // 
            // lblDateTime
            // 
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblDateTime.ForeColor = System.Drawing.Color.FromArgb(127, 140, 141);
            this.lblDateTime.Location = new System.Drawing.Point(10, 55);
            this.lblDateTime.Text = "Thứ Năm, 12/12/2024 - 10:30:00";

            // 
            // timerDateTime
            // 
            this.timerDateTime.Interval = 1000;
            this.timerDateTime.Enabled = true;

            // 
            // pnlCards
            // 
            this.pnlCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCards.Height = 140;
            this.pnlCards.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.pnlCards.AutoSize = false;
            this.pnlCards.WrapContents = false;
            this.pnlCards.Controls.Add(this.pnlCardSanPham);
            this.pnlCards.Controls.Add(this.pnlCardKhachHang);
            this.pnlCards.Controls.Add(this.pnlCardPhieuNhap);
            this.pnlCards.Controls.Add(this.pnlCardPhieuXuat);
            this.pnlCards.Controls.Add(this.pnlCardDoanhThu);
            this.pnlCards.Controls.Add(this.pnlCardTonKho);

            // 
            // pnlCardSanPham - Card Sản phẩm
            // 
            this.pnlCardSanPham.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.pnlCardSanPham.Size = new System.Drawing.Size(180, 110);
            this.pnlCardSanPham.Margin = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.pnlCardSanPham.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlCardSanPham.Controls.Add(this.lblSanPhamIcon);
            this.pnlCardSanPham.Controls.Add(this.lblSanPhamValue);
            this.pnlCardSanPham.Controls.Add(this.lblSanPhamTitle);

            this.lblSanPhamIcon.Text = "📦";
            this.lblSanPhamIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 28F);
            this.lblSanPhamIcon.ForeColor = System.Drawing.Color.White;
            this.lblSanPhamIcon.Location = new System.Drawing.Point(120, 30);
            this.lblSanPhamIcon.AutoSize = true;

            this.lblSanPhamValue.Text = "0";
            this.lblSanPhamValue.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblSanPhamValue.ForeColor = System.Drawing.Color.White;
            this.lblSanPhamValue.Location = new System.Drawing.Point(15, 25);
            this.lblSanPhamValue.AutoSize = true;

            this.lblSanPhamTitle.Text = "Sản phẩm";
            this.lblSanPhamTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSanPhamTitle.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.lblSanPhamTitle.Location = new System.Drawing.Point(15, 75);
            this.lblSanPhamTitle.AutoSize = true;

            // 
            // pnlCardKhachHang - Card Khách hàng
            // 
            this.pnlCardKhachHang.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.pnlCardKhachHang.Size = new System.Drawing.Size(180, 110);
            this.pnlCardKhachHang.Margin = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.pnlCardKhachHang.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlCardKhachHang.Controls.Add(this.lblKhachHangIcon);
            this.pnlCardKhachHang.Controls.Add(this.lblKhachHangValue);
            this.pnlCardKhachHang.Controls.Add(this.lblKhachHangTitle);

            this.lblKhachHangIcon.Text = "👥";
            this.lblKhachHangIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 28F);
            this.lblKhachHangIcon.ForeColor = System.Drawing.Color.White;
            this.lblKhachHangIcon.Location = new System.Drawing.Point(120, 30);
            this.lblKhachHangIcon.AutoSize = true;

            this.lblKhachHangValue.Text = "0";
            this.lblKhachHangValue.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblKhachHangValue.ForeColor = System.Drawing.Color.White;
            this.lblKhachHangValue.Location = new System.Drawing.Point(15, 25);
            this.lblKhachHangValue.AutoSize = true;

            this.lblKhachHangTitle.Text = "Khách hàng";
            this.lblKhachHangTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblKhachHangTitle.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.lblKhachHangTitle.Location = new System.Drawing.Point(15, 75);
            this.lblKhachHangTitle.AutoSize = true;

            // 
            // pnlCardPhieuNhap - Card Phiếu nhập
            // 
            this.pnlCardPhieuNhap.BackColor = System.Drawing.Color.FromArgb(155, 89, 182);
            this.pnlCardPhieuNhap.Size = new System.Drawing.Size(180, 110);
            this.pnlCardPhieuNhap.Margin = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.pnlCardPhieuNhap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlCardPhieuNhap.Controls.Add(this.lblPhieuNhapIcon);
            this.pnlCardPhieuNhap.Controls.Add(this.lblPhieuNhapValue);
            this.pnlCardPhieuNhap.Controls.Add(this.lblPhieuNhapTitle);

            this.lblPhieuNhapIcon.Text = "📥";
            this.lblPhieuNhapIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 28F);
            this.lblPhieuNhapIcon.ForeColor = System.Drawing.Color.White;
            this.lblPhieuNhapIcon.Location = new System.Drawing.Point(120, 30);
            this.lblPhieuNhapIcon.AutoSize = true;

            this.lblPhieuNhapValue.Text = "0";
            this.lblPhieuNhapValue.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblPhieuNhapValue.ForeColor = System.Drawing.Color.White;
            this.lblPhieuNhapValue.Location = new System.Drawing.Point(15, 25);
            this.lblPhieuNhapValue.AutoSize = true;

            this.lblPhieuNhapTitle.Text = "Phiếu nhập";
            this.lblPhieuNhapTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPhieuNhapTitle.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.lblPhieuNhapTitle.Location = new System.Drawing.Point(15, 75);
            this.lblPhieuNhapTitle.AutoSize = true;

            // 
            // pnlCardPhieuXuat - Card Phiếu xuất
            // 
            this.pnlCardPhieuXuat.BackColor = System.Drawing.Color.FromArgb(230, 126, 34);
            this.pnlCardPhieuXuat.Size = new System.Drawing.Size(180, 110);
            this.pnlCardPhieuXuat.Margin = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.pnlCardPhieuXuat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlCardPhieuXuat.Controls.Add(this.lblPhieuXuatIcon);
            this.pnlCardPhieuXuat.Controls.Add(this.lblPhieuXuatValue);
            this.pnlCardPhieuXuat.Controls.Add(this.lblPhieuXuatTitle);

            this.lblPhieuXuatIcon.Text = "📤";
            this.lblPhieuXuatIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 28F);
            this.lblPhieuXuatIcon.ForeColor = System.Drawing.Color.White;
            this.lblPhieuXuatIcon.Location = new System.Drawing.Point(120, 30);
            this.lblPhieuXuatIcon.AutoSize = true;

            this.lblPhieuXuatValue.Text = "0";
            this.lblPhieuXuatValue.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblPhieuXuatValue.ForeColor = System.Drawing.Color.White;
            this.lblPhieuXuatValue.Location = new System.Drawing.Point(15, 25);
            this.lblPhieuXuatValue.AutoSize = true;

            this.lblPhieuXuatTitle.Text = "Phiếu xuất";
            this.lblPhieuXuatTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPhieuXuatTitle.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.lblPhieuXuatTitle.Location = new System.Drawing.Point(15, 75);
            this.lblPhieuXuatTitle.AutoSize = true;

            // 
            // pnlCardDoanhThu - Card Doanh thu
            // 
            this.pnlCardDoanhThu.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.pnlCardDoanhThu.Size = new System.Drawing.Size(200, 110);
            this.pnlCardDoanhThu.Margin = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.pnlCardDoanhThu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlCardDoanhThu.Controls.Add(this.lblDoanhThuIcon);
            this.pnlCardDoanhThu.Controls.Add(this.lblDoanhThuValue);
            this.pnlCardDoanhThu.Controls.Add(this.lblDoanhThuTitle);

            this.lblDoanhThuIcon.Text = "💰";
            this.lblDoanhThuIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 28F);
            this.lblDoanhThuIcon.ForeColor = System.Drawing.Color.White;
            this.lblDoanhThuIcon.Location = new System.Drawing.Point(140, 30);
            this.lblDoanhThuIcon.AutoSize = true;

            this.lblDoanhThuValue.Text = "0 đ";
            this.lblDoanhThuValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblDoanhThuValue.ForeColor = System.Drawing.Color.White;
            this.lblDoanhThuValue.Location = new System.Drawing.Point(15, 30);
            this.lblDoanhThuValue.AutoSize = true;

            this.lblDoanhThuTitle.Text = "Doanh thu tháng";
            this.lblDoanhThuTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDoanhThuTitle.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.lblDoanhThuTitle.Location = new System.Drawing.Point(15, 75);
            this.lblDoanhThuTitle.AutoSize = true;

            // 
            // pnlCardTonKho - Card Tồn kho
            // 
            this.pnlCardTonKho.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.pnlCardTonKho.Size = new System.Drawing.Size(180, 110);
            this.pnlCardTonKho.Margin = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.pnlCardTonKho.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlCardTonKho.Controls.Add(this.lblTonKhoIcon);
            this.pnlCardTonKho.Controls.Add(this.lblTonKhoValue);
            this.pnlCardTonKho.Controls.Add(this.lblTonKhoTitle);

            this.lblTonKhoIcon.Text = "🏭";
            this.lblTonKhoIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 28F);
            this.lblTonKhoIcon.ForeColor = System.Drawing.Color.White;
            this.lblTonKhoIcon.Location = new System.Drawing.Point(120, 30);
            this.lblTonKhoIcon.AutoSize = true;

            this.lblTonKhoValue.Text = "0";
            this.lblTonKhoValue.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblTonKhoValue.ForeColor = System.Drawing.Color.White;
            this.lblTonKhoValue.Location = new System.Drawing.Point(15, 25);
            this.lblTonKhoValue.AutoSize = true;

            this.lblTonKhoTitle.Text = "Tồn kho";
            this.lblTonKhoTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTonKhoTitle.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.lblTonKhoTitle.Location = new System.Drawing.Point(15, 75);
            this.lblTonKhoTitle.AutoSize = true;

            // 
            // pnlQuickActions - Quick Actions Panel
            // 
            this.pnlQuickActions.BackColor = System.Drawing.Color.White;
            this.pnlQuickActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQuickActions.Height = 80;
            this.pnlQuickActions.Padding = new System.Windows.Forms.Padding(15);
            this.pnlQuickActions.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.pnlQuickActions.Controls.Add(this.btnQuickThongKe);
            this.pnlQuickActions.Controls.Add(this.btnQuickKiemKe);
            this.pnlQuickActions.Controls.Add(this.btnQuickXuat);
            this.pnlQuickActions.Controls.Add(this.btnQuickNhap);
            this.pnlQuickActions.Controls.Add(this.lblQuickActionsTitle);

            this.lblQuickActionsTitle.Text = "Thao tác nhanh:";
            this.lblQuickActionsTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblQuickActionsTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblQuickActionsTitle.Location = new System.Drawing.Point(15, 28);
            this.lblQuickActionsTitle.AutoSize = true;

            this.btnQuickNhap.Text = "📥 Tạo phiếu nhập";
            this.btnQuickNhap.Size = new System.Drawing.Size(150, 40);
            this.btnQuickNhap.Location = new System.Drawing.Point(150, 18);
            this.btnQuickNhap.BackColor = System.Drawing.Color.FromArgb(155, 89, 182);
            this.btnQuickNhap.ForeColor = System.Drawing.Color.White;
            this.btnQuickNhap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuickNhap.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnQuickNhap.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnQuickXuat.Text = "📤 Tạo phiếu xuất";
            this.btnQuickXuat.Size = new System.Drawing.Size(150, 40);
            this.btnQuickXuat.Location = new System.Drawing.Point(310, 18);
            this.btnQuickXuat.BackColor = System.Drawing.Color.FromArgb(230, 126, 34);
            this.btnQuickXuat.ForeColor = System.Drawing.Color.White;
            this.btnQuickXuat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuickXuat.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnQuickXuat.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnQuickKiemKe.Text = "📋 Kiểm kê";
            this.btnQuickKiemKe.Size = new System.Drawing.Size(120, 40);
            this.btnQuickKiemKe.Location = new System.Drawing.Point(470, 18);
            this.btnQuickKiemKe.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnQuickKiemKe.ForeColor = System.Drawing.Color.White;
            this.btnQuickKiemKe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuickKiemKe.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnQuickKiemKe.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnQuickThongKe.Text = "📊 Thống kê";
            this.btnQuickThongKe.Size = new System.Drawing.Size(120, 40);
            this.btnQuickThongKe.Location = new System.Drawing.Point(600, 18);
            this.btnQuickThongKe.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnQuickThongKe.ForeColor = System.Drawing.Color.White;
            this.btnQuickThongKe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuickThongKe.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnQuickThongKe.Cursor = System.Windows.Forms.Cursors.Hand;

            // 
            // pnlCharts - Charts Panel
            // 
            this.pnlCharts.BackColor = System.Drawing.Color.White;
            this.pnlCharts.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCharts.Height = 280;
            this.pnlCharts.Padding = new System.Windows.Forms.Padding(15);
            this.pnlCharts.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.pnlCharts.Controls.Add(this.pnlChartContainer);
            this.pnlCharts.Controls.Add(this.lblChartTitle);

            this.lblChartTitle.Text = "📈 Doanh thu 7 ngày gần nhất";
            this.lblChartTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblChartTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblChartTitle.Location = new System.Drawing.Point(15, 15);
            this.lblChartTitle.AutoSize = true;
            this.lblChartTitle.Dock = System.Windows.Forms.DockStyle.Top;

            this.pnlChartContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChartContainer.BackColor = System.Drawing.Color.White;
            this.pnlChartContainer.Padding = new System.Windows.Forms.Padding(10, 40, 10, 10);

            // 
            // pnlRecentActivity - Recent Activity Panel
            // 
            this.pnlRecentActivity.BackColor = System.Drawing.Color.White;
            this.pnlRecentActivity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRecentActivity.Padding = new System.Windows.Forms.Padding(15);
            this.pnlRecentActivity.Controls.Add(this.dgvRecentActivity);
            this.pnlRecentActivity.Controls.Add(this.lblRecentTitle);

            this.lblRecentTitle.Text = "📋 Hoạt động gần đây";
            this.lblRecentTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRecentTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblRecentTitle.Location = new System.Drawing.Point(15, 15);
            this.lblRecentTitle.AutoSize = true;
            this.lblRecentTitle.Dock = System.Windows.Forms.DockStyle.Top;

            // 
            // dgvRecentActivity
            // 
            this.dgvRecentActivity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecentActivity.BackgroundColor = System.Drawing.Color.White;
            this.dgvRecentActivity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRecentActivity.RowHeadersVisible = false;
            this.dgvRecentActivity.AllowUserToAddRows = false;
            this.dgvRecentActivity.AllowUserToDeleteRows = false;
            this.dgvRecentActivity.ReadOnly = true;
            this.dgvRecentActivity.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecentActivity.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRecentActivity.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.dgvRecentActivity.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvRecentActivity.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dgvRecentActivity.ColumnHeadersHeight = 40;
            this.dgvRecentActivity.EnableHeadersVisualStyles = false;
            this.dgvRecentActivity.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.dgvRecentActivity.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.dgvRecentActivity.RowTemplate.Height = 35;

            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.pnlMain);
            this.Name = "DashboardForm";
            this.Text = "Dashboard";
            this.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);

            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentActivity)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlCards.ResumeLayout(false);
            this.pnlCardSanPham.ResumeLayout(false);
            this.pnlCardSanPham.PerformLayout();
            this.pnlCardKhachHang.ResumeLayout(false);
            this.pnlCardKhachHang.PerformLayout();
            this.pnlCardPhieuNhap.ResumeLayout(false);
            this.pnlCardPhieuNhap.PerformLayout();
            this.pnlCardPhieuXuat.ResumeLayout(false);
            this.pnlCardPhieuXuat.PerformLayout();
            this.pnlCardDoanhThu.ResumeLayout(false);
            this.pnlCardDoanhThu.PerformLayout();
            this.pnlCardTonKho.ResumeLayout(false);
            this.pnlCardTonKho.PerformLayout();
            this.pnlCharts.ResumeLayout(false);
            this.pnlCharts.PerformLayout();
            this.pnlRecentActivity.ResumeLayout(false);
            this.pnlRecentActivity.PerformLayout();
            this.pnlQuickActions.ResumeLayout(false);
            this.pnlQuickActions.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        // Main panels
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.FlowLayoutPanel pnlCards;
        private System.Windows.Forms.Panel pnlCharts;
        private System.Windows.Forms.Panel pnlRecentActivity;
        private System.Windows.Forms.Panel pnlQuickActions;

        // Header controls
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Timer timerDateTime;

        // Stat cards
        private System.Windows.Forms.Panel pnlCardSanPham;
        private System.Windows.Forms.Panel pnlCardKhachHang;
        private System.Windows.Forms.Panel pnlCardPhieuNhap;
        private System.Windows.Forms.Panel pnlCardPhieuXuat;
        private System.Windows.Forms.Panel pnlCardDoanhThu;
        private System.Windows.Forms.Panel pnlCardTonKho;

        // Card labels
        private System.Windows.Forms.Label lblSanPhamTitle;
        private System.Windows.Forms.Label lblSanPhamValue;
        private System.Windows.Forms.Label lblSanPhamIcon;

        private System.Windows.Forms.Label lblKhachHangTitle;
        private System.Windows.Forms.Label lblKhachHangValue;
        private System.Windows.Forms.Label lblKhachHangIcon;

        private System.Windows.Forms.Label lblPhieuNhapTitle;
        private System.Windows.Forms.Label lblPhieuNhapValue;
        private System.Windows.Forms.Label lblPhieuNhapIcon;

        private System.Windows.Forms.Label lblPhieuXuatTitle;
        private System.Windows.Forms.Label lblPhieuXuatValue;
        private System.Windows.Forms.Label lblPhieuXuatIcon;

        private System.Windows.Forms.Label lblDoanhThuTitle;
        private System.Windows.Forms.Label lblDoanhThuValue;
        private System.Windows.Forms.Label lblDoanhThuIcon;

        private System.Windows.Forms.Label lblTonKhoTitle;
        private System.Windows.Forms.Label lblTonKhoValue;
        private System.Windows.Forms.Label lblTonKhoIcon;

        // Charts
        private System.Windows.Forms.Label lblChartTitle;
        private System.Windows.Forms.Panel pnlChartContainer;

        // Recent activity
        private System.Windows.Forms.Label lblRecentTitle;
        private System.Windows.Forms.DataGridView dgvRecentActivity;

        // Quick actions
        private System.Windows.Forms.Label lblQuickActionsTitle;
        private System.Windows.Forms.Button btnQuickNhap;
        private System.Windows.Forms.Button btnQuickXuat;
        private System.Windows.Forms.Button btnQuickKiemKe;
        private System.Windows.Forms.Button btnQuickThongKe;
    }
}