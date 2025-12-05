using System;
using System.Reflection;

namespace src.DTO
{
    public class NhanVienDTO
    {
        public int MNV { get; set; }
        public string HOTEN { get; set; }
        public int GIOITINH { get; set; }
        public string SDT { get; set; }
        public DateTime NGAYSINH{ get; set; }
        public int TT { get; set; }
        public string EMAIL { get; set; }

        public NhanVienDTO()
        {
        }

        public NhanVienDTO(int MNV, string HOTEN, int GIOITINH, DateTime NGAYSINH, string SDT, int TT, string EMAIL)
        {
            this.MNV = MNV;
            this.HOTEN = HOTEN;
            this.GIOITINH = GIOITINH;
            this.NGAYSINH= NGAYSINH;
            this.SDT = SDT;
            this.TT = TT;
            this.EMAIL = EMAIL;
        }

        public NhanVienDTO(string HOTEN, int GIOITINH, DateTime NGAYSINH, string SDT, int TT)
        {
            this.HOTEN = HOTEN;
            this.GIOITINH = GIOITINH;
            this.NGAYSINH= NGAYSINH;
            this.SDT = SDT;
            this.TT = TT;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 7;
                hash = 17 * hash + MNV.GetHashCode();
                hash = 17 * hash + (HOTEN != null ? HOTEN.GetHashCode() : 0);
                hash = 17 * hash + GIOITINH.GetHashCode();
                hash = 17 * hash + NGAYSINH.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null) return false;
            if (GetType() != obj.GetType()) return false;

            NhanVienDTO other = (NhanVienDTO)obj;
            
            if (MNV != other.MNV) return false;
            if (GIOITINH != other.GIOITINH) return false;
            if (HOTEN != other.HOTEN) return false;
            return NGAYSINH== other.NGAYSINH;
        }

        public override string ToString()
        {
            return $"NhanVien{{ MNV={MNV}, HOTEN={HOTEN}, GIOITINH={GIOITINH}, ngaysinh={NGAYSINH} }}";
        }

        public int GetColumnCount()
        {
            // Đếm số lượng Property (thuộc tính) trong class
            return this.GetType().GetProperties().Length;
        }
    }
}