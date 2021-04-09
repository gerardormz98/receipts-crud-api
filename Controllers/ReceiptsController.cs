using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCrudAPI.Models;
using SimpleCrudAPI.Models.Responses;
using SimpleCrudAPI.Repositories;
using SimpleCrudAPI.Security;
using SimpleCrudAPI.Security.Action_Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerCommon
    {
        private readonly IReceiptRepository _repo;
        private readonly ISupplierRepository _proveedorRepository;
        private readonly ISecurityHelper _securityHelper;

        public ReceiptsController(IReceiptRepository repo, ISupplierRepository proveedorRepository, ISecurityHelper securityHelper)
        {
            _repo = repo;
            _proveedorRepository = proveedorRepository;
            _securityHelper = securityHelper;
        }

        [HttpGet]
        [Route("{id}")]
        [ServiceFilter(typeof(ReceiptOwnerValidation))]
        public IActionResult GetByID(int id)
        {
            try
            {
                var receipt = _repo.Get(id);

                if (receipt != null)
                {
                    return Ok(BuildResponse(receipt));
                }

                return NotFound();
            }
            catch
            {
                return ReturnUserFriendlyError(Errors.Unknown);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            //try
            //{
                var user = _securityHelper.GetCurrentUser(HttpContext);
                var receipts = _repo.GetAll().Where(r => r.UserID == user.ID).OrderByDescending(r => r.ID);
                return Ok(BuildResponse(receipts));
            //}
            //catch
            //{
            //    return ReturnUserFriendlyError(Errors.Unknown);
            //}
        }

        [HttpPost]
        public IActionResult Insert([FromBody] Receipt r)
        {
            try
            {
                var user = _securityHelper.GetCurrentUser(HttpContext);

                if (!ValidValues(r))
                    return ReturnUserFriendlyError(Errors.InvalidValues);

                r.UserID = user.ID;
                r.Date = DateTime.Now;
                _repo.Insert(r);

                return CreatedAtAction("GetByID", new { id = r.ID }, BuildResponse(r));
            }
            catch
            {
                return ReturnUserFriendlyError(Errors.Unknown);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [ServiceFilter(typeof(ReceiptOwnerValidation))]
        public IActionResult Delete(int id)
        {
            try
            {
                var receipt = _repo.Get(id);

                if (receipt != null)
                {
                    _repo.Delete(id);
                    return NoContent();
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
        [ServiceFilter(typeof(ReceiptOwnerValidation))]
        public IActionResult Update(int id, [FromBody] Receipt r)
        {
            try
            {
                var receipt = _repo.Get(id);

                if (receipt != null)
                {
                    if (!ValidValues(r))
                        return ReturnUserFriendlyError(Errors.InvalidValues);

                    var newReceipt = _repo.Update(id, r);
                    return Ok(BuildResponse(newReceipt));
                }

                return NotFound();
            }
            catch
            {
                return ReturnUserFriendlyError(Errors.Unknown);
            }
        }

        #region Helper Methods

        private bool ValidValues(Receipt r)
        {
            var supplier = _proveedorRepository.Get(r.SupplierID);

            return supplier != null;
        }

        private ReceiptResponse BuildResponse(Receipt r)
        {
            return new ReceiptResponse
            {
                ReceiptID = r.ID,
                Amount = r.Amount,
                Comments = r.Comments,
                Date = r.Date,
                Supplier = new SupplierResponse()
                {
                    SupplierID = r.SupplierID,
                    Name = r.Supplier.Name,
                    Phone = r.Supplier.Phone
                }
            };
        }

        private List<ReceiptResponse> BuildResponse(IEnumerable<Receipt> rList)
        {
            var list = new List<ReceiptResponse>();

            foreach (Receipt r in rList)
            {
                list.Add(BuildResponse(r));
            }

            return list;
        }

        #endregion
    }
}
