using System;

namespace src.DTO
{
    public class ChiTietPhieuKiemKeDTO
    {
        public int MPKK { get; set; }
        public int MSP { get; set; }
        
        // ⚠️ CRITICAL WARNING - MISLEADING COLUMN NAME ⚠️
        // Database column: TRANGTHAISP (suggests "Product Status")
        // Actual data: QUANTITY after audit (Số lượng thực tế sau kiểm kê)
        // 
        // PROBLEM:
        // - The column name "TRANGTHAISP" literally means "Product Status" in Vietnamese
        // - However, it actually stores the QUANTITY found during inventory audit, NOT status
        // - This causes confusion for developers who expect status values (e.g., 0/1/2)
        // 
        // WHY THIS HAPPENED:
        // - Likely a naming mistake during initial database design
        // - Should have been named "SOLUONGTHUCTE" (Actual Quantity) or "SLKIEMKE" (Audit Quantity)
        // 
        // CURRENT FIX:
        // - KEEP database column name unchanged to avoid breaking existing data
        // - USE clear variable names in code: soLuongThucTe, tonThucTe, actualQuantity
        // - ADD explicit comments wherever this property is used
        // - VALIDATE that values are non-negative integers representing quantity
        // 
        // USAGE EXAMPLE:
        //   int soLuongThucTe = chiTiet.TRANGTHAISP;  // Actual quantity found
        //   int tonHienTai = sanPham.SL;              // Current inventory
        //   int chenhLech = soLuongThucTe - tonHienTai; // Difference
        //
        // See also: PhieuKiemKeDAO.cs line 197-199 for correct usage
        public int TRANGTHAISP { get; set; }  // STORES QUANTITY, NOT STATUS!
        
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