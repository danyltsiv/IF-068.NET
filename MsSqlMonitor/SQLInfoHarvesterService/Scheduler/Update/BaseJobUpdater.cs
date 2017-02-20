using CommonLib;
using DALLib.Contracts;
using SQLInfoCollectionService.Contracts;
using SQLInfoCollectionService.InstanceInfoUpdating;
using SQLInfoCollectionService.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectorService.Scheduler.Update
{
   public  abstract class BaseJobUpdater
    {
        protected ISLogger logger;    
        protected IConnectionManager connManager; 
        protected InstanceInfoUpdater instanceInfoUpdater;
        protected IUnitOfWork unitOfWork;
        protected IInstanceDataCollector instanceDataCollector;

        public BaseJobUpdater(InstanceInfoUpdater instanceInfoUpdater,ISLogger logger, IUnitOfWork unitOfWork, IConnectionManager connManager, IInstanceDataCollector instanceDataCollector)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.connManager = connManager;
            this.instanceInfoUpdater = instanceInfoUpdater;
            this.instanceDataCollector = instanceDataCollector;
        }

        public abstract  Task<CollectionResult> UpdateJob(SchedulerJob job);
    }
}
