using SimpleCrudAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Repositories
{
    public interface IReceiptRepository : IRepository<Receipt>
    {
        public void Delete(int id);
    }
}
