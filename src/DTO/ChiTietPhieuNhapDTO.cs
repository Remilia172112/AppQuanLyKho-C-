using System;

namespace src.DTO
{
    public class ChiTietPhieuNhapDTO
    {
        public int MPN { get; set; }
        public int MSP { get; set; }
        public int SL { get; set; }
        public int TIENNHAP { get; set; }
        public int HINHTHUC { get; set; }

        public ChiTietPhieuNhapDTO() { }

        public ChiTietPhieuNhapDTO(int mpn, int msp, int sl, int tiennhap, int hinhthuc)
        {
            MPN = mpn;
            MSP = msp;
            SL = sl;
            TIENNHAP = tiennhap;
            HINHTHUC = hinhthuc;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            ChiTietPhieuNhapDTO other = (ChiTietPhieuNhapDTO)obj;
            return MPN == other.MPN && MSP == other.MSP;
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                return MPN.GetHashCode() + MSP.GetHashCode();
            }
        }
    }
}