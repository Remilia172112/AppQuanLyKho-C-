using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.Auth
{
    partial class QuenMatKhau
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            
            // Step 1: Nhập Email
            this.pnlStep1 = new System.Windows.Forms.Panel();
            this.lblNhapEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.btnSendMail = new System.Windows.Forms.Button();

            // Step 2: Nhập OTP
            this.pnlStep2 = new System.Windows.Forms.Panel();
            this.lblNhapOTP = new System.Windows.Forms.Label();
            this.txtOTP = new System.Windows.Forms.TextBox();
            this.btnConfirmOTP = new System.Windows.Forms.Button();

            // Step 3: Đổi mật khẩu
            this.pnlStep3 = new System.Windows.Forms.Panel();
            this.lblNhapPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnChangePass = new System.Windows.Forms.Button();

            this.pnlTop.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlStep1.SuspendLayout();
            this.pnlStep2.SuspendLayout();
            this.pnlStep3.SuspendLayout();
            this.SuspendLayout();

            // 
            // Form Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 250); // Tăng chiều cao xíu cho thoáng
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quên mật khẩu";

            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(22, 122, 198);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 60;

            // lblTitle
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Text = "QUÊN MẬT KHẨU";

            // 
            // pnlMain (Container)
            // 
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.Controls.Add(this.pnlStep1);
            this.pnlMain.Controls.Add(this.pnlStep2);
            this.pnlMain.Controls.Add(this.pnlStep3);

            // ================= STEP 1 =================
            this.pnlStep1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStep1.Padding = new System.Windows.Forms.Padding(20);
            
            this.lblNhapEmail.Text = "Nhập địa chỉ email:";
            this.lblNhapEmail.AutoSize = true;
            this.lblNhapEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNhapEmail.Location = new System.Drawing.Point(50, 30);

            this.txtEmail.Location = new System.Drawing.Point(50, 60);
            this.txtEmail.Size = new System.Drawing.Size(380, 30);
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 12F);

            this.btnSendMail.Text = "Gửi mã";
            this.btnSendMail.Size = new System.Drawing.Size(120, 40);
            this.btnSendMail.Location = new System.Drawing.Point(180, 110);
            this.btnSendMail.BackColor = System.Drawing.Color.FromArgb(22, 122, 198);
            this.btnSendMail.ForeColor = System.Drawing.Color.White;
            this.btnSendMail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendMail.Cursor = System.Windows.Forms.Cursors.Hand;

            this.pnlStep1.Controls.Add(this.lblNhapEmail);
            this.pnlStep1.Controls.Add(this.txtEmail);
            this.pnlStep1.Controls.Add(this.btnSendMail);

            // ================= STEP 2 =================
            this.pnlStep2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStep2.Visible = false; // Ẩn mặc định

            this.lblNhapOTP.Text = "Nhập mã OTP:";
            this.lblNhapOTP.AutoSize = true;
            this.lblNhapOTP.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNhapOTP.Location = new System.Drawing.Point(50, 30);

            this.txtOTP.Location = new System.Drawing.Point(50, 60);
            this.txtOTP.Size = new System.Drawing.Size(380, 30);
            this.txtOTP.Font = new System.Drawing.Font("Segoe UI", 12F);

            this.btnConfirmOTP.Text = "Xác nhận";
            this.btnConfirmOTP.Size = new System.Drawing.Size(120, 40);
            this.btnConfirmOTP.Location = new System.Drawing.Point(180, 110);
            this.btnConfirmOTP.BackColor = System.Drawing.Color.FromArgb(22, 122, 198);
            this.btnConfirmOTP.ForeColor = System.Drawing.Color.White;
            this.btnConfirmOTP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmOTP.Cursor = System.Windows.Forms.Cursors.Hand;

            this.pnlStep2.Controls.Add(this.lblNhapOTP);
            this.pnlStep2.Controls.Add(this.txtOTP);
            this.pnlStep2.Controls.Add(this.btnConfirmOTP);

            // ================= STEP 3 =================
            this.pnlStep3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStep3.Visible = false; // Ẩn mặc định

            this.lblNhapPassword.Text = "Nhập mật khẩu mới:";
            this.lblNhapPassword.AutoSize = true;
            this.lblNhapPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNhapPassword.Location = new System.Drawing.Point(50, 30);

            this.txtPassword.Location = new System.Drawing.Point(50, 60);
            this.txtPassword.Size = new System.Drawing.Size(380, 30);
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtPassword.UseSystemPasswordChar = true;

            this.btnChangePass.Text = "Xác nhận";
            this.btnChangePass.Size = new System.Drawing.Size(120, 40);
            this.btnChangePass.Location = new System.Drawing.Point(180, 110);
            this.btnChangePass.BackColor = System.Drawing.Color.FromArgb(22, 122, 198);
            this.btnChangePass.ForeColor = System.Drawing.Color.White;
            this.btnChangePass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangePass.Cursor = System.Windows.Forms.Cursors.Hand;

            this.pnlStep3.Controls.Add(this.lblNhapPassword);
            this.pnlStep3.Controls.Add(this.txtPassword);
            this.pnlStep3.Controls.Add(this.btnChangePass);

            // Add Controls
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlTop);

            this.pnlTop.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlStep1.ResumeLayout(false);
            this.pnlStep1.PerformLayout();
            this.pnlStep2.ResumeLayout(false);
            this.pnlStep2.PerformLayout();
            this.pnlStep3.ResumeLayout(false);
            this.pnlStep3.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlMain;

        // Step 1
        private System.Windows.Forms.Panel pnlStep1;
        private System.Windows.Forms.Label lblNhapEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnSendMail;

        // Step 2
        private System.Windows.Forms.Panel pnlStep2;
        private System.Windows.Forms.Label lblNhapOTP;
        private System.Windows.Forms.TextBox txtOTP;
        private System.Windows.Forms.Button btnConfirmOTP;

        // Step 3
        private System.Windows.Forms.Panel pnlStep3;
        private System.Windows.Forms.Label lblNhapPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnChangePass;
    }
}