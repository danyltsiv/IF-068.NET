using DALLib.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALLib.Models;
using DALLib.EF;

namespace DALLib.Repos
{
    class JobTypeRepository : IJobTypeRepository
    {
        protected MsSqlMonitorEntities context;

        private const int MINUTES_15 = 15*60; //900 seconds
        private const int MINUTES_2 = 2 * 60; //120 seconds


        public JobTypeRepository(MsSqlMonitorEntities context)
        {
            this.context = context;
        }


        public List<JobType> GetAll()
        {
            List<JobType> jobs = context.JobTypes.ToList<JobType>();

            if (jobs == null) jobs = new List<JobType>();
            if (jobs.Count == 0) jobs = addDefaultJobs();

            return jobs;
        }

        private new List<JobType> addDefaultJobs()
        {
            CreateJobTypeInDataBase(JobType.UpdateInfoType.Full, MINUTES_15);
            CreateJobTypeInDataBase(JobType.UpdateInfoType.RemoveInstances, MINUTES_15);
            CreateJobTypeInDataBase(JobType.UpdateInfoType.CheckStatus, MINUTES_2);

            return context.JobTypes.ToList<JobType>();
        }

        private void CreateJobTypeInDataBase(JobType.UpdateInfoType type,int repeatTime)
        {
            JobType jobType = new JobType();
            jobType.Type = type;
            jobType.MaxMutualWrites = Environment.ProcessorCount;
            jobType.MaxParallelReads = Environment.ProcessorCount;
            jobType.RepeatTimeSec = repeatTime;

            // DALLib.EF.MsSqlMonitorEntities dbContext = new DALLib.EF.MsSqlMonitorEntities();
            context.JobTypes.Add(jobType);
            context.SaveChanges();
        }
    }
}
