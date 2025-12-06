using System;
using System.Drawing;
using System.Windows.Forms;

namespace src.GUI.Component
{
    public class InputForm : UserControl
    {
        private Label lblTitle;
        public TextBox txtForm; // Public để dễ truy cập sự kiện từ bên ngoài
        public TextBox txtPass; // Public để dễ truy cập sự kiện từ bên ngoài

        // 1. Constructor mặc định
        public InputForm()
        {
            InitCommon();
        }

        // 2. Constructor nhập văn bản thường
        public InputForm(string title)
        {
            InitCommon();
            SetupLayout(title, false);
        }

        // 3. Constructor nhập mật khẩu (style="password")
        public InputForm(string title, string style)
        {
            InitCommon();
            if (style == "password")
            {
                SetupLayout(title, true);
            }
            else
            {
                SetupLayout(title, false);
            }
        }

        // 4. Constructor có kích thước
        public InputForm(string title, int w, int h)
        {
            InitCommon();
            this.Size = new Size(w, h);
            SetupLayout(title, false);
        }

        // Hàm khởi tạo chung
        private void InitCommon()
        {
            this.BackColor = Color.White;
            // BorderEmpty(0, 10, 5, 10) bên Java -> Padding bên C#
            this.Padding = new Padding(10, 0, 10, 5); 
            this.Size = new Size(200, 60); // Chiều cao mặc định để chứa đủ 2 dòng
        }

        // Hàm dựng giao diện (Thay cho GridLayout)
        private void SetupLayout(string title, bool isPassword)
        {
            // 1. Tạo Label (Tiêu đề)
            lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Dock = DockStyle.Top; // Dính lên trên
            lblTitle.Height = 25; // Chiều cao label
            lblTitle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblTitle.TextAlign = ContentAlignment.BottomLeft; // Canh lề dưới trái

            // 2. Tạo TextBox
            if (isPassword)
            {
                txtPass = new TextBox();
                txtPass.Dock = DockStyle.Top; // Dính ngay dưới Label
                txtPass.Height = 30;
                txtPass.Font = new Font("Segoe UI", 12, FontStyle.Regular);
                txtPass.UseSystemPasswordChar = true; // Ẩn ký tự mật khẩu
                
                // Thêm vào Panel (Thứ tự thêm quan trọng khi dùng Dock)
                // Trong WinForms, Dock Top được xếp từ dưới lên nếu add sau, nên ta add Pass trước rồi Title sau để Title nằm trên
                this.Controls.Add(txtPass);
            }
            else
            {
                txtForm = new TextBox();
                txtForm.Dock = DockStyle.Top;
                txtForm.Height = 30;
                txtForm.Font = new Font("Segoe UI", 12, FontStyle.Regular);
                
                this.Controls.Add(txtForm);
            }

            this.Controls.Add(lblTitle);
        }

        // --- Các phương thức Setter / Getter giống Java ---

        public void setTitle(string title)
        {
            if (lblTitle != null) lblTitle.Text = title;
        }

        public string getPass()
        {
            return txtPass != null ? txtPass.Text : "";
        }

        public string getText()
        {
            return txtForm != null ? txtForm.Text : "";
        }

        public void setText(string content)
        {
            if (txtForm != null) txtForm.Text = content;
        }

        public void setPass(string s)
        {
            if (txtPass != null) txtPass.Text = s;
        }

        public void setDisablePass()
        {
            if (txtPass != null) txtPass.Enabled = false;
        }

        public void setDisable()
        {
            if (txtForm != null) txtForm.Enabled = false;
        }

        public void setEditable(bool value)
        {
            // Trong C#, setEditable(false) tương đương ReadOnly = true
            if (txtForm != null) txtForm.ReadOnly = !value;
        }

        // Getter cho các thành phần (để add sự kiện KeyListener bên ngoài)
        public TextBox getTxtForm()
        {
            return txtForm;
        }

        public TextBox getTxtPass()
        {
            return txtPass;
        }
    }
}