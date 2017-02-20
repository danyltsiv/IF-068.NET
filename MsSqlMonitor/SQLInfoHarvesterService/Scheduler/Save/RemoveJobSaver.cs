using CommonLib;
using SQLInfoCollectionService.InstanceInfoUpdating;
using SQLInfoCollectionService.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectorService.Scheduler.Save
{
    public class RemoveJobSaver : BaseJobSaver
    {
        public RemoveJobSaver(ISLogger logger, ILocalStorage localDb, SQLTaskScheduler scheduler) : base(logger, localDb, scheduler)
        {
        }

        public async override void Save(IEnumerable<CollectionResult> results)
        {
            foreach (CollectionResult result in results) Save(result);
        }

        public async override void Save(CollectionResult result)
        {
            if (result == null) return;

            localDb.RemoveInstanceForeverAsync(result, logger);

        }


    }
}
