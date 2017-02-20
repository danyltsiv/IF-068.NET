using DALLib.Models;
using System.Collections.Generic;
using System.Linq;
using DALLib.EF;
using DALLib.Contracts;
using System;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DALLib.Repos
{
    public class InstPermissionRepository : BaseRepository<InstPermission>, IInstPermissionRepository
    {
        public InstPermissionRepository(MsSqlMonitorEntities context) : base(context) { }

        public IEnumerable<InstPermission> GetPrincipalPermissions(int principalId)
        {
            return table.Where(g => g.Principals.Select(p => p.Id).Contains(principalId));
        }

        public async Task<IEnumerable<InstPermission>> GetPrincipalPermissionsAsync(int principalId)
        {
            return await table.Where(g => g.Principals.Any(p => p.Id == principalId)).ToListAsync();
        }
    }
}
