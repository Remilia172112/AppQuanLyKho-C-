namespace src.GUI
{
    partial class Main
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.pnlMainContent = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            
            // 
            // Form Main Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ thống quản lý cửa hàng sách";
            this.Name = "Main";

            // 
            // pnlMenu (Vùng bên trái - Taskbar)
            // 
            this.pnlMenu.BackColor = System.Drawing.Color.White; // Màu nền tạm
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMenu.Size = new System.Drawing.Size(250, 800);
            this.pnlMenu.Name = "pnlMenu";

            // 
            // pnlMainContent (Vùng chính giữa - TrangChu, NhanVien...)
            // 
            this.pnlMainContent.BackColor = System.Drawing.Color.FromArgb(250, 250, 250); // Màu MainColor
            this.pnlMainContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContent.Name = "pnlMainContent";
            this.pnlMainContent.Padding = new System.Windows.Forms.Padding(10); // Tạo khoảng cách viền cho đẹp

            // Thêm Controls vào Form
            this.Controls.Add(this.pnlMainContent); // Fill add trước hoặc sau tùy Z-Index, nhưng Dock Left phải add đúng thứ tự
            this.Controls.Add(this.pnlMenu);

            this.ResumeLayout(false);
        }

        #endregion

        // Panel chứa MenuTaskbar
        private System.Windows.Forms.Panel pnlMenu;
        
        // Panel chứa nội dung thay đổi (MainContent)
        public System.Windows.Forms.Panel pnlMainContent; 
    }
}