using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETAPP.ViewModels;

namespace ASPNETAPP.DataProvider
{
    public interface IMonitorDataProvider
    {
        Task<InstanceViewModel> AddInstance(InstanceViewModel newInstance);

        Task<InstanceViewModel> UpdateInstance(InstanceViewModel instance);

        Task<InstanceViewModel> DeleteInstance(int instanceId);

        Task<InstanceViewModel> RecoverInstance(int instanceId);

        Task HideInstance(int instanceId, int userId);

        Task ShowInstance(int instanceId, int userId);

        Task<InstanceViewModel> GetInstanceById(int instanceId);

        Task<IEnumerable<InstanceLoginViewModel>> GetLogins(int instanceId);
        
        Task<IEnumerable<InstanceRoleViewModel>> GetInstanceRoles(int instanceId);

        Task<IEnumerable<PermissionViewModel>> GetInstancePrincipalPermissions(int principalId);

        Task<IEnumerable<DatabaseViewModel>> GetDatabases(int instanceId);

        Task<IEnumerable<DatabaseUserViewModel>> GetDatabaseUsers(int databaseId);

        Task<IEnumerable<DatabaseRoleViewModel>> GetDatabaseRoles(int databaseId);

        Task<IEnumerable<PermissionViewModel>> GetDatabasePrincipalPermissions(int principalId);

        Task<IEnumerable<UserViewModel>> GetAssignedUsers(int instanceId);

        Task<IEnumerable<UserViewModel>> GetNotAssignedUsers(int instanceId);

        Task GrantInstanceAccess(int instanceId, int userId);

        Task RevokeInstanceAccess(int instanceId, int userId);
    }
}
