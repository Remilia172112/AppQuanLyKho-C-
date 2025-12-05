using System;

namespace src.DTO
{
    public class ChiTietPhieuKiemKeDTO
    {
        public int MPKK { get; set; }
        public int MSP { get; set; }
        public int TRANGTHAISP { get; set; }
        public string GHICHU { get; set; } 

        public ChiTietPhieuKiemKeDTO()
        {
        }

        public ChiTietPhieuKiemKeDTO(int mpkk, int msp, int trangthaisp, string ghichu)
        {
            MPKK = mpkk;
            MSP = msp;
            TRANGTHAISP = trangthaisp;
            GHICHU = ghichu;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = 23 * hash + MPKK.GetHashCode();
                hash = 23 * hash + MSP.GetHashCode();
                hash = 23 * hash + TRANGTHAISP.GetHashCode();
                hash = 23 * hash + GHICHU.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            ChiTietPhieuKiemKeDTO other = (ChiTietPhieuKiemKeDTO)obj;

            if (MPKK != other.MPKK) return false;
            if (MSP != other.MSP) return false;
            if (TRANGTHAISP != other.TRANGTHAISP) return false;
            return GHICHU == other.GHICHU;
        }

        public override string ToString()
        {
            return $"ChiTietPhieuKiemKeDTO{{ MPKK={MPKK}, MSP={MSP}, TRANGTHAISP={TRANGTHAISP}, GHICHU={GHICHU} }}";
        }
    }
}