using SimpleCrudAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly SimpleCrudDBContext _context;

        public ReceiptRepository(SimpleCrudDBContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            var receipt = Get(id);

            if (receipt != null)
            {
                _context.Receipts.Remove(receipt);
                _context.SaveChanges();
            }
        }

        public Receipt Get(int id)
        {
            return _context.Receipts.Include(r => r.Supplier).FirstOrDefault(r => r.ID == id);
        }

        public IEnumerable<Receipt> GetAll()
        {
            return _context.Receipts.Include(r => r.Supplier).ToList();
        }

        public void Insert(Receipt r)
        {
            _context.Receipts.Add(r);
            _context.SaveChanges();
        }

        public Receipt Update(int id, Receipt r)
        {
            var receipt = Get(id);

            receipt.Amount = r.Amount;
            receipt.Comments = r.Comments;
            receipt.SupplierID = r.SupplierID;

            _context.SaveChanges();
            return receipt;
        }
    }
}
