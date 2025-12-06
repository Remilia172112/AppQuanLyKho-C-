using System;
using System.Drawing;
using System.Windows.Forms;
using src.BUS;
using src.DTO;
using src.Helper; 

namespace src.GUI
{
    public partial class LoginPage : Form
    {
        // Khởi tạo BUS
        private readonly TaiKhoanBUS taiKhoanBUS = new TaiKhoanBUS();

        public LoginPage()
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
            
            // Set giá trị mặc định (như code Java cũ)
            txtUsername.setText("admin");
            txtPassword.setPass("123456");

            // Đăng ký sự kiện
            AddEvents();
        }

        private void AddEvents()
        {
            // 1. Sự kiện nút Đăng nhập
            btnLogin.Click += BtnLogin_Click;
            
            // Hiệu ứng Hover nút
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = Color.FromArgb(0, 202, 232);
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = Color.Black;

            // 2. Sự kiện Quên mật khẩu
            lblForgotPass.Click += (s, e) => MessageBox.Show("Chức năng đang phát triển!");
            lblForgotPass.MouseEnter += (s, e) => lblForgotPass.ForeColor = Color.FromArgb(0, 202, 232);
            lblForgotPass.MouseLeave += (s, e) => lblForgotPass.ForeColor = Color.Black;

            // 3. Sự kiện Đăng ký
            lblRegister.Click += (s, e) => MessageBox.Show("Chức năng đang phát triển!");
            lblRegister.MouseEnter += (s, e) => lblRegister.ForeColor = Color.Green;
            lblRegister.MouseLeave += (s, e) => lblRegister.ForeColor = Color.Black;

            // 4. Sự kiện phím Enter (Lấy TextBox bên trong InputForm ra để gán sự kiện)
            txtUsername.getTxtForm().KeyDown += Enter_KeyDown;
            txtPassword.getTxtPass().KeyDown += Enter_KeyDown;
        }

        private void Enter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CheckLogin();
                e.SuppressKeyPress = true; // Chặn tiếng 'ding'
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            CheckLogin();
        }

        private void CheckLogin()
        {
            // Lấy dữ liệu từ custom component
            string username = txtUsername.getText().Trim();
            string password = txtPassword.getPass().Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập thông tin đầy đủ", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var listUser = taiKhoanBUS.Search(username, "Username");
            
            // Lọc chính xác username (vì search dùng contains)
            TaiKhoanDTO tk = listUser.Find(x => x.TDN.Equals(username));

            if (tk == null)
            {
                MessageBox.Show("Tài khoản không tồn tại trên hệ thống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra trạng thái
            if (tk.TT == 0)
            {
                MessageBox.Show("Tài khoản của bạn đang bị khóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra mật khẩu (Dùng BCrypt Helper)
            if (BCrypts.checkpw(password, tk.MK))
            {
                // Đăng nhập thành công
                this.Hide();
                
                // Mở Form Main (Giả sử bạn sẽ tạo form này sau)
                MessageBox.Show($"Xin chào {username}! Đăng nhập thành công.", "Thông báo");
                
                Main mainForm = new Main(tk);
                mainForm.ShowDialog();

                this.Close();
            }
            else
            {
                MessageBox.Show("Mật khẩu không khớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}