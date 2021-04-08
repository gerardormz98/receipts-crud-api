using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Models
{
    public class Receipt
    {
        public int ID { get; set; }
        public int SupplierID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
        public int UserID { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual User User { get; set; }
    }
}
