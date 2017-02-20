using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Contracts
{
    public interface IInstPermissionRepository : IRepository<InstPermission>
    {
        IEnumerable<InstPermission> GetPrincipalPermissions(int principalId);
        Task<IEnumerable<InstPermission>> GetPrincipalPermissionsAsync(int principalId);
    }
}
