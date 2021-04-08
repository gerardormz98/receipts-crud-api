using SimpleCrudAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public void Delete(int id);
        public User Get(string email);
    }
}
