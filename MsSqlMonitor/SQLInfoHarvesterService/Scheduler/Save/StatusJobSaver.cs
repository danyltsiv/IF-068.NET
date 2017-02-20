using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using SQLInfoCollectionService.InstanceInfoUpdating;
using SQLInfoCollectionService.Scheduler;
using SQLInfoCollectionService.InstanceInfoUpdating.Entities;
using DALLib.Models;

namespace SQLInfoCollectorService.Scheduler.Save
{
    public class StatusJobSaver : BaseJobSaver
    {
        public StatusJobSaver(ISLogger logger, ILocalStorage localDb, SQLTaskScheduler scheduler) : base(logger, localDb, scheduler)
        {
        }

        public async override void Save(IEnumerable<CollectionResult> results)
        {
            var arrStatus = results.Where(i => i != null && i.JobType == JobType.UpdateInfoType.CheckStatus).Select<CollectionResult, InstanceInfo>(i => i.InstanceInfo).ToArray();

            await localDb.SaveStatusOnlyAsync(arrStatus, logger);

            foreach (CollectionResult result in results) scheduler.InstanceUpdateFinished(result.InstanceID, result.JobType);
        }

        public async override void Save(CollectionResult result)
        {
            if (result.InstanceInfo == null) return;

            logger.Debug("save status job ID=" + result.InstanceID);


            await localDb.SaveStatusOnlyAsync(new InstanceInfo[] { result.InstanceInfo}, logger);

            scheduler.InstanceUpdateFinished(result.InstanceID, result.JobType);
        }
    }
}
