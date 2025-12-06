using System;
using System.Drawing;
using System.Windows.Forms;
using src.DAO;
using src.DTO;
using src.Helper;

namespace src.GUI.Auth
{
    public partial class QuenMatKhau : Form
    {
        private string emailCheck;

        public QuenMatKhau()
        {
            InitializeComponent();
            
            // Đăng ký sự kiện
            btnSendMail.Click += BtnSendMail_Click;
            btnConfirmOTP.Click += BtnConfirmOTP_Click;
            btnChangePass.Click += BtnChangePass_Click;
        }

        // --- STEP 1: Gửi OTP ---
        private void BtnSendMail_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (Validation.IsEmpty(email))
            {
                MessageBox.Show("Vui lòng không để trống email", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Validation.IsEmail(email))
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng email", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TaiKhoanDTO tk = TaiKhoanDAO.Instance.selectByEmail(email);

            if (tk == null)
            {
                MessageBox.Show("Tài khoản của email này không tồn tại trên hệ thống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Gửi OTP
            this.emailCheck = email;
            string otp = SendEmailSMTP.GetOTP();
            
            // Gửi qua mail
            SendEmailSMTP.SendOTP(email, otp);
            
            // Lưu OTP vào DB
            TaiKhoanDAO.Instance.sendOpt(email, otp);

            MessageBox.Show("Đã gửi mã OTP vào email của bạn!", "Thông báo");
            
            // Chuyển sang Step 2
            SwitchToPanel(pnlStep2);
        }

        // --- STEP 2: Xác nhận OTP ---
        private void BtnConfirmOTP_Click(object sender, EventArgs e)
        {
            string otp = txtOTP.Text.Trim();

            if (Validation.IsEmpty(otp))
            {
                MessageBox.Show("Vui lòng không để trống mã OTP", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate 6 số
            if (!System.Text.RegularExpressions.Regex.IsMatch(otp, @"^\d{6}$"))
            {
                MessageBox.Show("Vui lòng nhập mã OTP có 6 chữ số!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check OTP Database
            bool check = TaiKhoanDAO.Instance.checkOtp(this.emailCheck, otp);

            if (check)
            {
                MessageBox.Show("Xác thực thành công!", "Thông báo");
                // Chuyển sang Step 3
                SwitchToPanel(pnlStep3);
            }
            else
            {
                MessageBox.Show("Mã OTP không khớp", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- STEP 3: Đổi mật khẩu ---
        private void BtnChangePass_Click(object sender, EventArgs e)
        {
            string pass = txtPassword.Text.Trim();

            if (Validation.IsEmpty(pass))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cập nhật mật khẩu
            // Lưu ý: Hàm TaiKhoanDAO.updatePass bên C# đã có sẵn logic Hash BCrypt bên trong rồi
            // Nên ở đây mình truyền password thô vào, không cần hash ở GUI nữa (để tránh hash 2 lần)
            
            // Nếu bạn muốn giữ logic cũ (Hash tại GUI), thì phải sửa DAO nhận hash sẵn.
            // Nhưng theo DAO C# tôi gửi trước đó, DAO sẽ tự hash.
            TaiKhoanDAO.Instance.updatePass(this.emailCheck, pass);

            // Xóa OTP cũ
            TaiKhoanDAO.Instance.sendOpt(emailCheck, "null");

            MessageBox.Show("Thay đổi mật khẩu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        // Hàm helper để chuyển Panel (Thay thế CardLayout)
        private void SwitchToPanel(Panel pnl)
        {
            pnlStep1.Visible = false;
            pnlStep2.Visible = false;
            pnlStep3.Visible = false;

            pnl.Visible = true;
            pnl.BringToFront();
        }
    }
}