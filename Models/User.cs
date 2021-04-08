using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }

        public virtual ICollection<Receipt> Receipt { get; set; }
    }
}
