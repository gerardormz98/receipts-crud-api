using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Models.Responses
{
    public class UserResponse
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
