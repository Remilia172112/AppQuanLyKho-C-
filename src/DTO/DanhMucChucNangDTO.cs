using System;

namespace src.DTO
{
    public class DanhMucChucNangDTO
    {
        public string MCN { get; set; }
        public string TEN { get; set; }

        public DanhMucChucNangDTO()
        {
        }

        public DanhMucChucNangDTO(string MCN, string TEN)
        {
            this.MCN = MCN;
            this.TEN = TEN;
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = 7;
                hash = 37 * hash + (MCN != null ? MCN.GetHashCode() : 0);
                hash = 37 * hash + (TEN != null ? TEN.GetHashCode() : 0);
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

            DanhMucChucNangDTO other = (DanhMucChucNangDTO)obj;

            if (MCN != other.MCN)
            {
                return false;
            }
            return TEN == other.TEN;
        }

        public override string ToString()
        {
            return $"DanhMucChucNang{{ Ma chuc nang = {MCN}, Ten chuc nang = {TEN} }}";
        }
    }
}