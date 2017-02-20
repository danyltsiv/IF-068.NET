using ASPNETAPP.ActionFilters;
using ASPNETAPP.Extensions;
using ASPNETAPP.Models;
using ASPNETAPP.ViewModels;
using CommonLib;
using DALLib.Contracts;
using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DALLib;

namespace ASPNETAPP.Controllers
{
    [RoutePrefix("api/users"),
     ExceptionFilter,
     ActionFilter,
     Authorize(Roles = "Admin")]
    public class UsersController : ApiController
    {
        private IUnitOfWork unitOfWork;
        private static ISLogger logger;
        private readonly IMapper mapper;

        public UsersController(IUnitOfWork uof, ISLogger log, IMapper mapp)
        {
            unitOfWork = uof;
            logger = log;
            mapper = mapp;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await unitOfWork.Users.GetAllAsync();
            var model = mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(result);

            return Ok(model);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var result = await unitOfWork.Users.GetAsync(id);
            var model = mapper.Map<User, UserViewModel>(result);

            return Ok(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("username")]
        public async Task<IHttpActionResult> GetByUserUsername([FromUri]string username)
        {
            var result = await unitOfWork.Users.GetUserByUsernameAsync(username);
            var model = mapper.Map<User, UserViewModel>(result);

            return Ok(model);
        }
    
        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> Post([FromBody]UserModelRequest userRequest)
        {
            if (!await unitOfWork.Users.IsLoginExistAsync(userRequest.User.UserName))
            {
                User user = await unitOfWork.Users.CreateAsync(userRequest.User, userRequest.Password, userRequest.Role);
                if (user != null)
                {
                    await unitOfWork.SaveAsync();
                    var result = await unitOfWork.Users.GetAllAsync();

                    return Ok(mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(result));
                } else
                {
                    return BadRequest("Bad user model!");
                }
            }
            else
            {
                return BadRequest("Login is already exists!");
            }
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put([FromBody]UserModelRequest userRequest)
        {
            //if (await unitOfWork.Users.IsLoginExistAsync(userRequest.User))
            //{
            //    return BadRequest("Login is already exists!");
            //}
            if (!String.IsNullOrEmpty(userRequest.Role) && !await unitOfWork.Users.AssignSingleRoleAsync(userRequest.User.Id, userRequest.Role))
            {
                return BadRequest("Role is not valid!");
            } else if (!String.IsNullOrEmpty(userRequest.Password) && !await unitOfWork.Users.ChangePasswordAsync(userRequest.User.Id, userRequest.Password))
            {
                return BadRequest("Password is not valid!");
            }

            //var result = await unitOfWork.Users
            //    .UpdateAsync(userRequest.User);
            //await unitOfWork.SaveAsync();
            return Ok(true);
        }

        [HttpGet]
        [Route("assigned")]
        public async Task<PaginatedViewInstances> GetAssignedInstances(int id , int page = 0, String namefilter = "",
            String serverNamefilter = "", String versionFilter = "",String sqlversions = "")
        {
            PaginatedInstances result = await unitOfWork.Users.GetAssignedInstancesAsync(id,page,namefilter,serverNamefilter,versionFilter, sqlversions);       
            return new PaginatedViewInstances(result);    
        }

        [HttpGet]
        [Route("not-assigned")]
        public async Task<PaginatedViewInstances> GetNotAssignedInstances(int id, int page = 0, String namefilter = "",
            String serverNamefilter = "", String versionFilter = "", String sqlversions = "")
        {
            PaginatedInstances result = await unitOfWork.Users.GetNotAssignedInstancesAsync(id, page, namefilter, serverNamefilter, versionFilter, sqlversions);
            return new PaginatedViewInstances(result);
        }

        [HttpPost]
        [Route("assign")]
        public async Task<IHttpActionResult> AssignInstance([FromBody]Assign assign)
        {
            Assign newAssign = unitOfWork.Assigns.Create(assign);

            if (newAssign != null)
            {
                await unitOfWork.SaveAsync();
                var result = await unitOfWork.Users.GetAssignedInstancesAsync(assign.UserId);
                return Ok(result.Select(inst => inst.ToViewModel()).ToList());
            } else
            {
                return BadRequest("Asssign is not valid!");
            }
        }

        [HttpDelete]
        [Route("deassign")]
        public async Task<IHttpActionResult> DeassignInstance([FromUri]int userId, [FromUri]int instanceId)
        {
            Assign newAssign = unitOfWork.Assigns.Delete(userId, instanceId);

            if (newAssign != null)
            {
                await unitOfWork.SaveAsync();
                var result = await unitOfWork.Users.GetAssignedInstancesAsync(userId);
                return Ok(result.Select(inst => inst.ToViewModel()).ToList());
            }
            else
            {
                return BadRequest("Asssign is not valid!");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            User deletedUser = await unitOfWork.Users.DeleteAsync(id);

            if (deletedUser != null)
            {
                await unitOfWork.SaveAsync();
                var result = await unitOfWork.Users.GetAllAsync();
                return Ok(result.ToViewModelList());
            } else
            {
                return BadRequest("Wrong user ID!");
            }
        }
    }
}