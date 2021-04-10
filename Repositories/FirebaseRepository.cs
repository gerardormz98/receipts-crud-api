using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Repositories
{
    public class FirebaseRepository : IFirebaseRepository
    {
        private readonly IFirebaseAuthProvider _authProvider;

        public FirebaseRepository(IFirebaseAuthProvider authProvider)
        {
            _authProvider = authProvider;
        }

        public async Task<User> GetUser(string token)
        {
            return await _authProvider.GetUserAsync(token);
        }

        public async Task<string> GenerateUserToken(string email, string password)
        {
            var auth = await _authProvider.SignInWithEmailAndPasswordAsync(email, password);

            return auth.FirebaseToken;
        }

        public async Task<FirebaseAuthLink> CreateUser(string email, string password)
        {
            return await _authProvider.CreateUserWithEmailAndPasswordAsync(email, password, null);
        }

        public async Task DeleteUser(string token)
        {
            await _authProvider.DeleteUser(token);
        }

        public async Task DeleteUserByEmail(string email)
        {
            var user = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
            await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.DeleteUserAsync(user.Uid);
        }

        public async Task UpdateUser(string token, string name)
        {
            await _authProvider.UpdateProfileAsync(token, name, null);
        }

        public async Task SendResetPassword(string email)
        {
            await _authProvider.SendPasswordResetEmailAsync(email);
        }
    }
}
