using ASPNETAPP.ActionFilters;
using DALLib.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ASPNETAPP.Controllers
{
    [RoutePrefix("api/roles"),
     ExceptionFilter,
     ActionFilter,
     Authorize(Roles = "Admin")]

    public class RolesController : ApiController
    {
        public IUnitOfWork unitOfWork;

        public RolesController(IUnitOfWork uof)
        {
            unitOfWork = uof;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await unitOfWork.Roles.GetAllAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Get(int id)
        {
            var result = await unitOfWork.Roles.FindByIdAsync(id);

            return Ok(result);
        }
    }
}
