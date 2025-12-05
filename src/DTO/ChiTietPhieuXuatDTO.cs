using System;

namespace src.DTO
{
    public class ChiTietPhieuXuatDTO
    {
        public int MPX { get; set; }
        public int MSP { get; set; }
        public int MKM { get; set; }
        public int SL { get; set; }
        public int TIENXUAT { get; set; }

        public ChiTietPhieuXuatDTO() { }

        public ChiTietPhieuXuatDTO(int mpx, int msp, int mkm, int sl, int tienxuat)
        {
            MPX = mpx;
            MSP = msp;
            MKM = mkm;
            SL = sl;
            TIENXUAT = tienxuat;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            ChiTietPhieuXuatDTO other = (ChiTietPhieuXuatDTO)obj;
            return MPX == other.MPX && MSP == other.MSP;
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                return MPX.GetHashCode() + MSP.GetHashCode();
            }
        }
    }
}