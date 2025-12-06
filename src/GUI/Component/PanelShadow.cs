using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Svg; // Thư viện Svg

namespace src.GUI.Component
{
    public class PanelShadow : UserControl
    {
        // Màu sắc
        private Color backgroundColor = Color.FromArgb(79, 100, 87);
        private Color mainColor = Color.FromArgb(172, 208, 192);
        private Color fontColor = Color.FromArgb(0, 151, 178);
        private Color contentColor = Color.Gray;

        // Components
        private System.Windows.Forms.Panel pnlIconBackground;
        private PictureBox pbIcon;
        private Label lblTitle;
        private Label lblContent;

        public PanelShadow(string linkIcon, string title, string content)
        {
            // 1. Cấu hình Panel chính
            this.Size = new Size(1000, 200); // Kích thước cố định giống Java
            this.BackColor = Color.Transparent;
            this.Padding = new Padding(20); // Margin nội bộ (FlowLayout(0, 20, 10) bên Java)
            this.Margin = new Padding(0, 0, 0, 20); // Khoảng cách giữa các thẻ trong TrangChu

            // --- LAYOUT DÙNG DOCK (Thẳng hàng tuyệt đối) ---

            // A. Phần Nội dung (Nằm cuối cùng bên phải -> Add trước nếu dùng Dock Fill)
            lblContent = new Label();
            lblContent.Text = CleanText(content);
            lblContent.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lblContent.ForeColor = contentColor;
            lblContent.TextAlign = ContentAlignment.MiddleLeft;
            lblContent.Dock = DockStyle.Fill; // Tự động lấp đầy phần còn lại
            // Padding trái để cách tiêu đề ra
            lblContent.Padding = new Padding(10, 0, 0, 0); 
            this.Controls.Add(lblContent);

            // B. Phần Tiêu đề (Nằm giữa -> Add sau nội dung)
            lblTitle = new Label();
            lblTitle.Text = CleanText(title).ToUpper();
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = fontColor;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft; // Căn giữa dọc
            lblTitle.Dock = DockStyle.Left; // Neo bên trái
            lblTitle.Width = 150; // *** QUAN TRỌNG: Cố định chiều rộng để thẳng hàng ***
            this.Controls.Add(lblTitle);

            // C. Phần Icon (Nằm bên trái cùng -> Add cuối cùng)
            pnlIconBackground = new System.Windows.Forms.Panel();
            pnlIconBackground.Size = new Size(200, 125); // Kích thước khung icon
            pnlIconBackground.Dock = DockStyle.Left; // Neo sát trái
            pnlIconBackground.BackColor = Color.Transparent;
            // Thêm margin phải cho icon bằng Padding của Panel cha (đã set ở trên) hoặc Padding của chính nó
            pnlIconBackground.Padding = new Padding(0, 0, 20, 0); 
            
            // Vẽ bo góc cho khung Icon
            pnlIconBackground.Paint += (s, e) => 
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = GetRoundedPath(new Rectangle(0,0, 200, 125), 30))
                using (SolidBrush brush = new SolidBrush(backgroundColor))
                {
                    e.Graphics.FillPath(brush, path);
                }
            };

            // Hình ảnh Icon
            pbIcon = new PictureBox();
            pbIcon.Size = new Size(100, 100);
            pbIcon.SizeMode = PictureBoxSizeMode.Zoom;
            pbIcon.BackColor = Color.Transparent;
            // Căn giữa icon trong khung xanh
            pbIcon.Location = new Point((200 - 100) / 2, (125 - 100) / 2);
            
            LoadImage(pbIcon, linkIcon);
            pnlIconBackground.Controls.Add(pbIcon);
            
            this.Controls.Add(pnlIconBackground);
        }

        // Vẽ nền bo góc cho toàn bộ thẻ
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 20); // Trừ height để tạo khoảng hở shadow giả
            using (GraphicsPath path = GetRoundedPath(rect, 30))
            using (SolidBrush brush = new SolidBrush(mainColor))
            {
                e.Graphics.FillPath(brush, path);
            }
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private string CleanText(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return input.Replace("<html>", "").Replace("</html>", "").Replace("<br>", "\n").Replace("<br/>", "\n");
        }

        private void LoadImage(PictureBox pb, string linkIcon)
        {
            try
            {
                string path = "img/icon/" + linkIcon;
                if (System.IO.File.Exists(path))
                {
                    if (path.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
                    {
                        var svgDoc = SvgDocument.Open(path);
                        pb.Image = svgDoc.Draw(pb.Width, pb.Height);
                    }
                    else
                    {
                        string pngPath = path.Replace(".svg", ".png");
                        if (System.IO.File.Exists(pngPath)) pb.Image = Image.FromFile(pngPath);
                        else pb.Image = Image.FromFile(path);
                    }
                }
            }
            catch { }
        }
    }
}