using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCrudAPI.Models;
using SimpleCrudAPI.Models.Responses;
using SimpleCrudAPI.Repositories;
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
    public class SuppliersController : ControllerCommon
    {
        private readonly ISupplierRepository _repo;

        public SuppliersController(ISupplierRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetByID(int id)
        {
            try
            {
                var supplier = _repo.Get(id);

                if (supplier != null)
                {
                    return Ok(BuildResponse(supplier));
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
            try
            {
                return Ok(BuildResponse(_repo.GetAll().OrderBy(s => s.Name)));
            }
            catch
            {
                return ReturnUserFriendlyError(Errors.Unknown);
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(AdminValidation))]
        public IActionResult Insert([FromBody] Supplier s)
        {
            try
            {
                var supplier = _repo.GetAll().FirstOrDefault(x => x.Name.ToLower() == s.Name.ToLower());

                if (supplier == null)
                {
                    _repo.Insert(s);
                    return CreatedAtAction("GetByID", new { id = s.ID }, BuildResponse(s));
                }
                    
                return ReturnUserFriendlyError(Errors.ExistingName);
            }
            catch
            {
                return ReturnUserFriendlyError(Errors.Unknown);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [ServiceFilter(typeof(AdminValidation))]
        public IActionResult Delete(int id)
        {
            try
            {
                var supplier = _repo.Get(id);

                if (supplier != null)
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
        [ServiceFilter(typeof(AdminValidation))]
        public IActionResult Update(int id, [FromBody] Supplier s)
        {
            try
            {
                var supplierWithSameName = _repo.GetAll().FirstOrDefault(x => x.Name.ToLower() == s.Name.ToLower());

                if (supplierWithSameName == null || supplierWithSameName.ID == id)
                {
                    var supplier = _repo.Get(id);

                    if (supplier != null)
                    {
                        var newSupplier = _repo.Update(id, s);
                        return Ok(BuildResponse(newSupplier));
                    }

                    return NotFound();
                }
                else
                {
                    return ReturnUserFriendlyError(Errors.ExistingName);
                }
            }
            catch
            {
                return ReturnUserFriendlyError(Errors.Unknown);
            }
        }

        #region Helper Methods

        private SupplierResponse BuildResponse(Supplier s)
        {
            return new SupplierResponse
            {
                SupplierID = s.ID,
                Name = s.Name,
                Phone = s.Phone
            };
        }

        private List<SupplierResponse> BuildResponse(IEnumerable<Supplier> sList)
        {
            var list = new List<SupplierResponse>();

            foreach (Supplier s in sList)
            {
                list.Add(BuildResponse(s));
            }

            return list;
        }

        #endregion
    }
}
