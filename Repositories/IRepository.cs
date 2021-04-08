using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Repositories
{
    public interface IRepository<T>
    {
        public T Get(int id);
        public void Insert(T obj);
        public T Update(int id, T obj);
        public IEnumerable<T> GetAll();
    }
}
