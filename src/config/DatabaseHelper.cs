using System;
using System.Windows.Forms; // Để dùng MessageBox
using MySql.Data.MySqlClient; // Thư viện MySQL

namespace src.config
{
    public class DatabaseHelper
    {
        // Hàm lấy kết nối
        public static MySqlConnection GetConnection()
        {
            MySqlConnection result = null;
            try
            {
                // Cấu hình chuỗi kết nối (ConnectionString)
                // Lưu ý: Port mặc định MySQL là 3306
                string connString = "Server=localhost;Database=quanlykho;Port=3306;User Id=root;Password=1234;";

                // Tạo và mở kết nối
                result = new MySqlConnection(connString);
                result.Open();
            }
            catch (Exception e)
            {
                // Hiển thị lỗi nếu không kết nối được
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu!\nLỗi: " + e.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        // Hàm đóng kết nối
        public static void CloseConnection(MySqlConnection c)
        {
            try
            {
                // Kiểm tra nếu kết nối chưa đóng thì mới đóng
                if (c != null && c.State == System.Data.ConnectionState.Open)
                {
                    c.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}