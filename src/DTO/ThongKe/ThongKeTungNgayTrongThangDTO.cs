using System;

namespace src.DTO.ThongKe
{
    public class ThongKeTungNgayTrongThangDTO
    {
        public DateTime Ngay { get; set; }
        public int Chiphi { get; set; }
        public int Doanhthu { get; set; }
        public int Loinhuan { get; set; }

        public ThongKeTungNgayTrongThangDTO()
        {
        }

        public ThongKeTungNgayTrongThangDTO(DateTime ngay, int chiphi, int doanhthu, int loinhuan)
        {
            Ngay = ngay;
            Chiphi = chiphi;
            Doanhthu = doanhthu;
            Loinhuan = loinhuan;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 5;
                hash = 29 * hash + Ngay.GetHashCode();
                hash = 29 * hash + Chiphi.GetHashCode();
                hash = 29 * hash + Doanhthu.GetHashCode();
                hash = 29 * hash + Loinhuan.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ThongKeTungNgayTrongThangDTO other = (ThongKeTungNgayTrongThangDTO)obj;

            if (Chiphi != other.Chiphi) return false;
            if (Doanhthu != other.Doanhthu) return false;
            if (Loinhuan != other.Loinhuan) return false;
            return Ngay == other.Ngay;
        }

        public override string ToString()
        {
            return $"ThongKeTungNgayTrongThangDTO{{ ngay={Ngay}, chiphi={Chiphi}, doanhthu={Doanhthu}, loinhuan={Loinhuan} }}";
        }
    }
}