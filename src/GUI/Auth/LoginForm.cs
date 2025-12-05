using System;
using System.Drawing;
using System.Windows.Forms;
using src.BUS;
using src.DTO;
using src.GUI.Components;

namespace src.GUI.Auth
{
    public partial class LoginForm : Form
    {
        private TaiKhoanBUS taiKhoanBUS;
        private NhanVienBUS nhanVienBUS;
        private NhomQuyenBUS nhomQuyenBUS;

        public LoginForm()
        {
            InitializeComponent();
            taiKhoanBUS = new TaiKhoanBUS();
            nhanVienBUS = new NhanVienBUS();
            nhomQuyenBUS = new NhomQuyenBUS();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form settings
            this.ClientSize = new Size(400, 500);
            this.Text = "Đăng nhập - Quản lý kho hàng";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Logo/Title Panel
            Panel titlePanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(400, 120),
                BackColor = Color.FromArgb(41, 128, 185)
            };

            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ KHO HÀNG",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 35),
                Size = new Size(400, 50)
            };
            titlePanel.Controls.Add(lblTitle);
            this.Controls.Add(titlePanel);

            // Login Panel
            Panel loginPanel = new Panel
            {
                Location = new Point(50, 150),
                Size = new Size(300, 280),
                BackColor = Color.White
            };

            // Username Label
            Label lblUsername = new Label
            {
                Text = "Tên đăng nhập:",
                Location = new Point(0, 0),
                Size = new Size(280, 25),
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            loginPanel.Controls.Add(lblUsername);

            // Username TextBox
            TextBox txtUsername = new TextBox
            {
                Name = "txtUsername",
                Location = new Point(0, 30),
                Size = new Size(280, 35),
                Font = new Font("Segoe UI", 11F),
                BorderStyle = BorderStyle.FixedSingle
            };
            loginPanel.Controls.Add(txtUsername);

            // Password Label
            Label lblPassword = new Label
            {
                Text = "Mật khẩu:",
                Location = new Point(0, 80),
                Size = new Size(280, 25),
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            loginPanel.Controls.Add(lblPassword);

            // Password TextBox
            TextBox txtPassword = new TextBox
            {
                Name = "txtPassword",
                Location = new Point(0, 110),
                Size = new Size(280, 35),
                Font = new Font("Segoe UI", 11F),
                UseSystemPasswordChar = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            loginPanel.Controls.Add(txtPassword);

            // Login Button
            Button btnLogin = new Button
            {
                Name = "btnLogin",
                Text = "ĐĂNG NHẬP",
                Location = new Point(0, 170),
                Size = new Size(280, 45),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            loginPanel.Controls.Add(btnLogin);

            // Cancel Button
            Button btnCancel = new Button
            {
                Name = "btnCancel",
                Text = "THOÁT",
                Location = new Point(0, 225),
                Size = new Size(280, 40),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(189, 195, 199),
                ForeColor = Color.FromArgb(52, 73, 94),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => Application.Exit();
            loginPanel.Controls.Add(btnCancel);

            this.Controls.Add(loginPanel);

            // Enter key support
            txtPassword.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    BtnLogin_Click(btnLogin, EventArgs.Empty);
                }
            };

            this.ResumeLayout(false);
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            try
            {
                TextBox? txtUsername = this.Controls.Find("txtUsername", true)[0] as TextBox;
                TextBox? txtPassword = this.Controls.Find("txtPassword", true)[0] as TextBox;

                if (txtUsername == null || txtPassword == null) return;

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
