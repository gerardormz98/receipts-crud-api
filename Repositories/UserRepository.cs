using SimpleCrudAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SimpleCrudDBContext _context;

        public UserRepository(SimpleCrudDBContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            var user = Get(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public User Get(int id)
        {
            return _context.Users.FirstOrDefault(u => u.ID == id);
        }

        public User Get(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public void Insert(User u)
        {
            _context.Users.Add(u);
            _context.SaveChanges();
        }

        public User Update(int id, User u)
        {
            var user = Get(id);

            user.IsAdmin = u.IsAdmin;

            _context.SaveChanges();
            return user;
        }
    }
}
