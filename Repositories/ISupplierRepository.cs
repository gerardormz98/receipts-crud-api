using SimpleCrudAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Repositories
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        public void Delete(int id);
    }
}
