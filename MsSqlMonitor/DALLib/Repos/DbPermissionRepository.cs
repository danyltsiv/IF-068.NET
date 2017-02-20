using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using DALLib.EF;
using DALLib.Contracts;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DALLib.Repos
{
    public class DbPermissionRepository : BaseRepository<DbPermission>, IDbPermissionRepository
    {
        public DbPermissionRepository(MsSqlMonitorEntities context) : base(context) { }

        public IEnumerable<DbPermission> GetPrincipalPermissions(int principalId)
        {
            return table.Where(g => g.Principals.Any(p => p.Id == principalId));
        }

        public async Task<IEnumerable<DbPermission>> GetPrincipalPermissionsAsync(int principalId)
        {
            return await table.Where(g => g.Principals.Any(p => p.Id == principalId)).ToListAsync();
        }
    }
}
