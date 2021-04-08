using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Controllers
{
    public enum Errors
    {
        Unknown,
        ExistingName,
        ExistingEmail,
        InvalidCredentials,
        InvalidValues,
        CurrentUserDeletionForbidden,
        AttemptLimitExceeded,
        SuperUserModificationForbidden
    }

    public class ControllerCommon : ControllerBase
    {
        protected IActionResult ReturnUserFriendlyError(Errors? error)
        {
            switch (error)
            {
                case Errors.ExistingName:
                    return BadRequest(BuildErrorObject((Errors)error, "The name already exists!"));
                case Errors.ExistingEmail:
                    return BadRequest(BuildErrorObject((Errors)error, "The email is already registered!"));
                case Errors.InvalidCredentials:
                    return BadRequest(BuildErrorObject((Errors)error, "The email or password are invalid."));
                case Errors.InvalidValues:
                    return BadRequest(BuildErrorObject((Errors)error, "At least one of the entered values is invalid"));
                case Errors.CurrentUserDeletionForbidden:
                    return BadRequest(BuildErrorObject((Errors)error, "It is not possible to delete your own admin user."));
                case Errors.AttemptLimitExceeded:
                    return BadRequest(BuildErrorObject((Errors)error, "You have exceeded the attempt limit. Try again later."));
                case Errors.SuperUserModificationForbidden:
                    return BadRequest(BuildErrorObject((Errors)error, "It is not possible to delete or modify superuser."));
                case Errors.Unknown:
                default:
                    return StatusCode(500, BuildErrorObject(Errors.Unknown, "There was an error while processing your request."));
            }
        }

        private object BuildErrorObject(Errors err, string message)
        {
            return new
            {
                code = err,
                message
            };
        }
    }
}
