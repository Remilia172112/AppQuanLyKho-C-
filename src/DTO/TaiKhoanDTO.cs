using System;

namespace src.DTO
{
    public class TaiKhoanDTO
    {
        public int MNV { get; set; }
        public string MK { get; set; }
        public string TDN { get; set; }
        public int MNQ { get; set; }
        public int TT { get; set; }

        public TaiKhoanDTO()
        {
            
        }

        public TaiKhoanDTO(int MNV, string TDN, string MK, int MNQ, int TT)
        {
            this.MNV = MNV;
            this.TDN = TDN;
            this.MK = MK;
            this.MNQ = MNQ;
            this.TT = TT;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = 19 * hash + MNV.GetHashCode();
                hash = 19 * hash + (TDN != null ? TDN.GetHashCode() : 0);
                hash = 19 * hash + (MK != null ? MK.GetHashCode() : 0);
                hash = 19 * hash + MNQ.GetHashCode();
                hash = 19 * hash + TT.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            if (GetType() != obj.GetType())
            {
                return false;
            }
            
            TaiKhoanDTO other = (TaiKhoanDTO)obj;
            
            if (MNV != other.MNV) return false;
            if (MNQ != other.MNQ) return false;
            if (TT != other.TT) return false;
            
            if (TDN != other.TDN) return false;
            return MK == other.MK;
        }

        public override string ToString()
        {
            return $"AccountDTO{{ Ma nhan vien ={MNV}, Username ={TDN}, Mat khau={MK}, Ma nhom quyen={MNQ}, Trang thai={TT} }}";
        }
    }
}