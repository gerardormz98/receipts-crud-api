using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SimpleCrudAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Security.Action_Filters
{
    /// <summary>
    /// Validates that the user from the request token is the owner of a receipt to modify (admins can modify too).
    /// </summary>
    public class ReceiptOwnerValidation : IActionFilter
    {
        private readonly ISecurityHelper _helper;
        private readonly IReceiptRepository _repo;

        public ReceiptOwnerValidation(ISecurityHelper helper, IReceiptRepository repo)
        {
            _helper = helper;
            _repo = repo;
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

                int ownerId = _repo.Get(idToModify).User.ID;

                // Admins can modify too!
                if (user.ID != ownerId && !user.IsAdmin)
                    context.Result = new UnauthorizedObjectResult(null);
            }
            catch
            {
                context.Result = new UnauthorizedObjectResult(null);
            }
        }
    }
}
