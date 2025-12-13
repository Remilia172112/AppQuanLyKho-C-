using System;
using System.Drawing;
using System.Windows.Forms;
using src.BUS;
using src.DTO;
using src.Helper;
using src.GUI.Components;

namespace src.GUI.Auth
{
    public partial class LoginPage : Form
    {
        private TaiKhoanBUS taiKhoanBUS;
        private NhanVienBUS nhanVienBUS;
        private NhomQuyenBUS nhomQuyenBUS;

        public LoginPage()
        {
            try 
            {
                string iconPath = "icon/app.ico";
                if (System.IO.File.Exists(iconPath))
                {
                    this.Icon = new System.Drawing.Icon(iconPath);
                }
            }
            catch { }
            InitializeComponent();
            taiKhoanBUS = new TaiKhoanBUS();
            nhanVienBUS = new NhanVienBUS();
            nhomQuyenBUS = new NhomQuyenBUS();
            // Set giá trị mặc định
            txtUsername.Text = "QL1";
            txtPassword.Text = "123456";

            // Đăng ký các sự kiện
            AddEvents();
        }

        private void AddEvents()
        {
            // Sự kiện Login
            btnLogin.Click += BtnLogin_Click;
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = Color.FromArgb(0, 202, 232);
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = Color.Black;

            // Sự kiện Quên mật khẩu
            lblForgotPass.Click += (s, e) => {
                new QuenMatKhau().ShowDialog();
            };
            lblForgotPass.MouseEnter += (s, e) => lblForgotPass.ForeColor = Color.FromArgb(0, 202, 232);
            lblForgotPass.MouseLeave += (s, e) => lblForgotPass.ForeColor = Color.Black;


            // Sự kiện phím Enter
            txtUsername.KeyDown += Enter_KeyDown;
            txtPassword.KeyDown += Enter_KeyDown;
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
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                // Validation
                if (string.IsNullOrEmpty(username))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                // Authenticate
                TaiKhoanDTO? account = taiKhoanBUS.DangNhap(username, password);

                if (account == null)
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi đăng nhập", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                // Check account status
                if (account.TT == 0)
                {
                    MessageBox.Show("Tài khoản đã bị khóa!", "Lỗi đăng nhập", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get employee info
                NhanVienDTO? employee = nhanVienBUS.GetById(account.MNV);
                if (employee == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin nhân viên!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Get role and permissions
                NhomQuyenDTO? role = nhomQuyenBUS.GetById(account.MNQ);
                var permissions = nhomQuyenBUS.GetChiTietQuyen(account.MNQ);

                // Set session
                SessionManager.Login(account, employee, role ?? new NhomQuyenDTO(), permissions);

                MessageBox.Show($"Đăng nhập thành công!\nXin chào, {employee.HOTEN}", "Thành công", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Open main form
                this.Hide();
                MainForm mainForm = new MainForm();
                mainForm.FormClosed += (s, args) => this.Close();
                mainForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng nhập: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}