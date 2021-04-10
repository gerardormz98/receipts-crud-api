using Firebase.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCrudAPI.Models.Requests;
using SimpleCrudAPI.Models.Responses;
using SimpleCrudAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerCommon
    {
        private readonly IFirebaseRepository _repo;
        private readonly IUserRepository _userRepo;

        public LoginController(IFirebaseRepository repo, IUserRepository userRepo)
        {
            _repo = repo;
            _userRepo = userRepo;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginParameters parameters)
        {
            try
            {
                var user = _userRepo.Get(parameters.Email);

                if (user == null)
                {
                    try
                    {
                        await _repo.DeleteUserByEmail(parameters.Email);
                    }
                    catch { }

                    return ReturnUserFriendlyError(Errors.InvalidCredentials);
                }
                    
                var token = await _repo.GenerateUserToken(parameters.Email, parameters.Password);

                return Ok(new LoginResponse(user.ID, user.Email, user.IsAdmin, token));
            }
            catch (FirebaseAuthException ex)
            {
                if (ex.Reason == AuthErrorReason.WrongPassword || ex.Reason == AuthErrorReason.UnknownEmailAddress)
                    return ReturnUserFriendlyError(Errors.InvalidCredentials);

                if (ex.Reason == AuthErrorReason.TooManyAttemptsTryLater)
                    return ReturnUserFriendlyError(Errors.AttemptLimitExceeded);

                return ReturnUserFriendlyError(Errors.Unknown);
            }
            catch
            {
                return ReturnUserFriendlyError(Errors.Unknown);
            }
        }
    }
}
