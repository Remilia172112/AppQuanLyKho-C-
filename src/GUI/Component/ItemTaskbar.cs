using System;
using System.Drawing;
using System.Windows.Forms;
using Svg; // Bắt buộc phải có thư viện này

namespace src.GUI.Component
{
    public class ItemTaskbar : UserControl
    {
        public bool IsSelected = false;
        
        // Màu sắc
        private Color colorBlack = Color.FromArgb(26, 26, 26);
        private Color defaultColor = Color.White;
        private Color hoverColor = Color.FromArgb(193, 237, 220); // Màu xanh nhạt giống Java
        private Color fontColorHover = Color.FromArgb(1, 87, 155);

        // Components
        private Label lblContent;
        private PictureBox pbIcon;

        // Constructor chính dùng cho Menu
        public ItemTaskbar(string linkIcon, string content)
        {
            InitCommon();
            // Chỉnh kích thước item nhỏ lại chút cho vừa Menu 250px
            this.Size = new Size(230, 45); 
            this.Padding = new Padding(15, 0, 0, 0); // Căn lề trái

            // 1. Icon (Bên trái)
            pbIcon = new PictureBox();
            pbIcon.Size = new Size(24, 24); // Kích thước icon chuẩn
            pbIcon.SizeMode = PictureBoxSizeMode.Zoom;
            pbIcon.Dock = DockStyle.Left;
            pbIcon.BackColor = Color.Transparent;
            
            // Load ảnh SVG
            LoadImage(pbIcon, linkIcon);
            this.Controls.Add(pbIcon);

            // 2. Text (Bên phải)
            lblContent = new Label();
            lblContent.Text = content;
            lblContent.Dock = DockStyle.Fill; // Lấp đầy phần còn lại
            lblContent.TextAlign = ContentAlignment.MiddleLeft; // Căn giữa dọc, lề trái
            lblContent.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblContent.ForeColor = colorBlack;
            lblContent.Padding = new Padding(10, 0, 0, 0); // Cách icon ra 10px
            
            // Add Label sau PictureBox (Vì Dock Left add sau sẽ nằm bên phải cái trước)
            // Mẹo: Trong Controls.Add, cái nào add trước sẽ được ưu tiên Dock trước. 
            // Nhưng để an toàn, ta dùng BringToFront cho Icon.
            this.Controls.Add(lblContent);
            pbIcon.BringToFront(); // Đảm bảo icon nằm bên trái cùng

            // Đăng ký sự kiện Hover
            AddHoverEvent(this);
            AddHoverEvent(lblContent);
            AddHoverEvent(pbIcon);
        }

        private void InitCommon()
        {
            this.IsSelected = false;
            this.BackColor = defaultColor;
            this.Cursor = Cursors.Hand;
        }

        private void LoadImage(PictureBox pb, string linkIcon)
        {
            try
            {
                // Đường dẫn tính từ thư mục bin/Debug
                string path = "img/icon/" + linkIcon; 
                
                if (System.IO.File.Exists(path))
                {
                    if (path.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
                    {
                        var svgDoc = SvgDocument.Open(path);
                        // Vẽ SVG thành ảnh Bitmap đúng kích thước PictureBox
                        pb.Image = svgDoc.Draw(pb.Width, pb.Height);
                    }
                    else
                    {
                        pb.Image = Image.FromFile(path);
                    }
                }
                else
                {
                    // Debug: In ra nếu không thấy file
                    Console.WriteLine("Không tìm thấy ảnh: " + path);
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Lỗi load ảnh: " + ex.Message);
            }
        }

        // --- Xử lý sự kiện Hover/Click ---
        private void AddHoverEvent(Control ctrl)
        {
            ctrl.MouseEnter += (s, e) => 
            {
                if (!IsSelected) {
                    this.BackColor = hoverColor;
                    lblContent.ForeColor = fontColorHover;
                }
            };
            ctrl.MouseLeave += (s, e) => 
            {
                if (!IsSelected) {
                    this.BackColor = defaultColor;
                    lblContent.ForeColor = colorBlack;
                }
            };
            ctrl.Click += (s, e) => this.InvokeOnClick(this, EventArgs.Empty);
        }

        public void SetSelected(bool selected)
        {
            this.IsSelected = selected;
            if (selected)
            {
                this.BackColor = hoverColor;
                lblContent.ForeColor = fontColorHover;
            }
            else
            {
                this.BackColor = defaultColor;
                lblContent.ForeColor = colorBlack;
            }
        }
    }
}