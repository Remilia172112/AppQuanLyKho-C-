using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.Auth
{
    partial class LoginPage
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
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlFormContainer = new System.Windows.Forms.FlowLayoutPanel();
            
            // Panel chứa Username
            this.pnlUser = new System.Windows.Forms.Panel();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            
            // Panel chứa Password
            this.pnlPass = new System.Windows.Forms.Panel();
            this.lblPass = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            
            this.lblForgotPass = new System.Windows.Forms.Label();
            this.pnlButton = new System.Windows.Forms.Panel();
            this.btnLogin = new System.Windows.Forms.Button();

            // Bắt đầu layout
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.pnlRight.SuspendLayout();
            this.pnlFormContainer.SuspendLayout();
            this.pnlUser.SuspendLayout();
            this.pnlPass.SuspendLayout();
            this.pnlButton.SuspendLayout();
            this.SuspendLayout();

            // 
            // Form LoginPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 500);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "Đăng nhập";

            // 
            // pnlLeft (Chứa hình ảnh bên trái)
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.White;
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Size = new System.Drawing.Size(500, 500);
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(10, 3, 5, 5);

            this.pbImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            
            // Logic load ảnh (Hỗ trợ SVG/PNG)
            try 
            { 
                string imgPath = "img/login-image.svg"; 
                if (System.IO.File.Exists(imgPath))
                {
                    if (imgPath.EndsWith(".svg", System.StringComparison.OrdinalIgnoreCase))
                    {
                        var svgDocument = Svg.SvgDocument.Open(imgPath);
                        this.pbImage.Image = svgDocument.Draw(500, 500);
                    }
                    else
                    {
                        this.pbImage.Image = System.Drawing.Image.FromFile(imgPath);
                    }
                }
            } 
            catch { }
            this.pnlLeft.Controls.Add(this.pbImage);

            // 
            // pnlRight (Chứa Form đăng nhập)
            // 
            this.pnlRight.BackColor = System.Drawing.Color.White;
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Padding = new System.Windows.Forms.Padding(20);

            // lblTitle
            this.lblTitle.Text = "ĐĂNG NHẬP VÀO HỆ THỐNG";
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 18, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Height = 80;

            // pnlFormContainer (Chứa 2 ô nhập liệu)
            this.pnlFormContainer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlFormContainer.BackColor = System.Drawing.Color.White;
            this.pnlFormContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFormContainer.Height = 150; // Đã giảm chiều cao để nút bấm gần hơn
            this.pnlFormContainer.Padding = new System.Windows.Forms.Padding(40, 10, 0, 0);

            // 
            // pnlUser (Gồm Label + TextBox Username)
            // 
            this.pnlUser.Size = new System.Drawing.Size(380, 60);
            this.pnlUser.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            
            this.lblUser.Text = "Tên đăng nhập";
            this.lblUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUser.Height = 25;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
            
            this.txtUsername.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtUsername.Height = 30;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
            
            this.pnlUser.Controls.Add(this.txtUsername);
            this.pnlUser.Controls.Add(this.lblUser);

            // 
            // pnlPass (Gồm Label + TextBox Password)
            // 
            this.pnlPass.Size = new System.Drawing.Size(380, 60);
            this.pnlPass.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);

            this.lblPass.Text = "Mật khẩu";
            this.lblPass.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPass.Height = 25;
            this.lblPass.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);

            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtPassword.Height = 30;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
            this.txtPassword.UseSystemPasswordChar = true;

            this.pnlPass.Controls.Add(this.txtPassword);
            this.pnlPass.Controls.Add(this.lblPass);

            // Thêm vào Container
            this.pnlFormContainer.Controls.Add(this.pnlUser);
            this.pnlFormContainer.Controls.Add(this.pnlPass);

            // 
            // lblForgotPass (Quên mật khẩu - Đặt ngay dưới ô pass)
            // 
            this.lblForgotPass.Text = "Quên mật khẩu?";
            this.lblForgotPass.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Underline);
            this.lblForgotPass.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblForgotPass.AutoSize = true;
            this.lblForgotPass.Location = new System.Drawing.Point(320, 235); // Vị trí đã chỉnh sửa

            // 
            // pnlButton (Chứa nút Đăng nhập - Đặt ngay dưới ForgotPass)
            // 
            this.pnlButton.Dock = System.Windows.Forms.DockStyle.None; // Không dùng Dock để tự chỉnh vị trí
            this.pnlButton.Location = new System.Drawing.Point(0, 260); // Tọa độ Y=260
            this.pnlButton.Size = new System.Drawing.Size(500, 80);
            this.pnlButton.Padding = new System.Windows.Forms.Padding(50, 10, 50, 0);

            // btnLogin
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.Font = new System.Drawing.Font("Tahoma", 14, System.Drawing.FontStyle.Bold);
            this.btnLogin.BackColor = System.Drawing.Color.Black;
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLogin.Height = 45;
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlButton.Controls.Add(this.btnLogin);

            // Add Controls to Right Panel
            this.pnlRight.Controls.Add(this.lblForgotPass);
            this.pnlRight.Controls.Add(this.pnlButton);
            this.pnlRight.Controls.Add(this.pnlFormContainer);
            this.pnlRight.Controls.Add(this.lblTitle);

            // Add Panels to Form
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);

            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.pnlFormContainer.ResumeLayout(false);
            this.pnlUser.ResumeLayout(false);
            this.pnlUser.PerformLayout();
            this.pnlPass.ResumeLayout(false);
            this.pnlPass.PerformLayout();
            this.pnlButton.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.FlowLayoutPanel pnlFormContainer;
        
        private System.Windows.Forms.Panel pnlUser;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtUsername;
        
        private System.Windows.Forms.Panel pnlPass;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox txtPassword;
        
        private System.Windows.Forms.Label lblForgotPass;
        private System.Windows.Forms.Panel pnlButton;
        private System.Windows.Forms.Button btnLogin;
    }
}