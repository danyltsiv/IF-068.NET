using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectionService.DataUpdating.Contracts
{
    using Entities;

    public interface IInfoKeeper
    {
        Task<InstanceInfo> UpdateAsync(Instance sourceModel);

        Task SaveAsync(IEnumerable<InstanceInfo> data);
    }
}
