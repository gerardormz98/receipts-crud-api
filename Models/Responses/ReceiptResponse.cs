using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Models.Responses
{
    public class ReceiptResponse
    {
        public int ReceiptID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
        public SupplierResponse Supplier { get; set; }
    }
}
