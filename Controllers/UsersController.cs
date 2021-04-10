using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCrudAPI.Models;
using SimpleCrudAPI.Models.Requests;
using SimpleCrudAPI.Models.Responses;
using SimpleCrudAPI.Repositories;
using SimpleCrudAPI.Security;
using SimpleCrudAPI.Security.Action_Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User = SimpleCrudAPI.Models.User;

namespace SimpleCrudAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerCommon
    {
        private readonly IUserRepository _repo;
        private readonly IFirebaseRepository _fireRepo;
        private readonly ISecurityHelper _securityHelper;

        public UsersController(IUserRepository repo, IFirebaseRepository fireRepo, ISecurityHelper securityHelper)
        {
            _repo = repo;
            _fireRepo = fireRepo;
            _securityHelper = securityHelper;
        }

        [HttpGet]
        [Route("{id}")]
        [ServiceFilter(typeof(UserOwnerValidation))]
        public IActionResult GetByID(int id)
        {
            try
            {
                var user = _repo.Get(id);

                if (user != null)
                {
                    return Ok(BuildResponse(user));
                }

                return NotFound();
            }
            catch
            {
                return ReturnUserFriendlyError(Errors.Unknown);
            }
        }

        [HttpGet]
        [ServiceFilter(typeof(AdminValidation))]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(BuildResponse(_repo.GetAll().OrderBy(u => u.Email)));
            }
            catch
            {
                return ReturnUserFriendlyError(Errors.Unknown);
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(AdminValidation))]
        public async Task<IActionResult> Insert(UserCreationParameters parameters)
        {
            FirebaseAuthLink firebaseUser = null;
            User user = null;

            try
            {
                user = _repo.GetAll().FirstOrDefault(x => x.Email.ToLower() == parameters.Email.ToLower());

                if (user == null)
                {
                    firebaseUser = await _fireRepo.CreateUser(parameters.Email, parameters.Password);

                    User u = new User()
                    {
                        Email = parameters.Email,
                        IsAdmin = parameters.IsAdmin
                    };

                    _repo.Insert(u);
                    return CreatedAtAction("GetByID", new { id = u.ID }, BuildResponse(u));
                }
                else 
                {
                    return ReturnUserFriendlyError(Errors.ExistingEmail);
                }
            }
            catch
            {
                if (firebaseUser != null)
                    await _fireRepo.DeleteUser(firebaseUser.FirebaseToken);

                if (user != null)
                    _repo.Delete(user.ID);

                return ReturnUserFriendlyError(Errors.Unknown);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [ServiceFilter(typeof(AdminValidation))]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var currentUser = _securityHelper.GetCurrentUser(HttpContext);

                if (currentUser.ID == id)
                {
                    return ReturnUserFriendlyError(Errors.CurrentUserDeletionForbidden);
                }

                var user = _repo.Get(id);

                if (user != null)
                {
                    if (IsSuperAdmin(user))
                    {
                        return ReturnUserFriendlyError(Errors.SuperUserModificationForbidden);
                    }
                    else
                    {
                        _repo.Delete(user.ID);
                        await _fireRepo.DeleteUserByEmail(user.Email);
                        return NoContent();
                    }
                }

                return NotFound();
            }
            catch
            {
                return ReturnUserFriendlyError(Errors.Unknown);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ServiceFilter(typeof(UserOwnerValidation))]
        public IActionResult Update(int id, [FromBody] UserUpdateParameters u)
        {
            try
            {
                var user = _repo.Get(id);

                if (user != null)
                {
                    if (IsSuperAdmin(user))
                    {
                        return ReturnUserFriendlyError(Errors.SuperUserModificationForbidden);
                    }
                    else
                    {
                        // Cambio en la propiedad "EsAdmin" solo puede ser hecha por un administrador
                        if (user.IsAdmin != u.IsAdmin)
                        {
                            var currentUser = _securityHelper.GetCurrentUser(HttpContext);

                            if (currentUser.IsAdmin)
                            {
                                user.IsAdmin = u.IsAdmin;
                                user = _repo.Update(id, user);
                            }
                        }
                    }
                    
                    return Ok(BuildResponse(user));
                }

                return NotFound();
            }
            catch
            {
                return ReturnUserFriendlyError(Errors.Unknown);
            }
        }

        [HttpPost]
        [Route("reset")]
        [AllowAnonymous]
        public IActionResult SendResetPassword(string email)
        {
            try
            {
                var user = _repo.Get(email);

                if (user != null)
                {
                    _fireRepo.SendResetPassword(user.Email);
                    return Ok();
                }

                return NotFound();
            }
            catch
            {
                return ReturnUserFriendlyError(Errors.Unknown);
            }
        }

        #region Helper Methods

        private bool IsSuperAdmin(User user)
        {
            return user.ID == 1;
        }

        private UserResponse BuildResponse(User user)
        {
            return new UserResponse
            {
                UserID = user.ID,
                Email = user.Email,
                IsAdmin = user.IsAdmin
            };
        }

        private List<UserResponse> BuildResponse(IEnumerable<User> uList)
        {
            var list = new List<UserResponse>();

            foreach (User u in uList)
            {
                list.Add(new UserResponse
                {
                    UserID = u.ID,
                    Email = u.Email,
                    IsAdmin = u.IsAdmin
                });
            }

            return list;
        }

        #endregion
    }
}
