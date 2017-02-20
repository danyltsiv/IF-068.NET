using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DALLib.Models;
using SQLInfoCollectionService.Configuration;
using SQLInfoCollectionService.Contracts;
using Microsoft.Practices.ServiceLocation;
using System.Configuration;
using System.Threading.Tasks.Schedulers;
using System.Threading.Tasks.Dataflow;
using SQLInfoCollectionService.Scheduler;
using DALLib.Repos;
using SQLInfoCollectionService.Collectors;
using System.Data.SqlClient;
using SQLInfoCollectorService.Scheduler;
using System.Collections.Concurrent;
using DALLib.Contracts;
using CommonLib;

using SQLInfoCollectionService.InstanceInfoUpdating;
using Microsoft.Practices.Unity;
using SQLInfoCollectorService.Scheduler.Update;
using SQLInfoCollectorService.Scheduler.Save;
using SQLInfoCollectorService.Security;

namespace SQLInfoCollectorService.Scheduler
{



    public class SQLTaskScheduler 
    {

        private const int ONE_THOUSAND = 1000;
        private readonly int MUTUAL_WRITES = Properties.Settings.Default.MUTUAL_WRITES;
        private readonly int PARALLEL_READS = Properties.Settings.Default.PARALLEL_READS;
        private readonly int TRIGGER_AFTER_MS = Properties.Settings.Default.TRIGGER_AFTER_MS; //triger batch block after 10 seconds

        private IEncryptionManager encryptionManager;
        private ISLogger logger;
        private IResourceManager resourceManager;
        private IUnitOfWork unitOfWork;


        //TPL DATAFLOW variables
        private QueuedTaskScheduler scheduler;


        //buffer for urgent  update tasks
        private BufferBlock<SchedulerJob> bufferBlockHighP;

        //buffer for common update tasks
        private BufferBlock<SchedulerJob> bufferBlockLowP;


        private System.Threading.Tasks.TaskScheduler highPriorityScheduler;  //scheduler for common updates
        private System.Threading.Tasks.TaskScheduler lowPriorityScheduler; //scheduler for refresh update

        //Dataflow options
        private ExecutionDataflowBlockOptions optionsReadHighP;
        private ExecutionDataflowBlockOptions optionsReadLowP;
        private ExecutionDataflowBlockOptions optionsWriteBlock;
        private GroupingDataflowBlockOptions optionsBatchBlock;
        private DataflowLinkOptions optionsLink;

        private TransformBlock<SchedulerJob, CollectionResult> highPriorityReadInfoBlock; //action block for update instance information
        private TransformBlock<SchedulerJob, CollectionResult> lowPriorityReadInfoBlock; //action block for urgent information update

        private BatchBlock<CollectionResult> batchBlock; //batch block to combine results into array from <MUTUAL_WRITES> elements
        private ActionBlock<CollectionResult[]> writeInfoBlock; //action block to save collected info

        private CancellationTokenSource cancelTokenSrc = new CancellationTokenSource();

        private System.Timers.Timer triggerBatchTimer; //uses to triger batch block, if posted jobs count less than <mutualWrites> 

        private ConcurrentDictionary<long, SchedulerWorkItem> nowCollecting = new ConcurrentDictionary<long, SchedulerWorkItem>();
        private long dictionaryKey = 0;

        private List<System.Timers.Timer> timers = new List<System.Timers.Timer>();
        private List<JobType> jobTypes;

        private IConnectionManager connManager;

        private JobWorkers jobWorkers;


        public SQLTaskScheduler(ISLogger logger, IResourceManager resourceManager, IUnitOfWork unitOfWork, IConnectionManager connManager, 
                               IEncryptionManager encryptionManager)
        {
            this.logger = logger;
            this.resourceManager = resourceManager;
            this.unitOfWork = unitOfWork;
            this.connManager = connManager;
            this.encryptionManager = encryptionManager;

            triggerBatchTimer = new System.Timers.Timer();
            triggerBatchTimer.Elapsed += OnTriggerBatch;
            triggerBatchTimer.AutoReset = true;
            triggerBatchTimer.Interval = TRIGGER_AFTER_MS;


            jobTypes = unitOfWork.JobTypes.GetAll();

            foreach (JobType jobType in jobTypes)
            {
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Elapsed += (sender, e) => TryStartDataFlow(jobType.Type);
                timer.Enabled = false;
                timer.Interval = jobType.RepeatTimeSec * ONE_THOUSAND;
                timer.AutoReset = true;

                timers.Add(timer);
            }




            jobWorkers = new JobWorkers(logger, resourceManager, unitOfWork,  this, encryptionManager);


        }




        public void InstanceUpdateFinished(int instanceID, JobType.UpdateInfoType jobType)
        {

            KeyValuePair<long, SchedulerWorkItem> pair = nowCollecting.FirstOrDefault(x => x.Value.InstanceId == instanceID && x.Value.JobType == jobType);

            if (pair.Value == null) return;

            SchedulerWorkItem value;
            nowCollecting.TryRemove(pair.Key, out value);

            logger.Debug("update finished ID= "+ value.InstanceId + " jobType="+value.JobType );
        }

        public void Start()
        {
            logger.Debug("Start scheduler  ");

            InitDataFlow();


            foreach (System.Timers.Timer timer in timers) timer.Enabled = true;

            triggerBatchTimer.Enabled = true;


            foreach (JobType jobType in jobTypes) TryStartDataFlow(jobType.Type);
        }


        public void Stop()
        {
            logger.Debug("Stop scheduler for jobtype ");

            //stop timers
            foreach (System.Timers.Timer timer in timers) timer.Enabled = false;

            triggerBatchTimer.Enabled = false;

            cancelTokenSrc.Cancel();
        }






        void InitDataFlow()
        {

            //Create schedulers
            scheduler = new QueuedTaskScheduler(System.Threading.Tasks.TaskScheduler.Default, PARALLEL_READS);
            highPriorityScheduler = scheduler.ActivateNewQueue(0);
            lowPriorityScheduler = scheduler.ActivateNewQueue(1);

            //create options
            optionsReadHighP = new ExecutionDataflowBlockOptions
            {
                TaskScheduler = highPriorityScheduler,
                MaxDegreeOfParallelism = PARALLEL_READS,
                CancellationToken = cancelTokenSrc.Token
            };

            //create options
            optionsReadHighP = new ExecutionDataflowBlockOptions
            {
                TaskScheduler = highPriorityScheduler,
                MaxDegreeOfParallelism = PARALLEL_READS,
                CancellationToken = cancelTokenSrc.Token
            };

            optionsReadLowP = new ExecutionDataflowBlockOptions
            {
                TaskScheduler = lowPriorityScheduler,
                MaxDegreeOfParallelism = PARALLEL_READS,
                CancellationToken = cancelTokenSrc.Token
            };


            optionsWriteBlock = new ExecutionDataflowBlockOptions
            {

                CancellationToken = cancelTokenSrc.Token
            };

            optionsBatchBlock = new GroupingDataflowBlockOptions
            {
                Greedy = true,
                CancellationToken = cancelTokenSrc.Token,

            };

            optionsLink = new DataflowLinkOptions { PropagateCompletion = true, };

           // CollectionInfoSaver collectionInfoSaver = new CollectionInfoSaver(logger);

            //create blocks
            bufferBlockHighP = new BufferBlock<SchedulerJob>();
            bufferBlockLowP = new BufferBlock<SchedulerJob>();

            highPriorityReadInfoBlock = new TransformBlock<SchedulerJob, CollectionResult>(async (sqlJob) => {

                if (sqlJob!=null)
                    if (sqlJob.JobUpdater != null) return await sqlJob.JobUpdater.UpdateJob(sqlJob);

                return null;
            }, optionsReadHighP);

            lowPriorityReadInfoBlock = new TransformBlock<SchedulerJob, CollectionResult>(async (sqlJob) => {

                if (sqlJob != null)
                    if (sqlJob.JobUpdater != null) return await sqlJob.JobUpdater.UpdateJob(sqlJob);

                return null;

            },  optionsReadLowP);

            batchBlock = new BatchBlock<CollectionResult>(1, optionsBatchBlock);

            writeInfoBlock = new ActionBlock<CollectionResult[]>(sqlInfoArray => ResultSaver.SaveResults(sqlInfoArray), optionsWriteBlock);


            //link blocks
            bufferBlockHighP.LinkTo(highPriorityReadInfoBlock, optionsLink);
            bufferBlockLowP.LinkTo(lowPriorityReadInfoBlock, optionsLink);

            highPriorityReadInfoBlock.LinkTo(batchBlock, optionsLink);
            lowPriorityReadInfoBlock.LinkTo(batchBlock, optionsLink);

            batchBlock.LinkTo(writeInfoBlock, optionsLink);


        }




        private void OnTriggerBatch(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (batchBlock != null) batchBlock.TriggerBatch();
            //  logger.Debug("trigger batch ");
        }

        void TryStartDataFlow(JobType.UpdateInfoType jobType)
        {
            if (bufferBlockLowP == null || lowPriorityReadInfoBlock == null || writeInfoBlock == null) return;

            logger.Debug("TryStartDataFlow " + jobType);

            //var allInstancesID = unitOfWork.Instances.GetAll().Where(i => i.IsDeleted == false).Select<Instance, int>(i => i.Id);
            var allInstancesID = unitOfWork.Instances.GetAll().Select<Instance, int>(i => i.Id);
            var curCollecting = nowCollecting.Where(i => i.Value.JobType == jobType).Select<KeyValuePair<long, SchedulerWorkItem>, int>(i => i.Value.InstanceId);

            var idToStartUpdate = allInstancesID.Except<int>(curCollecting);

            foreach (int id in idToStartUpdate)
            {
                SchedulerJob schedulerJob = new SchedulerJob(id, jobWorkers.GetUpdater(jobType), jobWorkers.GetSaver(jobType), jobType);

                while (!bufferBlockLowP.Post(schedulerJob)) { }

                nowCollecting.TryAdd( unchecked (dictionaryKey++), new SchedulerWorkItem(id, jobType));

               // logger.Debug("post to BufferBlock " + jobType + " instanceID=" + id);
            }
        }

        public void RefreshInstance(int id)
        {
            logger.Debug("refresh instance with id="+id);

            SchedulerJob schedulerJob = new SchedulerJob(id, jobWorkers.GetUpdater(JobType.UpdateInfoType.CheckStatus), jobWorkers.GetSaver(JobType.UpdateInfoType.CheckStatus), JobType.UpdateInfoType.CheckStatus);

            while (!bufferBlockHighP.Post(schedulerJob)) { }

            logger.Debug(" urgentInstanceUpdate=" + id);
        }
    }
}
