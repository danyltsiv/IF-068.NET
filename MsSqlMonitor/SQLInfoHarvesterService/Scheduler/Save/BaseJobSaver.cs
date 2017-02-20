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
    public abstract class BaseJobSaver
    {

        protected ISLogger logger;
        protected ILocalStorage localDb;
        protected SQLTaskScheduler scheduler;

        public BaseJobSaver(ISLogger logger, ILocalStorage localDb, SQLTaskScheduler scheduler)
        {
            this.localDb = localDb;
            this.logger = logger;
            this.scheduler = scheduler;
        }



        public abstract  void Save(CollectionResult result);
        public abstract void Save(IEnumerable<CollectionResult> results);
    }
}
