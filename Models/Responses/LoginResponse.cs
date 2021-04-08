using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Models.Responses
{
    public class LoginResponse
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public string Token { get; set; }

        public LoginResponse(int userId, string email, bool isAdmin, string token)
        {
            UserID = userId;
            Email = email;
            IsAdmin = isAdmin;
            Token = token;
        }
    }
}
