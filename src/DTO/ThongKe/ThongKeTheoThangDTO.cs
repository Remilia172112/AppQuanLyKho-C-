using System;

namespace src.DTO.ThongKe
{
    public class ThongKeTheoThangDTO
    {
        public int Thang { get; set; }
        public int Chiphi { get; set; }
        public int Doanhthu { get; set; }
        public int Loinhuan { get; set; }

        public ThongKeTheoThangDTO()
        {
        }

        public ThongKeTheoThangDTO(int thang, int chiphi, int doanhthu, int loinhuan)
        {
            Thang = thang;
            Chiphi = chiphi;
            Doanhthu = doanhthu;
            Loinhuan = loinhuan;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 7;
                hash = 59 * hash + Thang.GetHashCode();
                hash = 59 * hash + Chiphi.GetHashCode();
                hash = 59 * hash + Doanhthu.GetHashCode();
                hash = 59 * hash + Loinhuan.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ThongKeTheoThangDTO other = (ThongKeTheoThangDTO)obj;

            if (Thang != other.Thang) return false;
            if (Chiphi != other.Chiphi) return false;
            if (Doanhthu != other.Doanhthu) return false;
            return Loinhuan == other.Loinhuan;
        }

        public override string ToString()
        {
            return $"ThongKeTheoThangDTO{{ thang={Thang}, chiphi={Chiphi}, doanhthu={Doanhthu}, loinhuan={Loinhuan} }}";
        }
    }
}