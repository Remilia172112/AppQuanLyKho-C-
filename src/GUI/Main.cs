using System;
using System.Drawing;
using System.Windows.Forms;
using src.DTO;
using src.GUI.Component; 
using src.GUI.Panel;     

namespace src.GUI
{
    public partial class Main : Form
    {
        public TaiKhoanDTO user;
        
        // Khai báo các UserControl
        private MenuTaskbar menuTaskbar;
        private TrangChu trangChu;

        public Main()
        {
            try 
            {
                string iconPath = "img/icon/app.ico";
                if (System.IO.File.Exists(iconPath))
                {
                    this.Icon = new System.Drawing.Icon(iconPath);
                }
            }
            catch { }
            InitializeComponent();
            InitLogic(null);
        }

        public Main(TaiKhoanDTO user)
        {
            InitializeComponent();
            this.user = user;
            InitLogic(user);
        }

        private void InitLogic(TaiKhoanDTO user)
        {
            this.FormClosing += Main_FormClosing;

            // --- 1. KHỞI TẠO MENU TASKBAR ---
            // Xóa control cũ (nếu có) trong panel chứa menu
            pnlMenu.Controls.Clear();


            menuTaskbar = new MenuTaskbar(this, user);
        
            
            // Cho Menu lấp đầy Panel bên trái
            menuTaskbar.Dock = DockStyle.Fill; 
            pnlMenu.Controls.Add(menuTaskbar);

            // --- 2. KHỞI TẠO TRANG CHỦ ---
            trangChu = new TrangChu();
            SetPanel(trangChu);
        }

        // Hàm chuyển đổi Panel (Thay đổi nội dung chính)
        public void SetPanel(UserControl pn)
        {
            pnlMainContent.Controls.Clear();
            pn.Dock = DockStyle.Fill;
            pnlMainContent.Controls.Add(pn);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}