using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using SQLInfoCollectionService.InstanceInfoUpdating.Entities;
using DALLib.Models;
using SQLInfoCollectorService.Scheduler;

using SQLInfoCollectorService.Scheduler.Save;

namespace SQLInfoCollectionService.Scheduler
{
    public class CollectionResult
    {

        public int InstanceID { get; set; }
        public InstanceInfo InstanceInfo { get; set; }

        public BaseJobSaver JobSaver { get; set; }

         public JobType.UpdateInfoType JobType { get; set; }

        //  public SQLTaskScheduler Scheduler { get; set; }



    }



}
