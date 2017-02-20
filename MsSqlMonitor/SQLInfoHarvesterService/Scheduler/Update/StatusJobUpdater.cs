using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using SQLInfoCollectionService.Contracts;
using SQLInfoCollectionService.Scheduler;
using SQLInfoCollectionService.InstanceInfoUpdating;
using DALLib.Contracts;
using System.Data.SqlClient;

namespace SQLInfoCollectorService.Scheduler.Update
{
    class StatusJobUpdater : BaseJobUpdater
    {
        public StatusJobUpdater(InstanceInfoUpdater instanceInfoUpdater, ISLogger logger, IUnitOfWork unitOfWork, IConnectionManager connManager, IInstanceDataCollector instanceDataCollector) : 
            base(instanceInfoUpdater, logger, unitOfWork, connManager, instanceDataCollector)
        {
        }

        public override async Task<CollectionResult> UpdateJob(SchedulerJob job)
        {
           // logger.Debug("start collect status job  id=" + job.InstanceID);
            
            



 
            


            CollectionResult result = new CollectionResult();
            result.InstanceID = job.InstanceID;

            SqlConnection connection = await connManager.OpenConnection(job.InstanceID, unitOfWork);
            if (connection == null)
            {
                logger.Error("can't open connection inctanceID =  " + job.InstanceID);
                result.InstanceInfo = null;
            }
            else
            {
                result.InstanceInfo = await instanceInfoUpdater.UpdateStatusOnlyAsync(job.InstanceID, instanceDataCollector).ConfigureAwait(false);
                connManager.CloseConnection();
            }
            
            result.JobType = job.JobType;
            //result.Scheduler = job.Scheduler;
            result.JobSaver = job.JobSaver;


            

            if (result.InstanceInfo == null) logger.Error("Job type status update collect error for instance " + job.InstanceID);


            logger.Debug("end collect  status update  id=" + job.InstanceID);

            return result;
        }
    }
}
