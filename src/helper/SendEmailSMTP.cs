using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace src.Helper
{
    public class SendEmailSMTP
    {
        // 1. Hàm tạo mã OTP ngẫu nhiên (6 số)
        public static string GetOTP()
        {
            Random random = new Random();
            int otp = random.Next(100000, 999999);
            return otp.ToString();
        }

        // 2. Hàm gửi OTP qua Email
        public static void SendOTP(string emailTo, string otp)
        {
            // Cấu hình thông tin người gửi
            // LƯU Ý QUAN TRỌNG: 
            // password ở đây KHÔNG PHẢI MẬT KHẨU ĐĂNG NHẬP GMAIL
            // Mà phải là "APP PASSWORD" (Mật khẩu ứng dụng) 16 ký tự do Google cấp
            // Cách lấy: Google Account -> Security -> 2-Step Verification -> App passwords
            
            string fromEmail = "khoquanly152@gmail.com";
            string password = "oasp zkhs gibg blfy"; // Thay bằng App Password thực tế

            try
            {
                // Tạo đối tượng MailMessage
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromEmail);
                message.To.Add(new MailAddress(emailTo));
                message.Subject = "OTP Xác thực";
                message.IsBodyHtml = true; // Cho phép nội dung HTML

                // Nội dung HTML (Đã sửa lại cú pháp string của C# cho đẹp hơn)
                string htmlContent = $@"
                    <div style='font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2'>
                        <div style='margin:50px auto;width:70%;padding:20px 0'>
                            <div style='border-bottom:1px solid #eee'>
                                <a href='#' style='font-size:1.4em;color: #00466a;text-decoration:none;font-weight:600'>Quản lý kho hàng</a>
                            </div>
                            <p style='font-size:1.1em'>Hi,</p>
                            <p>Yêu cầu thay đổi mật khẩu: Vui lòng nhập mã OTP sau để xác thực. Tuyệt đối không chia sẻ mã này với người khác.</p>
                            <h2 style='background: #00466a;margin: 0 auto;width: max-content;padding: 0 10px;color: #fff;border-radius: 4px;'>{otp}</h2>
                            <p style='font-size:0.9em;'>Trân trọng,<br />QA</p>
                            <hr style='border:none;border-top:1px solid #eee' />
                            <div style='float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300'>
                                <p>App Quản lý kho hàng</p>
                                <p>Số 273 An Dương Vương, Phường Chợ Quán, TP. HCM</p>
                                <p>Việt Nam</p>
                            </div>
                        </div>
                    </div>";

                message.Body = htmlContent;

                // Cấu hình SMTP Client (Gmail)
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.EnableSsl = true; // Bắt buộc bật SSL
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromEmail, password);

                // Gửi thư
                smtp.Send(message);
                
                // (Tùy chọn) Thông báo thành công nếu cần, hoặc để form gọi hàm này tự thông báo
                // MessageBox.Show("Đã gửi OTP thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // In lỗi ra console hoặc hiện thông báo
                Console.WriteLine("Lỗi gửi Email: " + ex.Message);
                MessageBox.Show("Không thể gửi Email. Vui lòng kiểm tra kết nối mạng hoặc mật khẩu ứng dụng.\nLỗi: " + ex.Message, "Lỗi Gửi Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}