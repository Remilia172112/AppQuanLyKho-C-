using System.Collections.Generic;

namespace src.DAO
{
    public interface DAOinterface<T>
    {
        int insert(T t);
        int update(T t);
        int delete(string t);
        List<T> selectAll();
        T selectById(string t);
        int getAutoIncrement();
    }
}