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
    public class DatabaseRepository : BaseRepository<Models.Database>, IDatabaseRepository
    {
        public DatabaseRepository(MsSqlMonitorEntities context) : base(context)
        {
            table = context.Databases;
        }

        public IEnumerable<Models.Database> GetDatabasesByInstanceId(int instanceId)
        {
            return table.Where(g => g.InstanceId == instanceId);
        }

        public IEnumerable<DbUser> GetDbUsersByDatabaseId(int databaseId)
        {
            var result = table.FirstOrDefault(g => g.Id == databaseId);

            if (result != null)
            {
                return result.Users;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<DbRole> GetDbRolesByDatabaseId(int databaseId)
        {
            var result = table.FirstOrDefault(g => g.Id == databaseId);

            if (result != null)
            {
                return result.Roles;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Models.Database>> GetDatabasesByInstanceIdAsync(int instanceId)
        {
            return await table.Where(g => g.InstanceId == instanceId).ToListAsync();
        }

        public async Task<IEnumerable<DbUser>> GetDbUsersByDatabaseIdAsync(int databaseId)
        {
            var result = await table.FirstOrDefaultAsync(g => g.Id == databaseId);

            if (result != null)
            {
                return result.Users;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<DbRole>> GetDbRolesByDatabaseIdAsync(int databaseId)
        {
            var result = await table.FirstOrDefaultAsync(g => g.Id == databaseId);

            if (result != null)
            {
                return result.Roles;
            }
            else
            {
                return null;
            }
        }
    }
}
