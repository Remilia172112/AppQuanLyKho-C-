using System;
using System.Windows.Forms;
using src.GUI;

namespace src
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread] // Bắt buộc đối với ứng dụng WinForms
        static void Main()
        {
            // 1. Cấu hình Encoding (Để sửa lỗi font/encoding khi xuất PDF bằng iTextSharp sau này)
            // Yêu cầu cài NuGet: System.Text.Encoding.CodePages
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // 2. Cấu hình giao diện Windows
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 3. (Tùy chọn) Cấu hình DPI cho màn hình độ phân giải cao (NET Core/NET 5+)
            // Application.SetHighDpiMode(HighDpiMode.SystemAware);

            // 4. Chạy Form Đăng Nhập đầu tiên
            try 
            {
                Application.Run(new LoginPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi chạy ứng dụng: " + ex.Message, "Lỗi Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}