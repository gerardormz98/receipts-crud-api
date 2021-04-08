using SimpleCrudAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly SimpleCrudDBContext _context;

        public SupplierRepository(SimpleCrudDBContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            var supplier = Get(id);

            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
            }
        }

        public Supplier Get(int id)
        {
            return _context.Suppliers.FirstOrDefault(s => s.ID == id);
        }

        public IEnumerable<Supplier> GetAll()
        {
            return _context.Suppliers.ToList();
        }

        public void Insert(Supplier p)
        {
            _context.Suppliers.Add(p);
            _context.SaveChanges();
        }

        public Supplier Update(int id, Supplier s)
        {
            var supplier = Get(id);

            supplier.Name = s.Name;
            supplier.Phone = s.Phone;

            _context.SaveChanges();
            return supplier;
        }
    }
}
