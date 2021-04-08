using Microsoft.AspNetCore.Http;
using SimpleCrudAPI.Models;
using SimpleCrudAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Security
{
    public class SecurityHelper : ISecurityHelper
    {
        private readonly IUserRepository _repo;

        public SecurityHelper(IUserRepository repo)
        {
            _repo = repo;
        }

        private string GetUserEmail(HttpContext context)
        {
            return context.User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Select(x => x.Value).FirstOrDefault();
        }

        public User GetCurrentUser(HttpContext context)
        {
            User user = null;

            try
            {
                user = _repo.Get(GetUserEmail(context));
            }
            catch
            { }

            return user;
        }

        public string GetCurrentUserToken(HttpContext context)
        {
            string authHeader = context.Request.Headers.Where(h => h.Key == "Authorization").FirstOrDefault().Value;
            return authHeader.Split(' ')[1];
        }
    }
}
