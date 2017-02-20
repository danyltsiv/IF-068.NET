using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Contracts
{
    public interface IDatabaseRepository : IRepository<Database>
    {
        IEnumerable<Database> GetDatabasesByInstanceId(int instanceId);
        Task<IEnumerable<Database>> GetDatabasesByInstanceIdAsync(int instanceId);

        IEnumerable<DbUser> GetDbUsersByDatabaseId(int databaseId);
        Task<IEnumerable<DbUser>> GetDbUsersByDatabaseIdAsync(int databaseId);

        IEnumerable<DbRole> GetDbRolesByDatabaseId(int databaseId);
        Task<IEnumerable<DbRole>> GetDbRolesByDatabaseIdAsync(int databaseId);
    }
}
