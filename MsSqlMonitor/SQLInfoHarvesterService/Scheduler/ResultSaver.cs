using SQLInfoCollectionService.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectorService.Scheduler
{
    class ResultSaver
    {
        public static void SaveResults(IEnumerable<CollectionResult> results)
        {
            foreach (CollectionResult result in results)
                if (result!=null) result.JobSaver.Save(result);
        }
    }
}
