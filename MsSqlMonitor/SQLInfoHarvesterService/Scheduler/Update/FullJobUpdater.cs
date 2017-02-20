using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using SQLInfoCollectionService.Contracts;
using SQLInfoCollectionService.Scheduler;
using SQLInfoCollectionService.InstanceInfoUpdating;
using System.Data.SqlClient;
using DALLib.Contracts;
using DALLib.Models;

namespace SQLInfoCollectorService.Scheduler.Update
{
    class FullJobUpdater : BaseJobUpdater
    {
        public FullJobUpdater(InstanceInfoUpdater instanceInfoUpdater, ISLogger logger, IUnitOfWork unitOfWork, IConnectionManager connManager, IInstanceDataCollector instanceDataCollector) 
            : base(instanceInfoUpdater, logger, unitOfWork, connManager, instanceDataCollector)
        {
        }

        public override async Task<CollectionResult> UpdateJob(SchedulerJob job)
        {

            Instance instance = await unitOfWork.Instances.GetAsync(job.InstanceID);
            if (instance.IsDeleted) return null;

            CollectionResult result = new CollectionResult();
            result.InstanceID = job.InstanceID;

            SqlConnection connection = await connManager.OpenConnection(job.InstanceID, unitOfWork);
            if (connection == null)
            {
                logger.Error("can't open connection inctanceID =  " + job.InstanceID);
                result.InstanceInfo = null;
            } else
            {
                result.InstanceInfo = await instanceInfoUpdater.UpdateAsync(job.InstanceID, instanceDataCollector).ConfigureAwait(false);
                connManager.CloseConnection();
            }



            result.JobType = job.JobType;
            // result.Scheduler = job.Scheduler;
            result.JobSaver = job.JobSaver;

            
            
            if (result.InstanceInfo == null) logger.Error("Job type full update collect error for instance " + job.InstanceID);


            logger.Debug("end collect  full updat  id=" + job.InstanceID);

            return result;

        } 


    }
}
