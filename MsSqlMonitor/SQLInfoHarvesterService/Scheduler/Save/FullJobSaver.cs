
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLInfoCollectionService.Scheduler;
using CommonLib;
using SQLInfoCollectionService.InstanceInfoUpdating;
using SQLInfoCollectionService.InstanceInfoUpdating.Entities;
using DALLib.Models;

namespace SQLInfoCollectorService.Scheduler.Save
{
    public class FullJobSaver : BaseJobSaver
    {
        public FullJobSaver(ISLogger logger, ILocalStorage localDb, SQLTaskScheduler scheduler) : base(logger, localDb, scheduler)
        {
        }

        public async override void Save(IEnumerable<CollectionResult> results)
        {
            var arrStatus = results.Where(i => i != null && i.JobType == JobType.UpdateInfoType.Full).Select<CollectionResult, InstanceInfo>(i => i.InstanceInfo).ToArray();

            await localDb.SaveInstancesAsync(arrStatus, logger);

            foreach(CollectionResult result in results) scheduler.InstanceUpdateFinished(result.InstanceID, result.JobType);
        }

        public async override void Save(CollectionResult result)
        {
            if (result.InstanceInfo == null) return;

            logger.Debug("save full job ID="+result.InstanceID);

            await localDb.SaveInstancesAsync(new InstanceInfo[] { result.InstanceInfo }, logger);

            scheduler.InstanceUpdateFinished(result.InstanceID, result.JobType);
        }
    }
}
