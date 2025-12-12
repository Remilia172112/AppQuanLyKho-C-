using System;
using System.Collections.Generic;
using src.DAO;
using src.DTO;

namespace src.BUS
{
    public class KhuVucKhoBUS
    {
        private readonly KhuVucKhoDAO kvkDAO = KhuVucKhoDAO.Instance;
        public List<KhuVucKhoDTO> listKVK = new List<KhuVucKhoDTO>();

        public KhuVucKhoBUS()
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                listKVK = kvkDAO.selectAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                listKVK = new List<KhuVucKhoDTO>();
            }
        }


        // Phương thức getInstance (nếu cần dùng kiểu Singleton, nhưng class này có public constructor)
        public static KhuVucKhoBUS GetInstance()
        {
            return new KhuVucKhoBUS();
        }

        public List<KhuVucKhoDTO> GetAll()
        {
            return this.listKVK;
        }

        public KhuVucKhoDTO GetByIndex(int index)
        {
            return this.listKVK[index];
        }

        // Hàm này bị trùng lặp logic với GetIndexByMaKVK, mình giữ lại và đổi tên cho rõ nghĩa
        // Trong Java: getIndexByMaLH (chắc copy paste chưa sửa tên)
        public int GetIndexByMaKhuVuc(int makhuvuc)
        {
            return listKVK.FindIndex(kvk => kvk.MKVK == makhuvuc);
        }

        public bool Add(KhuVucKhoDTO kvk)
        {
            if (kvkDAO.insert(kvk) != 0)
            {
                LoadData(); // Reload để đồng bộ với DB
                return true;
            }
            return false;
        }

        public bool Delete(KhuVucKhoDTO kvk)
        {
            bool check = kvkDAO.delete(kvk.MKVK.ToString()) != 0;
            if (check)
            {
                listKVK.Remove(kvk);
            }
            return check;
        }

        public bool Update(KhuVucKhoDTO kvk)
        {
            bool check = kvkDAO.update(kvk) != 0;
            if (check)
            {
                int index = GetIndexByMaKhuVuc(kvk.MKVK);
                if (index != -1)
                {
                    this.listKVK[index] = kvk;
                }
            }
            return check;
        }

        public List<KhuVucKhoDTO> Search(string txt, string type)
        {
            txt = txt.ToLower();
            IEnumerable<KhuVucKhoDTO> query = listKVK;

            switch (type)
            {
                case "Mã khu vực kho":
                    query = query.Where(kvk => kvk.MKVK.ToString().Contains(txt));
                    break;
                case "Tên khu vực kho":
                    query = query.Where(kvk => kvk.TEN.ToLower().Contains(txt));
                    break;
                default: // Tất cả
                    query = query.Where(kvk =>
                        kvk.MKVK.ToString().Contains(txt) ||
                        kvk.TEN.ToLower().Contains(txt));
                    break;
            }
            return query.ToList();
        }

        public string[] GetArrTenKhuVuc()
        {
            return listKVK.Select(kvk => kvk.TEN).ToArray();
        }

        public string GetTenKhuVuc(int makhuvuc)
        {
            return listKVK.FirstOrDefault(kvk => kvk.MKVK == makhuvuc)?.TEN ?? "";
        }
    }
}