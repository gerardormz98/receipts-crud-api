using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Security.Action_Filters
{
    /// <summary>
    /// Validates that the user from the request token is the owner of the user to modify (admins can modify too).
    /// </summary>
    public class UserOwnerValidation : IActionFilter
    {
        private readonly ISecurityHelper _helper;
        public UserOwnerValidation(ISecurityHelper helper)
        {
            _helper = helper;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = _helper.GetCurrentUser(context.HttpContext);

            try
            {
                int idToModify = (int)context.ActionArguments.SingleOrDefault(p => p.Key == "id").Value;

                // Admins can modify too!
                if (user.ID != idToModify && !user.IsAdmin)
                    context.Result = new UnauthorizedObjectResult(null);
            }
            catch
            {
                context.Result = new UnauthorizedObjectResult(null);
            }
        }
    }
}
