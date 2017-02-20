using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALLib.Models;
using SQLInfoCollectionService.Contracts;
using SQLInfoCollectionService.Entities;

namespace SQLInfoCollectionService.DataUpdating.Keepers
{
    using Collectors;
    using Contracts;
    using Entities;
    using CommonLib;
    using HelpClasses;
    using SQLInfoCollectorService;

    public class InstanceDataKeeper : IInfoKeeper
    {
        private ISLogger logger;
        private readonly InstanceInfoHelper instanceInfoHelper;
        private readonly LocalDbHelper localDbHelper;

        public InstanceDataKeeper(ISLogger logger)
        {
            this.logger = logger;
            instanceInfoHelper = new InstanceInfoHelper();
            localDbHelper = new LocalDbHelper();
        }

        public async Task SaveAsync(IEnumerable<InstanceInfo> data)
        {
            await localDbHelper.SaveInstancesAsync(data.ToArray());
        }

        public async Task<InstanceInfo> UpdateAsync(Instance sourceModel)
        {
             ResourceManager resouceManager = new ResourceManager();
            using (ConnectionManager connectionManager = new ConnectionManager(logger))
            {
                await connectionManager.OpenConnection(sourceModel);
                InstanceDataCollector collector = new InstanceDataCollector(connectionManager, resouceManager, logger);
                return await instanceInfoHelper.UpdateAsync(sourceModel, collector);
            }
        }
    }
}
