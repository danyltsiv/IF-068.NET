using DALLib.EF;
using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETAPP.Extensions;
using ASPNETAPP.ViewModels;
using System.Data.Entity;
using Database = DALLib.Models.Database;
using CommonLib;
using SQLInfoCollectorService.Security;

namespace ASPNETAPP.DataProvider
{
    public class LocalDbDataProvider : IMonitorDataProvider
    {
        private ISLogger logger;
        private IEncryptionManager encryptionManager;

        public LocalDbDataProvider(ISLogger logger, IEncryptionManager encryptionManager)
        {
            this.logger = logger;
            this.encryptionManager = encryptionManager;
        }

        public async Task<InstanceViewModel> AddInstance(InstanceViewModel newInstance)
        {
            if (String.IsNullOrEmpty(newInstance.InstanceName) ||
                    String.IsNullOrEmpty(newInstance.ServerName) ||
                    String.IsNullOrEmpty(newInstance.Login) ||
                    String.IsNullOrEmpty(newInstance.Password) ||
                    !newInstance.IsWindowsAuthentication.HasValue)
                throw new InvalidModelException("Instance name, Server name, Login, Password, and authentication type are required.");

            string encryptionKey;
            var encryptedPassword = encryptionManager.Encrypt(out encryptionKey, newInstance.Password);

            using (var context = new MsSqlMonitorEntities())
            {
                if (null != await context.Instances.FirstOrDefaultAsync(inst => inst.ServerName.ToUpper() == newInstance.ServerName.ToUpper() 
                        && inst.InstanceName.ToUpper() == newInstance.InstanceName.ToUpper()))
                    throw new ObjectAlreadyExistException("Instance already exist in the system.");

                Instance result = new Instance() {
                    ServerName = newInstance.ServerName,
                    InstanceName = newInstance.InstanceName,
                    Password = encryptedPassword,
                    EncryptionKey = encryptionKey,
                    Login = newInstance.Login,
                    Authentication = newInstance.IsWindowsAuthentication.Value ? AuthenticationType.Windows : AuthenticationType.Sql };
                context.Instances.Add(result);
                await context.SaveChangesAsync();
                return result.ToViewModel();
            }
        }

        public async Task<InstanceViewModel> UpdateInstance(InstanceViewModel source)
        {
            if (!source.Id.HasValue)
                throw new InvalidModelException("Instance id is required.");

            using (var context = new MsSqlMonitorEntities())
            {
                Instance instToUpdate = await context.Instances.FirstOrDefaultAsync(inst => inst.Id == source.Id);
                if (instToUpdate == null)
                    throw new EntityNotExistException($"Instance with id '{source.Id}' that you try to update is not exist");
                if (!String.IsNullOrEmpty(source.ServerName))
                    instToUpdate.ServerName = source.ServerName;
                if (!String.IsNullOrEmpty(source.InstanceName))
                    instToUpdate.InstanceName = source.InstanceName;
                if (!String.IsNullOrEmpty(source.Login))
                    instToUpdate.Login = source.Login;
                if (!String.IsNullOrEmpty(source.Password))
                {
                    string encryptionKey;
                    instToUpdate.Password = encryptionManager.Encrypt(out encryptionKey, source.Password);
                    instToUpdate.EncryptionKey = encryptionKey;
                }
                if (source.IsWindowsAuthentication.HasValue)
                    instToUpdate.Authentication = source.IsWindowsAuthentication.Value
                        ? AuthenticationType.Windows
                        : AuthenticationType.Sql;
                await context.SaveChangesAsync();

                return instToUpdate.ToViewModel();
            }
        }

        public async Task<InstanceViewModel> DeleteInstance(int instanceId)
        {
            using(var context = new MsSqlMonitorEntities())
            {
                Instance instToDelete = await context.Instances.FirstOrDefaultAsync(inst => inst.Id == instanceId);
                if (instToDelete == null)
                    throw new EntityNotExistException($"Instance with id '{instanceId}' that you try to delete doesn't exist");
                instToDelete.IsDeleted = true;
                instToDelete.IsDeletedTime = DateTime.Now;
                await context.SaveChangesAsync();
                return instToDelete.ToViewModel();
            }
        }

        public async Task<InstanceViewModel> RecoverInstance(int instanceId)
        {
            using (var context = new MsSqlMonitorEntities())
            {
                Instance instToRecover = await context.Instances.FirstOrDefaultAsync(inst => inst.Id == instanceId);
                if (instToRecover == null)
                    throw new EntityNotExistException($"Instance with id '{instanceId}' that you are trying to recover doesn't exist");
                instToRecover.IsDeleted = false;
                await context.SaveChangesAsync();
                return instToRecover.ToViewModel();
            }
        }

        public async Task<InstanceViewModel> GetInstanceById(int instanceId)
        {
            using(var context = new MsSqlMonitorEntities())
            {
                Instance result = await context.Instances.FirstOrDefaultAsync(inst => inst.Id == instanceId);
                if (result == null)
                    throw new EntityNotExistException($"Instance with id {instanceId} doesn't exist.");
                return result.ToViewModel();
            }
        }

        public async Task<IEnumerable<InstanceLoginViewModel>> GetLogins(int instanceId)
        {
            using (MsSqlMonitorEntities context = new MsSqlMonitorEntities())
            {
                Instance instance = await context.Instances.FirstOrDefaultAsync(inst => inst.Id == instanceId);
                if (instance == null)
                    throw new EntityNotExistException($"Instance with Id '{instanceId}' doesn't exist.");
                return (await context.InstLogins.Where(login => login.InstanceId == instanceId)
                        .Include(login => login.Roles)
                        .ToListAsync()).Select(login => login.ToViewModel()).ToList();
            }
        }

        public async Task<IEnumerable<InstanceRoleViewModel>> GetInstanceRoles(int instanceId)
        {
            using (MsSqlMonitorEntities context = new MsSqlMonitorEntities())
            {
                Instance instance = await context.Instances.FirstOrDefaultAsync(inst => inst.Id == instanceId);
                if (instance == null)
                    throw new EntityNotExistException($"Instance with Id '{instanceId}' doesn't exist.");
                return (await context.InstRoles.Where(role => role.InstanceId == instanceId)
                        .Include(role => role.Logins)
                        .ToListAsync()).Select(role => role.ToViewModel()).ToList();
            }
        }

        public async Task<IEnumerable<PermissionViewModel>> GetInstancePrincipalPermissions(int principalId)
        {
            using (MsSqlMonitorEntities context = new MsSqlMonitorEntities())
            {
                InstPrincipal principal = await context.InstPrincipals.FirstOrDefaultAsync(p => p.Id == principalId);
                if (principal == null)
                    throw new EntityNotExistException($"InstancePrincipal with Id '{principalId}' doesn't exist.");
                return (await context.InstPermissions.Where(perm => perm.Principals.Select(princ => princ.Id).Contains(principalId))
                    .ToListAsync()).Select(p => p.ToViewModel()).ToList();
            }
        }
        
        public async Task<IEnumerable<DatabaseViewModel>> GetDatabases(int instanceId)
        {
            using(MsSqlMonitorEntities context = new MsSqlMonitorEntities())
            {
                Instance instance = await context.Instances.FirstOrDefaultAsync(inst => inst.Id == instanceId);
                if (instance == null)
                    throw new EntityNotExistException($"Instance with Id '{instanceId}' doesn't exist.");
                return (await context.Databases.Where(db => db.InstanceId == instanceId)
                    .ToListAsync()).Select(db => db.ToViewModel()).ToList();
            }
        }

        public async Task<IEnumerable<DatabaseUserViewModel>> GetDatabaseUsers(int databaseId)
        {
            using (MsSqlMonitorEntities context = new MsSqlMonitorEntities())
            {
                Database database = await context.Databases.FirstOrDefaultAsync(db => db.Id == databaseId);
                if (database == null)
                    throw new EntityNotExistException($"Database with Id '{databaseId}' doesn't exist.");
                return (await context.DbUsers.Where(user => user.DatabaseId == databaseId)
                    .Include(user => user.Roles).ToListAsync())
                    .Select(user => user.ToViewModel()).ToList();
            }
        }

        public async Task<IEnumerable<DatabaseRoleViewModel>> GetDatabaseRoles(int databaseId)
        {
            using (MsSqlMonitorEntities context = new MsSqlMonitorEntities())
            {
                Database database = await context.Databases.FirstOrDefaultAsync(db => db.Id == databaseId);
                if (database == null)
                    throw new EntityNotExistException($"Database with Id '{databaseId}' doesn't exist.");
                return (await context.DbRoles.Where(role => role.DatabaseId == databaseId)
                    .Include(role => role.Users).ToListAsync())
                    .Select(role => role.ToViewModel()).ToList();
            }
        }

        public async Task<IEnumerable<PermissionViewModel>> GetDatabasePrincipalPermissions(int principalId)
        {
            using (MsSqlMonitorEntities context = new MsSqlMonitorEntities())
            {
                DbPrincipal dbPrincipal = await context.DbPrincipals.FirstOrDefaultAsync(princ => princ.Id == principalId);
                if (dbPrincipal == null)
                    throw new EntityNotExistException($"Database principal with Id '{principalId}' doesn't exist.");
                return (await context.DbPermissions.Where(perm => perm.Principals.Select(princ => princ.Id).Contains(principalId)).ToListAsync())
                    .Select(perm => perm.ToViewModel()).ToList();
            }
        }

        public async Task<IEnumerable<UserViewModel>> GetAssignedUsers(int instanceId)
        {
            using(MsSqlMonitorEntities context = new MsSqlMonitorEntities())
            {
                Instance instance = await context.Instances.FirstOrDefaultAsync(inst => inst.Id == instanceId);
                if (instance == null)
                    throw new EntityNotExistException($"Instance with Id '{instanceId}' doesn't exist.");
                return (await context.Assigns
                    .Where(assign => assign.InstanceId == instanceId).Select(assign => assign.User).ToListAsync())
                    .Select(user => user.ToViewModel()).ToList();
            }
        }

        public async Task<IEnumerable<UserViewModel>> GetNotAssignedUsers(int instanceId)
        {
            using (MsSqlMonitorEntities context = new MsSqlMonitorEntities())
            {
                Instance instance = await context.Instances.FirstOrDefaultAsync(inst => inst.Id == instanceId);
                if (instance == null)
                    throw new EntityNotExistException($"Instance with Id '{instanceId}' doesn't exist.");
                var assignedUsersIds = await (from assign in context.Assigns where assign.InstanceId == instanceId select assign.UserId).ToListAsync();
                return (await context.Users.Where(user => !assignedUsersIds.Contains(user.Id)).ToListAsync())
                    .Select(user => user.ToViewModel()).ToList();
            }
        }

        public async Task GrantInstanceAccess(int instanceId, int userId)
        {
            using (MsSqlMonitorEntities context = new MsSqlMonitorEntities())
            {
                Assign assign = await context.Assigns.FirstOrDefaultAsync(a => a.InstanceId == instanceId && a.UserId == userId);
                if (assign != null)
                    throw new Exception($"User '{userId}' already has assign to instance '{instanceId}'");

                Instance instance = await context.Instances.FirstOrDefaultAsync(inst => inst.Id == instanceId);
                if (instance == null)
                    throw new EntityNotExistException($"There is no instance with id '{instanceId}' in the database");

                User user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                    throw new EntityNotExistException($"There is no user with id '{userId}' in the database");

                context.Assigns.Add(new Assign() { InstanceId = instanceId, UserId = userId });
                await context.SaveChangesAsync();
            }
        }

        public async Task RevokeInstanceAccess(int instanceId, int userId)
        {
            using (MsSqlMonitorEntities context = new MsSqlMonitorEntities())
            {
                Assign assign = await context.Assigns.FirstOrDefaultAsync(a => a.InstanceId == instanceId && a.UserId == userId);
                if (assign == null)
                    throw new EntityNotExistException($"Assignment of user '{userId}' to instance '{instanceId}' doesn't exist");
                context.Entry(assign).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }

        public async Task HideInstance(int instanceId, int userId)
        {
            using (MsSqlMonitorEntities context = new MsSqlMonitorEntities())
            {
                Assign assign = await context.Assigns.FirstOrDefaultAsync(a => a.InstanceId == instanceId && a.UserId == userId);
                if (assign == null)
                    throw new EntityNotExistException($"Assignment of user '{userId}' to instance '{instanceId}' doesn't exist");
                assign.IsHidden = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task ShowInstance(int instanceId, int userId)
        {
            using (MsSqlMonitorEntities context = new MsSqlMonitorEntities())
            {
                Assign assign = await context.Assigns.FirstOrDefaultAsync(a => a.InstanceId == instanceId && a.UserId == userId);
                if (assign == null)
                    throw new EntityNotExistException($"Assignment of user '{userId}' to instance '{instanceId}' doesn't exist");
                assign.IsHidden = false;
                await context.SaveChangesAsync();
            }
        }
    }
}
