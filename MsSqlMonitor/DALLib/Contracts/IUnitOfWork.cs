using DALLib.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;

namespace DALLib.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IAssignRepository Assigns { get; }
        IDatabaseRepository Databases { get; }
        IUserRepository Users { get; }
        IInstPermissionRepository InstPermissions { get; }
        IDbPermissionRepository DbPermissions { get; }
        IInstanceRepository Instances { get; }
        IJobTypeRepository JobTypes { get; }
        IRoleRepository Roles { get; }
        int Save();
        Task<int> SaveAsync();
    }
}
