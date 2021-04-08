using Microsoft.AspNetCore.Http;
using SimpleCrudAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Security
{
    public interface ISecurityHelper
    {
        public User GetCurrentUser(HttpContext ctx);
        public string GetCurrentUserToken(HttpContext ctx);
    }
}
