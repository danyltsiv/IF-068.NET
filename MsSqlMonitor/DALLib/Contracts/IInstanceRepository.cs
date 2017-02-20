using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Contracts
{
    public interface IInstanceRepository : IRepository<Instance>
    {
        bool IsInstanceExist(string serverName, string instanceName);
        Task<bool> IsInstanceExistAsync(string serverName, string instanceName);

        InstanceVersion GetVersionByInstanceId(int instanceId);
        Task<InstanceVersion> GetVersionByInstanceIdAsync(int instanceId);

        IEnumerable<InstLogin> GetInstLoginsByInstanceId(int instanceId);
        Task<IEnumerable<InstLogin>> GetInstLoginsByInstanceIdAsync(int instanceId);

        IEnumerable<InstRole> GetInstRolesByInstanceId(int instanceId);
        Task<IEnumerable<InstRole>> GetInstRolesByInstanceIdAsync(int instanceId);
    }
}
