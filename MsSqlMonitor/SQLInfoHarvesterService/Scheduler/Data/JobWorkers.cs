using CommonLib;
using DALLib.Contracts;
using DALLib.Models;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SQLInfoCollectionService.Configuration;
using SQLInfoCollectionService.Contracts;
using SQLInfoCollectionService.Entities;
using SQLInfoCollectionService.InstanceInfoUpdating;
using SQLInfoCollectorService.Scheduler.Save;
using SQLInfoCollectorService.Scheduler.Update;
using SQLInfoCollectorService.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectorService.Scheduler
{
    

    class JobWorkers
    {
        private Dictionary<JobType.UpdateInfoType, BaseJobSaver> saveWorkers = new Dictionary<JobType.UpdateInfoType, BaseJobSaver>();
        //private Dictionary<JobType.UpdateInfoType, BaseJobUpdater> updateWorkers = new Dictionary<JobType.UpdateInfoType, BaseJobUpdater>();

        ISLogger logger;
        IResourceManager resourceManager;
        IUnitOfWork unitOfWork;
        SQLTaskScheduler scheduler;
        private IEncryptionManager encryptionManager;

        public JobWorkers(ISLogger logger, IResourceManager resourceManager, IUnitOfWork unitOfWork,
            SQLTaskScheduler scheduler, IEncryptionManager encryptionManager)
        {

            this.logger = logger;
            this.resourceManager = resourceManager;
            this.unitOfWork = unitOfWork;
            this.scheduler = scheduler;
            this.encryptionManager = encryptionManager;

    

            ILocalStorage localDb = ServiceLocator.Current.GetInstance<ILocalStorage>();

           // BaseJobUpdater fullJobUpdater = new FullJobUpdater(instanceInfoUpdater, logger, unitOfWork, connManager, instanceDataCollector);
           // BaseJobUpdater statusJobUpdater = new StatusJobUpdater(instanceInfoUpdater, logger, unitOfWork, connManager, instanceDataCollector);


            BaseJobSaver fullJobSaver = new FullJobSaver(logger, localDb, scheduler);
            BaseJobSaver statusJobSaver = new StatusJobSaver(logger, localDb, scheduler);
            BaseJobSaver removeJobSaver = new RemoveJobSaver(logger, localDb, scheduler);


            saveWorkers.Add(JobType.UpdateInfoType.Full, fullJobSaver);
            saveWorkers.Add(JobType.UpdateInfoType.CheckStatus, statusJobSaver);
            saveWorkers.Add(JobType.UpdateInfoType.RemoveInstances, removeJobSaver);


            //updateWorkers.Add(JobType.UpdateInfoType.Full, fullJobUpdater);
            // updateWorkers.Add(JobType.UpdateInfoType.CheckStatus, statusJobUpdater);

        }

        public BaseJobUpdater GetUpdater(JobType.UpdateInfoType type)
        {
            //if (updateWorkers.ContainsKey(type)) return updateWorkers[type];

            IConnectionManager connManager = new ConnectionManager(logger, encryptionManager);
            InstanceInfoUpdater instanceInfoUpdater = new InstanceInfoUpdater(logger);

            IInstanceDataCollector instanceDataCollector = DependencyConfig.Initialize().Resolve<IInstanceDataCollector>(
                                                               new ParameterOverride("connManager", connManager),
                                                               new ParameterOverride("resourceManager", resourceManager),
                                                               new ParameterOverride("logger", logger));

            //logger.Debug("instanceDataCollector = "+ instanceDataCollector.GetHashCode());

            if (type == JobType.UpdateInfoType.Full) return new FullJobUpdater(instanceInfoUpdater, logger, unitOfWork, connManager, instanceDataCollector);
            if (type == JobType.UpdateInfoType.CheckStatus) return new StatusJobUpdater(instanceInfoUpdater, logger, unitOfWork, connManager, instanceDataCollector);
            if (type == JobType.UpdateInfoType.RemoveInstances) return new RemoveJobUpdater(instanceInfoUpdater, logger, unitOfWork, connManager, instanceDataCollector);

            return null;
        }


        public BaseJobSaver GetSaver(JobType.UpdateInfoType type)
        {
            if (saveWorkers.ContainsKey(type)) return saveWorkers[type];

            return null;
        }





    }
}
