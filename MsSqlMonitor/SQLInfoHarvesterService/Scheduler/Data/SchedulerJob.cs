using SQLInfoCollectionService.Contracts;
using SQLInfoCollectionService.Collectors;
using DALLib.Models;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using SQLInfoCollectionService.Entities;
using System;
using System.Collections.Generic;
using SQLInfoCollectionService.InstanceInfoUpdating.Entities;
using Microsoft.Practices.Unity;
using SQLInfoCollectionService.Configuration;
using Microsoft.Practices.ServiceLocation;
using DALLib.Impersonation;
using System.Threading.Tasks;
using SQLInfoCollectionService.InstanceInfoUpdating;
using SQLInfoCollectorService.Scheduler;
using DALLib.Contracts;
using CommonLib;
using SQLInfoCollectorService.Scheduler.Update;
using SQLInfoCollectorService.Scheduler.Save;


namespace SQLInfoCollectionService.Scheduler
{
    public class SchedulerJob
    {
        public int InstanceID { get; set; }
        public JobType.UpdateInfoType JobType { get; set; }

        public BaseJobUpdater JobUpdater { get; set; }

        public BaseJobSaver JobSaver { get; set; }

       // public SQLTaskScheduler Scheduler { get; set; }





        public SchedulerJob(int instanceID, BaseJobUpdater JobUpdater,  BaseJobSaver JobSaver,JobType.UpdateInfoType JobType)
        {
            this.InstanceID = instanceID;
            this.JobUpdater = JobUpdater;
            this.JobType = JobType;
           // this.Scheduler = Scheduler;
            this.JobSaver = JobSaver;
            this.JobUpdater = JobUpdater;

        }





    }
}
