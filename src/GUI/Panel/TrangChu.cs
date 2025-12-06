using System;
using System.Drawing;
using System.Windows.Forms;
using src.GUI.Component;

namespace src.GUI.Panel
{
    public class TrangChu : UserControl
    {
        private System.Windows.Forms.Panel pnlTop;
        private FlowLayoutPanel pnlCenter; // Dùng FlowLayout thay vì GridLayout để dễ cuộn
        
        // Màu sắc theo Java
        Color MainColor = Color.FromArgb(193, 237, 220);
        Color BackgroundColor = Color.FromArgb(79, 100, 87); // 0x4F6457

        // Dữ liệu (Giữ nguyên HTML string để PanelShadow xử lý replace)
        string[,] data = {
            {"Tính <br><br>tiện <br><br>lợi", "convenient_100px.svg", "<html>Có tính năng tìm kiếm nhanh chóng giúp người dùng dễ dàng<br><br> tìm sách theo tiêu chí cụ thể như tiêu đề, tác giả thể loại hoặc <br><br>theo mã ISBN sẽ đảm bảo tính chính xác và độ tin cậy cao hơn .</html>"},
            {"Tính <br><br>bảo <br><br>mật", "secure_100px.svg", "<html>Thông tin cá nhân và thông tin liên quan đến sách mượn thường <br><br>được bảo mật và chỉ được truy cập bởi người dùng hoặc những <br><br>người được ủy quyền.</html>"},
            {"Tính <br><br>hiệu <br><br>quả", "effective_100px.svg", "<html>Nhờ vào tính năng đặc biệt của mã ISBN giúp xác định được thông <br><br>tin về từng cuốn sách một cách nhanh chóng và chính xác, giúp <br><br>cho việc quản lý sách được thực hiện một cách hiệu quả hơn.</html>"},
        };

        public TrangChu()
        {
            InitComponent();
        }

        private void InitComponent()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(24, 24, 24);

            // 1. Panel Top (Banner Slogan)
            pnlTop = new System.Windows.Forms.Panel();
            pnlTop.BackColor = MainColor;
            pnlTop.Size = new Size(1100, 200); // Chiều cao cố định
            pnlTop.Dock = DockStyle.Top;
            
            PictureBox pbSlogan = new PictureBox();
            pbSlogan.Dock = DockStyle.Fill;
            pbSlogan.SizeMode = PictureBoxSizeMode.CenterImage; // Canh giữa ảnh
            try
            {
                // Load ảnh slogan (PNG hoặc SVG)
                // Lưu ý: Java code dùng /img/slogan.png, ở đây mình giả định bạn để trong Images/slogan.png
                string sloganPath = "img/slogan.png"; 
                if (System.IO.File.Exists(sloganPath))
                {
                    pbSlogan.Image = Image.FromFile(sloganPath);
                }
            }
            catch { }
            pnlTop.Controls.Add(pbSlogan);

            // 2. Panel Center (Danh sách PanelShadow)
            pnlCenter = new FlowLayoutPanel();
            pnlCenter.BackColor = BackgroundColor;
            pnlCenter.Dock = DockStyle.Fill;
            pnlCenter.AutoScroll = true; // Cho phép cuộn nếu tràn
            pnlCenter.FlowDirection = FlowDirection.LeftToRight; 
            pnlCenter.WrapContents = true;
            
            // Căn lề (Padding) giống Java: EmptyBorder(30, 110, 30, 220)
            // WinForms padding tính: Left, Top, Right, Bottom
            pnlCenter.Padding = new Padding(110, 30, 0, 30); 

            // Load dữ liệu vào
            int rows = data.GetLength(0);
            for (int i = 0; i < rows; i++)
            {
                string title = data[i, 0];
                string icon = data[i, 1];
                string desc = data[i, 2];

                // Tạo PanelShadow
                PanelShadow item = new PanelShadow(icon, title, desc);
                pnlCenter.Controls.Add(item);
            }
            this.Controls.Add(pnlCenter); // Fill
            this.Controls.Add(pnlTop);    // Top
            pnlCenter.BringToFront();     // Đưa Center lên trên (hoặc pnlTop.SendToBack())
        }
    }
}