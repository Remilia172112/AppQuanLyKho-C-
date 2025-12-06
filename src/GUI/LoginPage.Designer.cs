namespace src.GUI
{
    partial class LoginPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pbImage = new System.Windows.Forms.PictureBox(); // Đã đổi thành PictureBox
            this.pnlRight = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlFormContainer = new System.Windows.Forms.FlowLayoutPanel();
            
            // Custom Components
            this.txtUsername = new src.GUI.Component.InputForm("Tên đăng nhập");
            this.txtPassword = new src.GUI.Component.InputForm("Mật khẩu", "password");
            
            this.lblForgotPass = new System.Windows.Forms.Label();
            this.lblRegister = new System.Windows.Forms.Label();
            this.pnlButton = new System.Windows.Forms.Panel();
            this.btnLogin = new System.Windows.Forms.Button();

            // Bắt đầu khởi tạo Form
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.pnlRight.SuspendLayout();
            this.pnlButton.SuspendLayout();
            this.SuspendLayout();

            // 
            // 1. Cấu hình Form chính
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 500);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "Đăng nhập";

            // 
            // 2. Panel Trái (Chứa Ảnh)
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.White;
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Size = new System.Drawing.Size(500, 500);
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(10, 3, 5, 5);
            
            // Cấu hình PictureBox
            this.pbImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom; // Co giãn ảnh vừa khung
            
            try 
            { 
                // Đổi tên file logo của bạn ở đây
                string imgPath = "img/login-image.svg"; 

                if (System.IO.File.Exists(imgPath))
                {
                    // Kiểm tra đuôi file
                    if (imgPath.EndsWith(".svg", System.StringComparison.OrdinalIgnoreCase))
                    {
                        // Xử lý file SVG
                        var svgDocument = Svg.SvgDocument.Open(imgPath);
                        
                        // Chuyển đổi SVG thành Bitmap (Ảnh nén) để PictureBox hiển thị được
                        // Mẹo: Vẽ ảnh to hơn khung một chút để khi scale xuống nó nét căng
                        this.pbImage.Image = svgDocument.Draw(this.pnlLeft.Width, this.pnlLeft.Height);
                    }
                    else
                    {
                        // Xử lý file ảnh thường (PNG, JPG)
                        this.pbImage.Image = System.Drawing.Image.FromFile(imgPath);
                    }
                }
            }
            catch (System.Exception ex)
            {
                // Nếu lỗi thì in ra Console để biết (hoặc bỏ qua)
                System.Console.WriteLine("Lỗi load ảnh: " + ex.Message);
            }

            this.pnlLeft.Controls.Add(this.pbImage);

            // 
            // 3. Panel Phải (Chứa Form đăng nhập)
            // 
            this.pnlRight.BackColor = System.Drawing.Color.White;
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Padding = new System.Windows.Forms.Padding(20);

            // Tiêu đề
            this.lblTitle.Text = "ĐĂNG NHẬP VÀO HỆ THỐNG";
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 18, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Height = 80;

            // Container chứa 2 ô nhập liệu (Dùng FlowLayout để xếp dọc)
            this.pnlFormContainer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlFormContainer.BackColor = System.Drawing.Color.White;
            this.pnlFormContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFormContainer.Height = 180;
            this.pnlFormContainer.Padding = new System.Windows.Forms.Padding(40, 10, 0, 0); // Canh lề trái 40px

            // Cấu hình kích thước cho InputForm
            this.txtUsername.Size = new System.Drawing.Size(380, 70);
            this.txtPassword.Size = new System.Drawing.Size(380, 70);
            
            this.pnlFormContainer.Controls.Add(this.txtUsername);
            this.pnlFormContainer.Controls.Add(this.txtPassword);

            // Link Quên mật khẩu
            this.lblForgotPass.Text = "Quên mật khẩu?";
            this.lblForgotPass.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Underline);
            this.lblForgotPass.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblForgotPass.AutoSize = true;
            this.lblForgotPass.Location = new System.Drawing.Point(60, 280);

            // Link Đăng ký
            this.lblRegister.Text = "Đăng ký tài khoản?";
            this.lblRegister.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Underline);
            this.lblRegister.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblRegister.AutoSize = true;
            this.lblRegister.Location = new System.Drawing.Point(320, 280);

            // Panel chứa nút Đăng nhập (nằm dưới cùng)
            this.pnlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButton.Height = 120;
            this.pnlButton.Padding = new System.Windows.Forms.Padding(50, 20, 50, 40);

            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.Font = new System.Drawing.Font("Tahoma", 14, System.Drawing.FontStyle.Bold);
            this.btnLogin.BackColor = System.Drawing.Color.Black;
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Dock = System.Windows.Forms.DockStyle.Top; // Nút nằm trên cùng của panel bottom
            this.btnLogin.Height = 45;
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            
            this.pnlButton.Controls.Add(this.btnLogin);

            // Thêm các control vào Panel Phải
            this.pnlRight.Controls.Add(this.lblRegister);
            this.pnlRight.Controls.Add(this.lblForgotPass);
            this.pnlRight.Controls.Add(this.pnlButton);
            this.pnlRight.Controls.Add(this.pnlFormContainer);
            this.pnlRight.Controls.Add(this.lblTitle);

            // Thêm 2 Panel chính vào Form
            // Thứ tự quan trọng: Add Right trước (Fill) sau đó Left (Dock Left) sẽ bị che, 
            // nên ta Add Left trước, hoặc dùng BringToFront.
            // Trong WinForms Dock: Cái nào Dock Fill thì add cuối cùng trong code.
            this.Controls.Add(this.pnlRight); 
            this.Controls.Add(this.pnlLeft);

            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.pnlButton.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        // Khai báo biến
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.PictureBox pbImage; // Dùng PictureBox
        
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.FlowLayoutPanel pnlFormContainer;
        
        // Custom Controls
        private src.GUI.Component.InputForm txtUsername;
        private src.GUI.Component.InputForm txtPassword;
        
        private System.Windows.Forms.Label lblForgotPass;
        private System.Windows.Forms.Label lblRegister;
        private System.Windows.Forms.Panel pnlButton;
        private System.Windows.Forms.Button btnLogin;
    }
}