using DALLib.Contracts;
using DALLib.EF;
using DALLib.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DALLib.Repos
{
    

    public struct FilteredInstances
    {
        public int Page;
        public int PageCount;
        public IEnumerable<Instance> List;
    }


    public class InstanceRepository : BaseRepository<Instance>, IInstanceRepository
    {
        const int PAGE_SIZE = 5;

        


        public InstanceRepository(MsSqlMonitorEntities context) : base(context) { }

        public async Task<PaginatedInstances> GetInstancesAsync(int page, string nameFilter, string serverNameFilter, string versionFilter,string sqlverisons)
        {
            var tmpList = await context.Instances.ToListAsync();
            PaginatedInstances result = new PaginatedInstances();

            var list = tmpList.Where(p => CheckInstance(p, nameFilter, serverNameFilter, versionFilter,sqlverisons) && !p.IsDeleted);

            int pageCount = list.Count<Instance>() / PAGE_SIZE;
            if (pageCount * PAGE_SIZE < list.Count<Instance>()) pageCount++;

            if (page > pageCount) page = pageCount;
            if (page < 0) page = 0;

            int startElement = page * PAGE_SIZE;

            var resultList = list.Skip<Instance>(startElement).Take<Instance>(PAGE_SIZE);

            result.List = resultList;
            result.Page = page;
            result.PageCount = pageCount;
            result.PageSize = PAGE_SIZE;
            result.Versions = tmpList.Where(i => i.InstanceVersion!=null).Select<Instance,String>(i=> i.InstanceVersion.Version.Split('.')[0]).Distinct();

            return result;
        }

        public async Task<PaginatedInstances> GetDeletedInstancesAsync(int page, string nameFilter, string serverNameFilter, string versionFilter,string sqlversions)
        {
            var tmpList = await context.Instances.ToListAsync();
            PaginatedInstances result = new PaginatedInstances();

            var list = tmpList.Where(p => CheckInstance(p, nameFilter, serverNameFilter, versionFilter,sqlversions) && p.IsDeleted);

            int pageCount = list.Count<Instance>() / PAGE_SIZE;
            if (pageCount * PAGE_SIZE < list.Count<Instance>()) pageCount++;

            if (page > pageCount) page = pageCount;
            if (page < 0) page = 0;

            int startElement = page * PAGE_SIZE;

            var resultList = list.Skip<Instance>(startElement).Take<Instance>(PAGE_SIZE);

            result.List = resultList;
            result.Page = page;
            result.PageCount = pageCount;
            result.PageSize = PAGE_SIZE;
            result.Versions = resultList.Where(i => i.InstanceVersion != null).Select<Instance, String>(i => i.InstanceVersion.Version.Split('.')[0]).ToList().Distinct();

            return result;
        }

        public async Task<PaginatedInstances> GetAssignedInstancesAsync(int userID, int page, string nameFilter, string serverNameFilter, string versionFilter,string sqlversions)
        {
            List<int> idsList = await context.Assigns.Where(a => a.UserId == userID && !a.IsHidden).Select(g => g.InstanceId).ToListAsync();
            var tmpList = await context.Instances.Where(g => idsList.Any(i => i == g.Id) && !g.IsDeleted).ToListAsync();

            PaginatedInstances result = new PaginatedInstances();

            var list = tmpList.Where(p => CheckInstance(p, nameFilter, serverNameFilter, versionFilter,sqlversions));

            int pageCount = list.Count<Instance>() / PAGE_SIZE;
            if (pageCount * PAGE_SIZE < list.Count<Instance>()) pageCount++;

            if (page > pageCount) page = pageCount;
            if (page < 0) page = 0;

            int startElement = page * PAGE_SIZE;

            var resultList = list.Skip<Instance>(startElement).Take<Instance>(PAGE_SIZE);

            result.List = resultList;
            result.Page = page;
            result.PageCount = pageCount;
            result.PageSize = PAGE_SIZE;
            result.Versions = resultList.Where(i => i.InstanceVersion != null).Select<Instance, String>(i => i.InstanceVersion.Version.Split('.')[0]).ToList().Distinct();

            return result;
        }

        public async Task<PaginatedInstances> GetNotAssignedInstancesAsync(int userID, int page, string nameFilter, string serverNameFilter, string versionFilter, string sqlversions)
        {
            List<int> idsList = await context.Assigns.Where(a => a.UserId == userID && !a.IsHidden).Select(g => g.InstanceId).ToListAsync();
            var tmpList = await context.Instances.Where(g => idsList.Any(i => i != g.Id) && !g.IsDeleted).ToListAsync();

            PaginatedInstances result = new PaginatedInstances();

            var list = tmpList.Where(p => CheckInstance(p, nameFilter, serverNameFilter, versionFilter, sqlversions));

            int pageCount = list.Count<Instance>() / PAGE_SIZE;
            if (pageCount * PAGE_SIZE < list.Count<Instance>()) pageCount++;

            if (page > pageCount) page = pageCount;
            if (page < 0) page = 0;

            int startElement = page * PAGE_SIZE;

            var resultList = list.Skip<Instance>(startElement).Take<Instance>(PAGE_SIZE);

            result.List = resultList;
            result.Page = page;
            result.PageCount = pageCount;
            result.PageSize = PAGE_SIZE;
            result.Versions = resultList.Where(i => i.InstanceVersion != null).Select<Instance, String>(i => i.InstanceVersion.Version.Split('.')[0]).ToList().Distinct();

            return result;
        }


        public async Task<PaginatedInstances> GetAssignedHidenInstancesAsync(int userID, int page, string nameFilter, string serverNameFilter, string versionFilter,string sqlversions)
        {
            List<int> ids = await context.Assigns.Where(a => a.UserId == userID && a.IsHidden).Select(g => g.InstanceId).ToListAsync();
            var instances = await context.Instances.Where(g => ids.Any(i => i == g.Id && !g.IsDeleted)).ToListAsync();

            PaginatedInstances result = new PaginatedInstances();

            var list = instances.Where(p => CheckInstance(p, nameFilter, serverNameFilter, versionFilter,sqlversions));

            int pageCount = list.Count<Instance>() / PAGE_SIZE;
            if (pageCount * PAGE_SIZE < list.Count<Instance>()) pageCount++;

            if (page > pageCount) page = pageCount;
            if (page < 0) page = 0;

            int startElement = page * PAGE_SIZE;

            var resultList = list.Skip<Instance>(startElement).Take<Instance>(PAGE_SIZE);

            result.List = resultList;
            result.Page = page;
            result.PageCount = pageCount;
            result.PageSize = PAGE_SIZE;
            result.Versions = resultList.Where(i => i.InstanceVersion != null).Select<Instance, String>(i => i.InstanceVersion.Version.Split('.')[0]).ToList().Distinct();

            return result;
        }

        public async Task<PaginatedInstances> GetAssignedDeletedInstancesAsync(int userID, int page, string nameFilter, string serverNameFilter, string versionFilter,string sqlversions)
        {
            List<int> ids = await context.Assigns.Where(a => a.UserId == userID).Select(g => g.InstanceId).ToListAsync();
            var instances = await context.Instances.Where(g => ids.Any(i => i == g.Id && g.IsDeleted)).ToListAsync();

            PaginatedInstances result = new PaginatedInstances();

            var list = instances.Where(p => CheckInstance(p, nameFilter, serverNameFilter, versionFilter,sqlversions));

            int pageCount = list.Count<Instance>() / PAGE_SIZE;
            if (pageCount * PAGE_SIZE < list.Count<Instance>()) pageCount++;

            if (page > pageCount) page = pageCount;
            if (page < 0) page = 0;

            int startElement = page * PAGE_SIZE;

            var resultList = list.Skip<Instance>(startElement).Take<Instance>(PAGE_SIZE);

            result.List = resultList;
            result.Page = page;
            result.PageCount = pageCount;
            result.PageSize = PAGE_SIZE;
            result.Versions = resultList.Where(i => i.InstanceVersion != null).Select<Instance, String>(i => i.InstanceVersion.Version.Split('.')[0]).ToList().Distinct();

            return result;
        }

        public override Instance Delete(int id)
        {
            Instance i = table.Find(id);
            if (i != null)
            {
                i.IsDeleted = true;
                i.IsDeletedTime = DateTime.Now;
                context.SaveChanges();
            }

            return i;
        }

        public  Instance DeleteForever(int id)
        {
            Instance item = table.Find(id);
            return table.Remove(item);
        }

        public bool CheckInstance(Instance instance, string nameFilter, string serverNameFilter, string versionFilter,string sqlversion)
        {
            bool result = true;

            if (!string.IsNullOrEmpty(nameFilter)) result = result && instance.InstanceName.Contains(nameFilter);
            if (!string.IsNullOrEmpty(serverNameFilter)) result = result && instance.ServerName.Contains(nameFilter);
            if (!string.IsNullOrEmpty(versionFilter) && instance.InstanceVersion!=null) result = result && instance.InstanceVersion.Version.Contains(versionFilter);

            if (instance.InstanceVersion != null)
            {
                SQLVersion version = SQLVersions.GetSQLVersionFromString(instance.InstanceVersion.Version);
                result = result && SQLVersions.IsVersionInlist(version, sqlversion);
            }

            return result;
        }

        public bool IsInstanceExist(string serverName, string instanceName)
        {
            return table.Any(g => g.InstanceName == instanceName && g.ServerName == serverName);
        }

        public InstanceVersion GetVersionByInstanceId(int instanceId)
        {

            Instance instance = Get(instanceId);

            if (instance != null) return instance.InstanceVersion;

            return null;
            //return context.InstanceVersions.FirstOrDefault(g => g.InstanceId == instanceId);
        }

        public IEnumerable<InstLogin> GetInstLoginsByInstanceId(int instanceId)
        {
            var result = table.FirstOrDefault(g => g.Id == instanceId);

            if (result != null)
            {
                return result.Logins;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<InstRole> GetInstRolesByInstanceId(int instanceId)
        {
            var result = table.FirstOrDefault(g => g.Id == instanceId);

            if (result != null)
            {
                return result.Roles;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IsInstanceExistAsync(string serverName, string instanceName)
        {
            return await table.AnyAsync(g => g.InstanceName == instanceName && g.ServerName == serverName);
        }

        public async Task<InstanceVersion> GetVersionByInstanceIdAsync(int instanceId)
        {
            // return await context.InstanceVersions.FirstOrDefaultAsync(g => g.InstanceId == instanceId);
            var instance = await context.Instances.FirstOrDefaultAsync(i => i.Id == instanceId);

            if (instance != null)
                return instance.InstanceVersion;

            return null;
        }

        public async Task<IEnumerable<InstLogin>> GetInstLoginsByInstanceIdAsync(int instanceId)
        {
            var result = await table.FirstOrDefaultAsync(g => g.Id == instanceId);

            if (result != null)
            {
                return result.Logins;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<InstRole>> GetInstRolesByInstanceIdAsync(int instanceId)
        {
            var result = await table.FirstOrDefaultAsync(g => g.Id == instanceId);

            if (result != null)
            {
                return result.Roles;
            }
            else
            {
                return null;
            }
        }
    }
}
