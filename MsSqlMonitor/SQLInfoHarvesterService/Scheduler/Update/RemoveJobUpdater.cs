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
    public class RemoveJobUpdater : BaseJobUpdater
    {
        public int TIME_TO_SAVE_DELETED_INSECONDS = 60*5;

        public RemoveJobUpdater(InstanceInfoUpdater instanceInfoUpdater, ISLogger logger, IUnitOfWork unitOfWork, IConnectionManager connManager, 
                                  IInstanceDataCollector instanceDataCollector) : 
                                          base(instanceInfoUpdater, logger, unitOfWork, connManager, instanceDataCollector)
        {
        }

        public async override Task<CollectionResult> UpdateJob(SchedulerJob job)
        {

            var instance = await unitOfWork.Instances.GetAsync(job.InstanceID);

            if (instance != null)
                if (instance.IsDeleted && (DateTime.Now - instance.IsDeletedTime).Value.TotalSeconds > TIME_TO_SAVE_DELETED_INSECONDS)
                {
                    logger.Debug("RemoveJobUpdater found instance "+ job.InstanceID);

                    CollectionResult result = new CollectionResult();
                    result.InstanceID = job.InstanceID;
                    result.JobType = job.JobType;
                    result.JobSaver = job.JobSaver;
                    return result;
                }


            return null;

        }
    }
}
