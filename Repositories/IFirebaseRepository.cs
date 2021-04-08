using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Repositories
{
    public interface IFirebaseRepository
    {
        public Task<FirebaseAuthLink> CreateUser(string email, string password);
        public Task<User> GetUser(string token);
        public Task DeleteUser(string token);
        public Task DeleteUserByCorreo(string email);
        public Task UpdateUser(string token, string name);
        public Task<string> GenerateUserToken(string email, string password);
        public Task SendResetPassword(string email);
    }
}
