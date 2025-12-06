using System;
using System.Collections.Generic;
using src.DTO;

namespace src.GUI.Components
{
    /// <summary>
    /// Quản lý session người dùng đăng nhập và phân quyền
    /// </summary>
    public static class SessionManager
    {
        public static TaiKhoanDTO? CurrentUser { get; set; }
        public static NhanVienDTO? CurrentEmployee { get; set; }
        public static NhomQuyenDTO? CurrentRole { get; set; }
        public static List<ChiTietQuyenDTO> Permissions { get; set; } = new List<ChiTietQuyenDTO>();

        public static bool IsLoggedIn => CurrentUser != null;

        public static void Login(TaiKhoanDTO user, NhanVienDTO employee, NhomQuyenDTO role, List<ChiTietQuyenDTO> permissions)
        {
            CurrentUser = user;
            CurrentEmployee = employee;
            CurrentRole = role;
            Permissions = permissions;
        }

        public static void Logout()
        {
            CurrentUser = null;
            CurrentEmployee = null;
            CurrentRole = null;
            Permissions.Clear();
        }

        /// <summary>
        /// Kiểm tra quyền thực hiện hành động trên chức năng
        /// </summary>
        public static bool HasPermission(string machucnang, string hanhdong)
        {
            if (CurrentUser == null) return false;
            
            return Permissions.Exists(p => 
                p.Machucnang.Equals(machucnang, StringComparison.OrdinalIgnoreCase) && 
                p.Hanhdong.Equals(hanhdong, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Kiểm tra có quyền xem chức năng không
        /// </summary>
        public static bool CanView(string machucnang)
        {
            return HasPermission(machucnang, "view");
        }

        /// <summary>
        /// Kiểm tra có quyền tạo mới không
        /// </summary>
        public static bool CanCreate(string machucnang)
        {
            return HasPermission(machucnang, "create");
        }

        /// <summary>
        /// Kiểm tra có quyền cập nhật không
        /// </summary>
        public static bool CanUpdate(string machucnang)
        {
            return HasPermission(machucnang, "update");
        }

        /// <summary>
        /// Kiểm tra có quyền xóa không
        /// </summary>
        public static bool CanDelete(string machucnang)
        {
            return HasPermission(machucnang, "delete");
        }

        /// <summary>
        /// Kiểm tra có quyền duyệt không (approve - dành cho phiếu nhập/xuất/kiểm kê)
        /// </summary>
        public static bool CanApprove(string machucnang)
        {
            return HasPermission(machucnang, "approve");
        }
    }
}
