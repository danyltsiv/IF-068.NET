using ASPNETAPP.DataProvider;
using ASPNETAPP.Extensions;
using ASPNETAPP.Responses;
using ASPNETAPP.ViewModels;
using CommonLib;
using DALLib;
using DALLib.EF;
using DALLib.Repos;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web.Http;
using ASPNETAPP.ActionFilters;

namespace ASPNETAPP.Controllers
{
    [RoutePrefix("api/instances"),
     ExceptionFilter,
     ActionFilter]
    public class InstancesController : ApiController
    {
        const string ENDPOINT = "net.tcp://localhost:9999/WCFService";
        private IMonitorDataProvider dataProvider;

        public InstancesController(IMonitorDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        [Authorize]
        public async Task<IHttpActionResult> Get(int page = 0, String namefilter = "", 
            String serverNamefilter = "", String versionFilter = "", String sqlversions = "")
        {
            InstanceRepository repository = new InstanceRepository(new MsSqlMonitorEntities());
            IListModelResponse<InstanceViewModel> response = new ListModelResponse<InstanceViewModel>();

            try
            {
                PaginatedInstances data = await repository.GetInstancesAsync(page, namefilter, serverNamefilter, versionFilter, sqlversions);

                response.Model = data.List.Select(i => i.ToViewModel());

                response.PagesCount = data.PageCount;
                response.PageNumber = data.Page;
                response.PageSize = data.PageSize;
                response.Versions = data.Versions;

            }
            catch (Exception e)
            {
                response.DidError = true;
                response.ErrorMessage = e.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [Route("deleted")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetDeleted(int page = 0, String namefilter = "", 
            String serverNamefilter = "", String versionFilter = "", String sqlversions = "")
        {
            InstanceRepository repository = new InstanceRepository(new MsSqlMonitorEntities());
            IListModelResponse<InstanceViewModel> response = new ListModelResponse<InstanceViewModel>();

            try
            {
                PaginatedInstances data = await repository.GetDeletedInstancesAsync(page, namefilter, serverNamefilter, versionFilter, sqlversions);

                response.Model = data.List.Select(i => i.ToViewModel());
                response.PagesCount = data.PageCount;
                response.PageNumber = data.Page;
                response.PageSize = data.PageSize;
                response.Versions = data.Versions;
            }
            catch (Exception e)
            {
                response.DidError = true;
                response.ErrorMessage = e.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [Authorize]
        [Route("assigned")]
        public async Task<IHttpActionResult> GetAssigned(int id = 0, int page = 0, String namefilter = "",
            String serverNamefilter = "", String versionFilter = "", String sqlversions = "")
        {
            InstanceRepository repository = new InstanceRepository(new MsSqlMonitorEntities());
            IListModelResponse<InstanceViewModel> response = new ListModelResponse<InstanceViewModel>();

            try
            {
                PaginatedInstances data = await repository.GetAssignedInstancesAsync(id, page, namefilter, serverNamefilter, versionFilter, sqlversions);

                response.Model = data.List.Select(i => i.ToViewModel());

                response.PagesCount = data.PageCount;
                response.PageNumber = data.Page;
                response.PageSize = data.PageSize;
                response.Versions = data.Versions;

            }
            catch (Exception e)
            {
                response.DidError = true;
                response.ErrorMessage = e.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [Authorize]
        [Route("hiden-assigned")]
        public async Task<IHttpActionResult> GetHidenAssigned(int id = 0, int page = 0, String namefilter = "", 
            String serverNamefilter = "", String versionFilter = "", String sqlversions = "")
        {
            InstanceRepository repository = new InstanceRepository(new MsSqlMonitorEntities());
            IListModelResponse<InstanceViewModel> response = new ListModelResponse<InstanceViewModel>();

            try
            {
                PaginatedInstances data = await repository.GetAssignedHidenInstancesAsync(id, page, namefilter, serverNamefilter, versionFilter, sqlversions);

                response.Model = data.List.Select(i => i.ToViewModel());
                response.PagesCount = data.PageCount;
                response.PageNumber = data.Page;
                response.PageSize = data.PageSize;
                response.Versions = data.Versions;
            }
            catch (Exception e)
            {
                response.DidError = true;
                response.ErrorMessage = e.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [Authorize(Roles = "User")]
        [Route("deleted-assigned")]
        public async Task<IHttpActionResult> GetDeletedAssigned(int id = 0, int page = 0, String namefilter = "",
            String serverNamefilter = "", String versionFilter = "", String sqlversions = "")
        {
            InstanceRepository repository = new InstanceRepository(new MsSqlMonitorEntities());
            IListModelResponse<InstanceViewModel> response = new ListModelResponse<InstanceViewModel>();

            try
            {
                PaginatedInstances data = await repository.GetAssignedDeletedInstancesAsync(id, page, namefilter, serverNamefilter, versionFilter, sqlversions);

                response.Model = data.List.Select(i => i.ToViewModel());
                response.PagesCount = data.PageCount;
                response.PageNumber = data.Page;
                response.PageSize = data.PageSize;
                response.Versions = data.Versions;
            }
            catch (Exception e)
            {
                response.DidError = true;
                response.ErrorMessage = e.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [Authorize]
        public async Task<IHttpActionResult> Get(int id)
        {
            ISingleModelResponse<InstanceViewModel> response = new SingleModelResponse<InstanceViewModel>();
            try
            {
                response.Model = await dataProvider.GetInstanceById(id);
                response.DidError = false;
                response.Message = "Data were successfully obtained";
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.Message = ex.Message;
            }
            return response.ToHttpResponse(Request);
        }

        // Create
        [Authorize(Roles = "Admin, User")]
        public async Task<IHttpActionResult> Post([FromBody]string input)
        {
            InstanceViewModel value = JsonConvert.DeserializeObject<InstanceViewModel>(input);
            ISingleModelResponse<InstanceViewModel> result = new SingleModelResponse<InstanceViewModel>();
            try
            {
                result.Model = await dataProvider.AddInstance(value);
                result.Message = $"Instance {value.InstanceName} was successfully added to the system.";
            }
            catch (Exception ex)
            {
                result.DidError = true;
                result.Message = ex.Message;
            }
            return result.ToHttpResponse(Request);
        }

        // Recover instance
        [Authorize(Roles = "Admin, User")]
        public async Task<IHttpActionResult> Post(int id)
        {
            ISingleModelResponse<InstanceViewModel> result = new SingleModelResponse<InstanceViewModel>();
            try
            {
                result.Model = await dataProvider.RecoverInstance(id);
                result.DidError = false;
                result.Message = $"Instance {result.Model.InstanceName} was successfully recovered.";
            }
            catch (Exception ex)
            {
                result.DidError = true;
                result.Message = ex.Message;
            }
            return result.ToHttpResponse(Request);
        }

        // Update
        [Authorize(Roles = "Admin, User")]
        public async Task<IHttpActionResult> Put([FromBody]string value)
        {
            InstanceViewModel instance = JsonConvert.DeserializeObject<InstanceViewModel>(value);
            ISingleModelResponse<InstanceViewModel> response = new SingleModelResponse<InstanceViewModel>();
            try
            {
                response.Model = await dataProvider.UpdateInstance(instance);
                response.DidError = false;
                response.Message = "Instance was successfully updated";
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
            }
            return response.ToHttpResponse(Request);
        }

        // Delete
        [Authorize(Roles = "Admin, User")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            ISingleModelResponse<InstanceViewModel> result = new SingleModelResponse<InstanceViewModel>();
            try
            {
                result.Model = await dataProvider.DeleteInstance(id);
                result.DidError = false;
                result.Message = $"Instance {result.Model.InstanceName} was successfully deleted.";
            }
            catch (Exception ex)
            {
                result.DidError = true;
                result.Message = ex.Message;
            }
            return result.ToHttpResponse(Request);
        }

        [HttpGet]
        [Route("browsable")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IHttpActionResult> GetBrowsableInstances()
        {
            IListModelResponse<BrowsableInstance> response = new ListModelResponse<BrowsableInstance>();

            try
            {
                response.Model = BrowsableInstance.GetInstances();
                response.DidError = false;
                response.Message = "Data were successfully obtained";
            }
            catch (Exception e)
            {
                response.DidError = true;
                response.ErrorMessage = e.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [HttpGet]
        [Route("checkconnection")]
        [Authorize(Roles = "Admin, User")]
        public async Task<String> CheckConnection(string servername = "", string instanceName = "", 
            string login = "", string pswd = "", string windowsoauth = "false")
        {
            Boolean oauthType = false;
            Boolean.TryParse(windowsoauth, out oauthType);

            String result = await ConnectionChecker.Check(servername, instanceName, login, pswd, oauthType);
            return result;
        }

        [HttpGet]
        [Authorize]
        [Route("{instanceId}/refresh")]
        public async Task<ISingleModelResponse<string>> Refresh(int instanceId)
        {
            ISingleModelResponse<string> result = new SingleModelResponse<string>();
            result.ErrorMessage = "OK";

            try
            {
                ChannelFactory<IWCFContract> mChannelFacory = new ChannelFactory<IWCFContract>(new NetTcpBinding(),
                                                                              new EndpointAddress(ENDPOINT));
                IWCFContract proxy = mChannelFacory.CreateChannel();

                proxy.RefreshInstance(instanceId);

            }
            catch (Exception e)
            {
                result.DidError = true;
                result.ErrorMessage = e.Message;
            }


            return result;
        }

        [HttpGet]
        [Authorize]
        [Route("{instanceId}/logins")]
        public async Task<IHttpActionResult> GetLogins(int instanceId)
        {
            IListModelResponse<InstanceLoginViewModel> response = new ListModelResponse<InstanceLoginViewModel>();
            try
            {
                response.Model = await dataProvider.GetLogins(instanceId);
                response.DidError = false;
                response.Message = "Data were successfully obtained";
            }
            catch (EntityNotExistException)
            {
                response.DidError = true;
                response.ErrorMessage = "Cannot get logins because instance with specified Id doesn't exist.";
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [HttpGet]
        [Authorize]
        [Route("{instanceId}/roles")]
        public async Task<IHttpActionResult> GetRoles(int instanceId)
        {
            IListModelResponse<InstanceRoleViewModel> response = new ListModelResponse<InstanceRoleViewModel>();
            try
            {
                response.Model = await dataProvider.GetInstanceRoles(instanceId);
                response.DidError = false;
                response.Message = "Data were successfully obtained";
            }
            catch (EntityNotExistException)
            {
                response.DidError = true;
                response.ErrorMessage = "Cannot get roles because instance with specified Id doesn't exist.";
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [HttpGet]
        [Authorize]
        [Route("{principalId}/permissions")]
        public async Task<IHttpActionResult> GetInstancePrincipalPermissions(int principalId)
        {
            IListModelResponse<PermissionViewModel> response = new ListModelResponse<PermissionViewModel>();
            try
            {
                response.Model = await dataProvider.GetInstancePrincipalPermissions(principalId);
                response.DidError = false;
                response.Message = "Data were successfully obtained";
            }
            catch (EntityNotExistException)
            {
                response.DidError = true;
                response.ErrorMessage = "Cannot get permissions because instance principal with specified Id doesn't exist.";
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [HttpGet]
        [Authorize]
        [Route("{instanceId}/databases")]
        public async Task<IHttpActionResult> GetDatabases(int instanceId)
        {
            IListModelResponse<DatabaseViewModel> response = new ListModelResponse<DatabaseViewModel>();
            try
            {
                response.Model = await dataProvider.GetDatabases(instanceId);
                response.DidError = false;
                response.Message = "Databases were succeessfully obtained.";
            }
            catch (EntityNotExistException)
            {
                response.DidError = true;
                response.ErrorMessage = "Instance with specified Id doesn't exist";
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [HttpGet]
        [Authorize]
        [Route("{databaseId}/users")]
        public async Task<IHttpActionResult> GetDatabaseUsers(int databaseId)
        {
            IListModelResponse<DatabaseUserViewModel> response = new ListModelResponse<DatabaseUserViewModel>();
            try
            {
                response.Model = await dataProvider.GetDatabaseUsers(databaseId);
                response.DidError = false;
                response.Message = "Databases users were succeessfully obtained.";
            }
            catch (EntityNotExistException)
            {
                response.DidError = true;
                response.ErrorMessage = "Database with specified Id doesn't exist";
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [HttpGet]
        [Authorize]
        [Route("{databaseId}/database-roles")]
        public async Task<IHttpActionResult> GetDatabaseRoles(int databaseId)
        {
            IListModelResponse<DatabaseRoleViewModel> response = new ListModelResponse<DatabaseRoleViewModel>();
            try
            {
                response.Model = await dataProvider.GetDatabaseRoles(databaseId);
                response.DidError = false;
                response.Message = "Databases roles were succeessfully obtained.";
            }
            catch (EntityNotExistException)
            {
                response.DidError = true;
                response.ErrorMessage = "Database with specified Id doesn't exist";
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [HttpGet]
        [Authorize]
        [Route("{principalId}/database-permissions")]
        public async Task<IHttpActionResult> GetDatabasePrincipalPermissions(int principalId)
        {
            IListModelResponse<PermissionViewModel> response = new ListModelResponse<PermissionViewModel>();
            try
            {
                response.Model = await dataProvider.GetDatabasePrincipalPermissions(principalId);
                response.DidError = false;
                response.Message = "Principal permissions were succeessfully obtained.";
            }
            catch (EntityNotExistException)
            {
                response.DidError = true;
                response.ErrorMessage = "Principal with specified Id doesn't exist";
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [HttpGet]
        [Authorize]
        [Route("{instanceId}/assigned-users")]
        public async Task<IHttpActionResult> GetAssignedUsers(int instanceId)
        {
            IListModelResponse<UserViewModel> response = new ListModelResponse<UserViewModel>();
            try
            {
                response.Model = await dataProvider.GetAssignedUsers(instanceId);
                response.DidError = false;
                response.Message = "Principal permissions were succeessfully obtained.";
            }
            catch (EntityNotExistException)
            {
                response.DidError = true;
                response.ErrorMessage = "Principal with specified Id doesn't exist";
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [HttpGet]
        [Authorize]
        [Route("{instanceId}/not-assigned-users")]
        public async Task<IHttpActionResult> GetNotAssignedUsers(int instanceId)
        {
            IListModelResponse<UserViewModel> response = new ListModelResponse<UserViewModel>();
            try
            {
                response.Model = await dataProvider.GetNotAssignedUsers(instanceId);
                response.DidError = false;
                response.Message = "Principal permissions were succeessfully obtained.";
            }
            catch (EntityNotExistException)
            {
                response.DidError = true;
                response.ErrorMessage = "Principal with specified Id doesn't exist";
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [HttpGet]
        [Authorize]
        [Route("{instanceId}/{userId}/grant-instance-access")]
        public async Task<IHttpActionResult> GrantInstanceAccess(int instanceId, int userId)
        {
            ISingleModelResponse<bool> response = new SingleModelResponse<bool>();
            try
            {
                await dataProvider.GrantInstanceAccess(instanceId, userId);
                response.Model = true;
                response.DidError = false;
                response.Message = "Access to the instance was successfully granted.";
            }
            catch (EntityNotExistException ex)
            {
                response.DidError = true;
                response.Model = false;
                response.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.Model = false;
                response.ErrorMessage = ex.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [HttpPut]
        [Authorize]
        [Route("{instanceId}/{userId}/show-instance")]
        public async Task<IHttpActionResult> ShowInstance(int instanceId, int userId)
        {
            ISingleModelResponse<bool> response = new SingleModelResponse<bool>();
            try
            {
                await dataProvider.ShowInstance(instanceId, userId);
                response.Model = true;
                response.DidError = false;
                response.Message = string.Format("Instance: {0} for user: {1} successfully showed.", instanceId, userId);
            }
            catch (EntityNotExistException ex)
            {
                response.DidError = true;
                response.Model = false;
                response.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.Model = false;
                response.ErrorMessage = ex.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [HttpPut]
        [Authorize]
        [Route("{instanceId}/{userId}/hide-instance")]
        public async Task<IHttpActionResult> HideInstance(int instanceId, int userId)
        {
            ISingleModelResponse<bool> response = new SingleModelResponse<bool>();
            try
            {
                await dataProvider.HideInstance(instanceId, userId);
                response.Model = true;
                response.DidError = false;
                response.Message = string.Format("Instance: {0} for user: {1} successfully hided.", instanceId, userId);
            }
            catch (EntityNotExistException ex)
            {
                response.DidError = true;
                response.Model = false;
                response.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.Model = false;
                response.ErrorMessage = ex.Message;
            }

            return response.ToHttpResponse(Request);
        }

        [HttpGet]
        [Authorize]
        [Route("{instanceId}/{userId}/revoke-instance-access")]
        public async Task<IHttpActionResult> RevokeInstanceAccess(int instanceId, int userId)
        {
            ISingleModelResponse<bool> response = new SingleModelResponse<bool>();
            try
            {
                await dataProvider.RevokeInstanceAccess(instanceId, userId);
                response.Model = true;
                response.DidError = false;
                response.Message = "Access to the instance was successfully revoked.";
            }
            catch (EntityNotExistException ex)
            {
                response.DidError = true;
                response.Model = false;
                response.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.Model = false;
                response.ErrorMessage = ex.Message;
            }

            return response.ToHttpResponse(Request);
        }
    }
}
