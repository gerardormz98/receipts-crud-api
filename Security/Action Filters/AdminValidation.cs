using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Security.Action_Filters
{
    /// <summary>
    /// Validates that the user in the request token has the admin role
    /// </summary>
    public class AdminValidation : IActionFilter
    {
        private readonly ISecurityHelper _helper;

        public AdminValidation(ISecurityHelper helper)
        {
            _helper = helper;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = _helper.GetCurrentUser(context.HttpContext);

            if (!user.IsAdmin)
                context.Result = new UnauthorizedObjectResult(null);
        }
    }
}
