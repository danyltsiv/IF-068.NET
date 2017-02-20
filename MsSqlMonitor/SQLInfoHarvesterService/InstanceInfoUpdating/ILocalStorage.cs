using SQLInfoCollectionService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;


namespace SQLInfoCollectionService.InstanceInfoUpdating
{
    using Contracts;
    using Entities;
    using Scheduler;

    public interface ILocalStorage
    {
        void SaveInstances(InstanceInfo[] newInfo,ISLogger logger);

        void SaveStatusOnly(InstanceInfo[] newInfo, ISLogger logger);

        Task SaveStatusOnlyAsync(InstanceInfo[] newInfo, ISLogger logger);

        Task SaveInstancesAsync(InstanceInfo[] newInfo, ISLogger logger);
 
        void RemoveInstanceForeverAsync(CollectionResult result, ISLogger logger);
    }
}
