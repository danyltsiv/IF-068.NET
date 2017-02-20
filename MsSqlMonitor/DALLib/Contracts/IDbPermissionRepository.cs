using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Contracts
{
    public interface IDbPermissionRepository : IRepository<DbPermission>
    {
        IEnumerable<DbPermission> GetPrincipalPermissions(int principalId);
        Task<IEnumerable<DbPermission>> GetPrincipalPermissionsAsync(int principalId);
    }
}
