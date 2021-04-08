using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Models.Responses
{
    public class SupplierResponse
    {
        public int SupplierID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
