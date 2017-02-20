using System.Collections.Generic;
using System.Linq;
using DALLib.EF;
using System.Data.Entity;
using DALLib.Contracts;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DALLib.Models;

namespace DALLib.Repos
{
    public class AssignRepository : BaseRepository<Assign>, IAssignRepository
    {
        public AssignRepository(MsSqlMonitorEntities context) : base(context) { }
        public IEnumerable<Assign> GetUserAssigns(int userId)
        {
            return table.Where(g => g.UserId == userId);
        }
        public IEnumerable<Assign> GetUserNotAssigns(int userId)
        {
            return table.Where(g => g.UserId != userId);
        }
        public IEnumerable<Assign> GetInstanceAssings(int instanceId)
        {
            return table.Where(g => g.InstanceId == instanceId);
        }
        public async Task<IEnumerable<Assign>> GetUserAssignsAsync(int userId)
        {
            return await table.Where(g => g.UserId == userId).ToListAsync();
        }
        public async Task<IEnumerable<Assign>> GetUserNotAssignsAsync(int userId)
        {
            return await table.Where(g => g.UserId != userId).ToListAsync();
        }
        public async Task<IEnumerable<Assign>> GetInstanceAssingsAsync(int instanceId)
        {
            return await table.Where(g => g.InstanceId == instanceId).ToListAsync();
        }
        public Assign Delete(int userId,int instanceId)
        {
            Assign deleteAssign = table.FirstOrDefault(g => g.InstanceId == instanceId && g.UserId == userId);
            return table.Remove(deleteAssign);
        }
    }
}
