using System;

namespace src.DTO.ThongKe
{
    public class ThongKeKhachHangDTO
    {
        public int Makh { get; set; }
        public string Tenkh { get; set; }
        public int Soluongphieu { get; set; }
        public long Tongtien { get; set; }

        public ThongKeKhachHangDTO()
        {
        }

        public ThongKeKhachHangDTO(int makh, string tenkh, int soluongphieu, long tongtien)
        {
            Makh = makh;
            Tenkh = tenkh;
            Soluongphieu = soluongphieu;
            Tongtien = tongtien;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 7;
                hash = 29 * hash + Makh.GetHashCode();
                hash = 29 * hash + (Tenkh != null ? Tenkh.GetHashCode() : 0);
                hash = 29 * hash + Soluongphieu.GetHashCode();
                hash = 29 * hash + Tongtien.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ThongKeKhachHangDTO other = (ThongKeKhachHangDTO)obj;

            if (Makh != other.Makh) return false;
            if (Soluongphieu != other.Soluongphieu) return false;
            if (Tongtien != other.Tongtien) return false;
            return Tenkh == other.Tenkh;
        }

        public override string ToString()
        {
            return $"ThongKeKhachHangDTO{{ makh={Makh}, tenkh={Tenkh}, soluongphieu={Soluongphieu}, tongtien={Tongtien} }}";
        }
    }
}