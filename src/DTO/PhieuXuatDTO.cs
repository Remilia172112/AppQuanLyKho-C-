using System;

namespace src.DTO
{
    public class PhieuXuatDTO
    {
        public int MPX { get; set; }
        public int MNV { get; set; }
        public int MKH { get; set; }
        public int TIEN { get; set; }
        public DateTime TG { get; set; }
        public int TT { get; set; }

        public PhieuXuatDTO() { }

        public PhieuXuatDTO(int mpx, int mnv, int mkh, int tien, DateTime tg, int tt)
        {
            MPX = mpx;
            MNV = mnv;
            MKH = mkh;
            TIEN = tien;
            TG = tg;
            TT = tt;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            PhieuXuatDTO other = (PhieuXuatDTO)obj;
            return MPX == other.MPX;
        }

        public override int GetHashCode()
        {
            return MPX.GetHashCode();
        }
    }
}