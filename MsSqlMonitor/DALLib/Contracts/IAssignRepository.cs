using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Contracts
{
    public interface IAssignRepository : IRepository<Assign>
    {
        IEnumerable<Assign> GetUserAssigns(int userId);
        Task<IEnumerable<Assign>> GetUserAssignsAsync(int userId);
        IEnumerable<Assign> GetInstanceAssings(int instanceId);
        Task<IEnumerable<Assign>> GetInstanceAssingsAsync(int instanceId);
        Assign Delete(int userId, int instanceId);
    }
}
