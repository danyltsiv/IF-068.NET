using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectorService.Scheduler
{
    class SchedulerWorkItem
    {
        public int InstanceId { get; set; }
        public JobType.UpdateInfoType JobType { get; set; }

        public SchedulerWorkItem(int _instanceId, JobType.UpdateInfoType _jobType)
        {
            InstanceId = _instanceId;
            JobType = _jobType;
        }

        public override bool Equals(object obj)
        {
            var item = obj as SchedulerWorkItem;

            if (item == null)
            {
                return false;
            }

            return (this.InstanceId == item.InstanceId && this.JobType == item.JobType);
        }

        public override int GetHashCode()
        {
            return this.InstanceId * 3 + (int)this.JobType;
        }
    }
}
