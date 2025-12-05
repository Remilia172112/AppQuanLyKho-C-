using System;
using System.Collections.Generic;
using System.Linq;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class PhieuXuatBUS
    {
        private readonly PhieuXuatDAO phieuXuatDAO = PhieuXuatDAO.Instance;
        private readonly ChiTietPhieuXuatDAO chiTietPhieuXuatDAO = ChiTietPhieuXuatDAO.Instance;
        private List<PhieuXuatDTO> listPhieuXuat;

        private readonly NhanVienBUS nvBUS = new NhanVienBUS();
        private readonly KhachHangBUS khBUS = new KhachHangBUS();

        public PhieuXuatBUS()
        {
            this.listPhieuXuat = phieuXuatDAO.selectAll();
        }

        public List<PhieuXuatDTO> GetAll()
        {
            this.listPhieuXuat = phieuXuatDAO.selectAll();
            return this.listPhieuXuat;
        }

        public List<PhieuXuatDTO> GetAll(int mkh)
        {
            this.listPhieuXuat = phieuXuatDAO.selectByMKH(mkh.ToString());
            return this.listPhieuXuat;
        }

        public PhieuXuatDTO GetSelect(int index)
        {
            return listPhieuXuat[index];
        }

        public void Cancel(int px)
        {
            phieuXuatDAO.cancelPhieuXuat(px);
        }

        public void Remove(int px)
        {
            // Xóa theo index trong list
            listPhieuXuat.RemoveAt(px);
        }

        public void Update(PhieuXuatDTO px)
        {
            phieuXuatDAO.update(px);
            // Hàm updateSL trong DAO Java gọi lại selectAll để tính toán, trong C# bạn có thể cần kiểm tra lại logic này
            // Ở đây mình gọi lại hàm updateSL tương tự
            chiTietPhieuXuatDAO.updateSL(px.MPX.ToString());
        }

        // Insert Phiếu Xuất (Trừ kho)
        public void Insert(PhieuXuatDTO px, List<ChiTietPhieuXuatDTO> ct)
        {
            // Insert phiếu xuất
            phieuXuatDAO.insert(px);
            
            // Cập nhật MPX cho chi tiết phiếu (nếu ID tự tăng thì phải lấy lại ID mới)
            // Giả sử px.MPX đã đúng
            foreach(var item in ct)
            {
                item.MPX = px.MPX;
            }

            // Insert chi tiết (đã bao gồm logic trừ kho trong DAO)
            chiTietPhieuXuatDAO.insert(ct);
        }

        // Insert Giỏ hàng tạm (Không trừ kho?) - Theo logic code cũ
        public void InsertGH(PhieuXuatDTO px, List<ChiTietPhieuXuatDTO> ct)
        {
            phieuXuatDAO.insert(px);
            chiTietPhieuXuatDAO.insertGH(ct);
        }

        public List<ChiTietPhieuXuatDTO> SelectCTP(int maphieu)
        {
            return chiTietPhieuXuatDAO.selectAll(maphieu.ToString());
        }

        public int GetMPMAX()
        {
            List<PhieuXuatDTO> list = phieuXuatDAO.selectAll();
            int s = 1;
            foreach (PhieuXuatDTO i in list)
            {
                if (i.MPX > s) s = i.MPX;
            }
            return s;
        }

        public ChiTietPhieuXuatDTO FindCT(List<ChiTietPhieuXuatDTO> ctphieu, int masp)
        {
            foreach (var item in ctphieu)
            {
                if (item.MSP == masp)
                {
                    return item;
                }
            }
            return null;
        }

        // Lọc Phiếu Xuất (Filter)
        public List<PhieuXuatDTO> FillerPhieuXuat(int type, string input, int makh, int manv, DateTime timeStart, DateTime timeEnd, string priceMinStr, string priceMaxStr)
        {
            long priceMin = !string.IsNullOrEmpty(priceMinStr) ? long.Parse(priceMinStr) : 0;
            long priceMax = !string.IsNullOrEmpty(priceMaxStr) ? long.Parse(priceMaxStr) : long.MaxValue;

            // Set timeEnd về cuối ngày
            DateTime timeEndFixed = new DateTime(timeEnd.Year, timeEnd.Month, timeEnd.Day, 23, 59, 59);

            List<PhieuXuatDTO> result = new List<PhieuXuatDTO>();
            input = input.ToLower();

            foreach (PhieuXuatDTO phieuXuat in GetAll())
            {
                bool match = false;
                switch (type)
                {
                    case 0: // Tất cả
                        if (phieuXuat.MPX.ToString().Contains(input) || 
                            khBUS.GetTenKhachHang(phieuXuat.MKH).ToLower().Contains(input) || 
                            nvBUS.GetNameById(phieuXuat.MNV).ToLower().Contains(input))
                        {
                            match = true;
                        }
                        break;
                    case 1: // Mã phiếu
                        if (phieuXuat.MPX.ToString().Contains(input))
                        {
                            match = true;
                        }
                        break;
                    case 2: // Khách hàng
                        if (khBUS.GetTenKhachHang(phieuXuat.MKH).ToLower().Contains(input))
                        {
                            match = true;
                        }
                        break;
                    case 3: // Nhân viên
                        if (nvBUS.GetNameById(phieuXuat.MNV).ToLower().Contains(input))
                        {
                            match = true;
                        }
                        break;
                }

                if (match &&
                    (manv == 0 || phieuXuat.MNV == manv) &&
                    (makh == 0 || phieuXuat.MKH == makh) &&
                    (phieuXuat.TG >= timeStart) &&
                    (phieuXuat.TG <= timeEndFixed) &&
                    (phieuXuat.TIEN >= priceMin) &&
                    (phieuXuat.TIEN <= priceMax))
                {
                    result.Add(phieuXuat);
                }
            }
            return result;
        }

        public string[] GetArrMPX()
        {
            int size = listPhieuXuat.Count;
            string[] result = new string[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = listPhieuXuat[i].MPX.ToString();
            }
            return result;
        }

        public PhieuXuatDTO GetByIndex(int index)
        {
            return this.listPhieuXuat[index];
        }

        // Kiểm tra số lượng tồn kho trước khi xuất (Dựa trên List chi tiết truyền vào)
        public bool CheckSLPx(List<ChiTietPhieuXuatDTO> listctpx)
        {
            SanPhamBUS spBus = new SanPhamBUS();
            List<SanPhamDTO> spList = new List<SanPhamDTO>();
            
            // Lấy thông tin sản phẩm hiện tại
            foreach(var i in listctpx)
            {
                spList.Add(spBus.GetByIndex(spBus.GetIndexByMaSP(i.MSP)));
            }

            // So sánh
            for (int i = 0; i < spList.Count; i++)
            {
                if(listctpx[i].SL > spList[i].SL)
                {
                    return false;
                }
            }
            return true;
        }

        // Kiểm tra số lượng tồn kho (Dựa trên Mã phiếu đã lưu)
        public bool CheckSLPx(int maphieu)
        {
            return phieuXuatDAO.checkSLPx(maphieu);
        }

        public int CancelPhieuXuat(int maphieu)
        {
            return phieuXuatDAO.cancelPhieuXuat(maphieu);
        }
    }
}